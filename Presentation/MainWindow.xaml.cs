using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HashCore;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Presentation;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private uint firstFileHash;
    private string firstFileName;


    public MainWindow()
    {
        InitializeComponent();
    }

    private void FirstFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog();

        if (fileDialog.ShowDialog() == true)
        {
            firstFileName = fileDialog.FileName;
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

    private void WordCollisionButton_OnClick(object sender, RoutedEventArgs e)
    {
        var size = GetResultSize(ResultSize.SelectedIndex);
        FileInfo fi = new FileInfo(firstFileName);

        if (fi.Extension == ".cs")
        {
            var collisionFileName = "collisionFile" + fi.Extension;
            var tempFileName = "temp" + fi.Extension;

            File.Copy(firstFileName, tempFileName, true);

            while (true)
            {
                var text = File.ReadAllText(tempFileName) + $"\n// {Guid.NewGuid()}";
                File.WriteAllText(collisionFileName, text);

                var bytes = File.ReadAllBytes(collisionFileName);

                var cipher = new HashCipher();

                var fileHash = cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex));

                MessageBox.Show("new file hash = " +
                                Convert.ToString(fileHash, 2).PadLeft(GetResultSize(ResultSize.SelectedIndex), '0'));

                if (firstFileHash == fileHash)
                {
                    MessageBox.Show("Done");

                    break;
                }
            }

            File.Delete(tempFileName);
        }
        else
        {
            var task = Task.Run(() =>
            {
                var collisionFileName = "collisionFile" + fi.Extension;
                var tempFileName = "temp" + fi.Extension;

                File.Copy(firstFileName, tempFileName, true);

                while (true)
                {
                    using (var document = DocX.Load(tempFileName))
                    {
                        document.AddCustomProperty(new CustomProperty("my custom property", Guid.NewGuid().ToString()));
                        var props = document.CustomProperties;
                        document.SaveAs(collisionFileName);
                    }

                    var bytes = File.ReadAllBytes(collisionFileName);

                    var cipher = new HashCipher();

                    var fileHash = cipher.GetDigest(bytes, size);

                    if (firstFileHash == fileHash)
                    {
                        MessageBox.Show("Done");

                        break;
                    }
                }

                File.Delete(tempFileName);
            });
        }

    }

    private void CollisionPhotoButton_OnClick(object sender, RoutedEventArgs e)
    {
        FileInfo fi = new FileInfo(firstFileName);

        var collisionFileName = "collisionFile" + fi.Extension;
        var tempFileName = "temp" + fi.Extension;

        File.Copy(firstFileName, tempFileName, true);

        while (true)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(tempFileName);

            PropertyItem[] properties = originalImage.PropertyItems;

            Bitmap bmpImage = new Bitmap((Bitmap) originalImage);

            originalImage.Dispose();


            PropertyItem item = properties[0];

            string sTmp = Guid.NewGuid().ToString();
            var itemData = System.Text.Encoding.UTF8.GetBytes(sTmp);
            itemData[itemData.Length - 1] = 0;
            item.Type = 2;
            item.Id = 315;
            item.Len = itemData.Length;
            item.Value = itemData;
            bmpImage.SetPropertyItem(item);

            bmpImage.Save(collisionFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            bmpImage.Dispose();


            var bytes = File.ReadAllBytes(collisionFileName);

            var cipher = new HashCipher();

            var fileHash = cipher.GetDigest(bytes, GetResultSize(ResultSize.SelectedIndex));

            MessageBox.Show("new file hash = " +
                            Convert.ToString(fileHash, 2).PadLeft(GetResultSize(ResultSize.SelectedIndex), '0'));

            if (firstFileHash == fileHash)
            {
                MessageBox.Show("Done");

                break;
            }
        }

        File.Delete(tempFileName);
    }
}