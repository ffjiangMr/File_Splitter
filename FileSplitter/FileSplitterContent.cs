/// <summary>
/// 界面的上下文实体类
/// </summary>
namespace FileSplitter
{
    #region using directive

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    #endregion

    internal sealed class FileSplitterContent : INotifyPropertyChanged
    {
        #region 私有构造函数 以实现 单例

        private FileSplitterContent()
        {
        }

        #endregion

        #region  实现单例

        private static FileSplitterContent entity;

        public static FileSplitterContent GetInstance()
        {
            if (entity == null)
            {
                entity = new FileSplitterContent();
            }
            return entity;
        }

        #endregion

        public const Int32 M = 1024 * 1024;

        public event PropertyChangedEventHandler PropertyChanged;

        public Boolean? IsComposeFile
        {
            get

            {
                return this.isComposeFile;
            }
            set
            {
                this.isComposeFile = value;
                this.UpdateProperty("IsComposeFile");
            }
        }

        public Boolean? IsDecomposeFile
        {
            get
            {
                return this.isDecomposeFile;
            }
            set
            {
                this.isDecomposeFile = value;
                this.UpdateProperty("IsDecomposeFile");
            }
        }

        public ObservableCollection<String> FileList
        {

            get
            {
                return this.fileList;
            }
            set
            {
                this.fileList = value;
                this.UpdateProperty("FileList");
            }
        }

        public String Size
        {
            get
            {
                return this.size.ToString();
            }
            set
            {

                if (Int32.TryParse(value, out this.size))
                {
                    this.UpdateProperty("Size");
                }
            }
        }

        public Int32 LimitSize  { get { return this.size; } }

        public void UpdateProperty(String propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region private field

        private Boolean? isComposeFile;

        private Boolean? isDecomposeFile;

        private Int32  size = 0;

        private ObservableCollection<String> fileList = new ObservableCollection<String>();

        #endregion

    }
}
