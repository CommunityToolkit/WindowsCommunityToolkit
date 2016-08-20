using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.Data
{
    public class FileSystemViewModel : INotifyPropertyChanged
    {
        public IStorageItem StorageItem { get; set; }

        /// <summary>
        /// Gets name of StorageItem
        /// </summary>
        public string Text => StorageItem.Name;

        public Symbol StatusSymbol
        {
            get
            {
                if (StorageItem is StorageFolder)
                {
                    return Symbol.Folder;
                }

                return Symbol.Pictures;
            }
        }

        private ObservableCollection<FileSystemViewModel> _children;

        public ObservableCollection<FileSystemViewModel> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<FileSystemViewModel>();
                    LoadChildren();
                }

                return _children;
            }
        }

        private async void LoadChildren()
        {
            StorageFolder storageFolder = StorageItem as StorageFolder;
            if (storageFolder == null)
            {
                return;
            }

            IReadOnlyList<IStorageItem> itemFolderList = await storageFolder.GetItemsAsync();
            foreach (IStorageItem storageItem in itemFolderList)
            {
                _children.Add(new FileSystemViewModel
                {
                    StorageItem = storageItem
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
