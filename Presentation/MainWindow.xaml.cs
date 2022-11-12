﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HashCore;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace Presentation;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private uint firstFileHash;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void FirstFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog();

        if (fileDialog.ShowDialog() == true)
        {
            var bytes = File.ReadAllBytes(fileDialog.FileName);

            var cipher = new HashCipher();

            FirstFileName.Content = fileDialog.FileName.Split('\\').Last();
            firstFileHash = cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex));

            var result = Convert.ToString(firstFileHash, 2).PadLeft(GetResultSize(ResultSize.SelectedIndex), '0');

            FirstFileHash.Content = $"Hash = {result}";
        }
    }

    private void SecondFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog();

        if (fileDialog.ShowDialog() == true)
        {
            var bytes = File.ReadAllBytes(fileDialog.FileName);

            var cipher = new HashCipher();

            SecondFileName.Content = fileDialog.FileName.Split('\\').Last();

            var result = Convert.ToString(
                cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex)), 2)
                .PadLeft(GetResultSize(ResultSize.SelectedIndex), '0');

            SecondFileHash.Content = $"Hash = {result}";
        }
    }

    private void DirectoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog();
        if (dialog.ShowDialog(this).GetValueOrDefault()) WalkDirectoryTree(new DirectoryInfo(dialog.SelectedPath));
    }

    private int GetResultSize(int selectedIndex)
    {
        return selectedIndex switch
        {
            0 => 2,
            1 => 4,
            2 => 8,
            _ => 4
        };
    }

    private void ResultSize_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FirstFileName.Content = "";
        FirstFileHash.Content = "";

        SecondFileName.Content = "";
        SecondFileHash.Content = "";
    }

    private void WalkDirectoryTree(DirectoryInfo root)
    {
        FileInfo[] files = null;
        DirectoryInfo[] subDirs = null;

        try
        {
            files = root.GetFiles("*.*");
        }

        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine(e.Message);
        }

        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        if (files != null)
        {
            foreach (var fi in files)
            {
                var bytes = File.ReadAllBytes(fi.FullName);

                var hash = new HashCipher().GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex));

                if (firstFileHash == hash) MessageBox.Show(fi.Name);
            }

            subDirs = root.GetDirectories();

            foreach (var dirInfo in subDirs) WalkDirectoryTree(dirInfo);
        }
    }
}