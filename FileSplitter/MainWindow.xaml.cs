﻿#region

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

#endregion

namespace FileSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int BufferSize = 1 * FileSplitterContent.M;

        private FileSplitterContent dataContext = FileSplitterContent.GetInstance();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.dataContext;
            this.dataContext.IsDecomposeFile = true;
            this.dataContext.IsComposeFile = false;
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            if (this.dataContext.IsComposeFile == true)
            {
                fileDialog.Multiselect = true;
            }
            if (fileDialog.ShowDialog() == true)
            {
                this.dataContext.FileList.Clear();
                foreach (var item in fileDialog.FileNames)
                {
                    this.dataContext.FileList.Add(item);
                }
            }
            Console.WriteLine("");
        }

        private void Splitter_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(() =>
            {
                if (this.dataContext.IsDecomposeFile == true)
                {
                    this.DecomposeFile();
                }
                else if (this.dataContext.IsComposeFile == true)
                {
                    this.ComposeFile();
                }
            });
            task.Start();
        }

        private void DecomposeFile()
        {
            foreach (var item in this.dataContext.FileList)
            {
                var filePaht = item.Replace('/', '\\');
                var index = item.LastIndexOf('\\');
                var fileName = item.Substring(index + 1);
                Int32 num = 1;
                if (this.dataContext.LimitSize <= 0)
                {
                    this.dataContext.Size = "10";
                }
                using (var stream = new FileStream(item, FileMode.Open, FileAccess.Read))
                {
                    Int32 length = -1;
                    Byte[] buffer = new Byte[BufferSize];
                    Int32 totleLenght = 0;
                    length = stream.Read(buffer, 0, buffer.Length);
                    while (length != 0)
                    {
                        var offset = 0;
                        if (((this.dataContext.LimitSize * FileSplitterContent.M) - totleLenght) < length)
                        {
                            Int32 count = 0;
                            count = (this.dataContext.LimitSize * FileSplitterContent.M) - totleLenght;
                            totleLenght = 0;
                            using (var writer = new FileStream(fileName + "_" + num.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                writer.Position = writer.Length;
                                writer.Write(buffer, 0, count);
                            }
                            offset = count;
                            num++;
                        }
                        using (var writer = new FileStream(fileName + "_" + num.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            writer.Position = writer.Length;
                            writer.Write(buffer, offset, length - offset);
                            totleLenght += (length - offset);
                        }
                        length = stream.Read(buffer, 0, buffer.Length);
                    }
                    MessageBox.Show("文件分解完成", "提示", MessageBoxButton.OK);
                }
            }
        }

        private void ComposeFile()
        {
            var fileLsit = this.Sort();
            List<String> list = new List<String>();
            foreach (var item in fileLsit)
            {
                var index = item.LastIndexOf('_');
                var fileName = item.Substring(0, index);
                index = fileName.Replace('/', '\\').LastIndexOf('\\');
                var name = fileName.Substring(index + 1);
                if (list.Contains(fileName) == false)
                {
                    list.Add(fileName);
                    if (File.Exists(fileName) == true)
                    {
                        if (MessageBox.Show($"文件已存在，是否继续？{Environment.NewLine}继续将删除此文件！！！", "警告", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                        {
                            File.Delete(fileName);
                        }
                    }
                    foreach (var file in fileLsit)
                    {
                        if (file.StartsWith(fileName))
                        {
                            using (var reader = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                Int32 read = -1;
                                Byte[] buffer = new Byte[BufferSize];
                                read = reader.Read(buffer, 0, buffer.Length);
                                while (read != 0)
                                {
                                    try
                                    {
                                        using (var writer = new FileStream(name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                        {
                                            writer.Position = writer.Length;
                                            writer.Write(buffer, 0, read);
                                            writer.Flush();
                                        }
                                        read = reader.Read(buffer, 0, buffer.Length);
                                    }
                                    catch (Exception ex)
                                    {
                                        Thread.Sleep(10);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MessageBox.Show("文件合成完成", "提示", MessageBoxButton.OK);
        }

        private List<String> Sort()
        {
            List<String> result = new List<String>();
            result.AddRange(this.dataContext.FileList);
            return result;

        }
    }
}
