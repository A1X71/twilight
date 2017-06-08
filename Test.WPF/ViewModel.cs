using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Runtime.Hosting;
using Microsoft.Expression.Interactivity;
namespace Test.WPF
{
    
    public class ViewModel:INotifyPropertyChanged
    {
        
        private const string rootDirPath=@"c:\william";
        public IList<FileExplorerItem> Items { get; set; }

        private FileExplorerItem _selectedItem;
        public ViewModel()
        {
            System.IO.DirectoryInfo dirInfo =
                new System.IO.DirectoryInfo(rootDirPath);
            Directory rootDirectory=CreateDirectory(dirInfo); 
            this.Items=new List<FileExplorerItem>(){rootDirectory};
        }
        private Directory CreateDirectory(System.IO.DirectoryInfo dirInfo)
        {
            Directory directory = new Directory()
            {
                Name = dirInfo.Name,
                Path = dirInfo.FullName,
                SubItems = new List<FileExplorerItem>(),
            };

            //Add each file in the current directory
            foreach (System.IO.FileInfo fi in dirInfo.GetFiles())
            {
                File file = new File()
                {
                    Name = fi.Name,
                    Path = dirInfo.FullName
                };
                directory.SubItems.Add(file);
            }
            directory.PropertyChanged += item_PropertyChanged;
            // Add each subdirectory using recursion 
            foreach (System.IO.DirectoryInfo subDirInfo in dirInfo.GetDirectories())
            {
                directory.SubItems.Add(CreateDirectory(subDirInfo));
            }
            //Add each file in the current directory
            foreach (System.IO.FileInfo fi in dirInfo.GetFiles())
            {
                File file = new File()
                {
                    Name = fi.Name,
                    Path = dirInfo.FullName
                };
                file.PropertyChanged += item_PropertyChanged;
                directory.SubItems.Add(file);
            }
            return directory;
        }
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FileExplorerItem item = sender as FileExplorerItem;
            if (item != null && item.IsSelected)
                NotifyPropertyChanged("SelectedItem");
        }
        public FileExplorerItem SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        //private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            private void NotifyPropertyChanged( string propertyName = "")
        {
          //  System.Runtime.CompilerServices.
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private static FileExplorerItem GetSelectedItem(IEnumerable<FileExplorerItem> items)
        { 
          //top-level items:
            FileExplorerItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
               //sub-level items;
                IEnumerable<FileExplorerItem> subItems = items.OfType<Directory>().SelectMany(d => d.SubItems);
                if (items.Any())
                    item = GetSelectedItem(subItems);
            }
            return item;
        }
    }

    public class FileExplorerItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
      //  protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        protected void NotifyPropertyChanged( string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Directory : FileExplorerItem
    {
        public IList<FileExplorerItem> SubItems { get; set; }
    }

    public class File : FileExplorerItem
    {
        public string Content
        {
            get
            {
                //read file content (IsAsync=True) ...
                string path = string.Format("{0}\\{1}", this.Path, this.Name);
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);

                return string.Empty;
            }
        }
    }
}

