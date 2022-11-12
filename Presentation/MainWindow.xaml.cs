using System;
using System.IO;
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
    private int firstFileHash;

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

            FirstFileName.Content = fileDialog.FileName;
            firstFileHash = cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex));
            FirstFileHash.Content = $"Hash = {firstFileHash}";
        }
    }

    private void SecondFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog();

        if (fileDialog.ShowDialog() == true)
        {
            var bytes = File.ReadAllBytes(fileDialog.FileName);

            var cipher = new HashCipher();

            SecondFileName.Content = fileDialog.FileName;
            SecondFileHash.Content = $"Hash = {cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex))}";
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