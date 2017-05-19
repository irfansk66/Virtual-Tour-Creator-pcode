using Microsoft.Win32;
using Pano.Net.Commands;
using Pano.Net.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;





namespace Pano.Net.ViewModel
{
    /// <summary>
    /// Main ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Commands
        #region commands

        /// <summary>
        /// Open image with dialog
        /// </summary>
        public ICommand OpenCommand { get; private set; }
        //public ICommand OpenFolderCommand { get; private set; }
        
        /// <summary>
        /// Open image by file name
        /// </summary>
        public ICommand OpenWithFilenameCommand { get; private set; }

        /// <summary>
        /// Exit application
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Toggle fullscreen
        /// </summary>
        public ICommand FullscreenCommand { get; private set; }

        /// <summary>
        /// Display controls
        /// </summary>
        public ICommand ControlsCommand { get; private set; }

        /// <summary>
        /// Display about information
        /// </summary>
        public ICommand AboutCommand { get; private set; }
        #endregion

        // Public properties
        #region public_properties

        /// <summary>
        /// Panorama
        /// </summary>
        public BitmapImage Image { get; private set; }

        /// <summary>
        /// Is fullscreen mode on
        /// </summary>
        public bool IsFullscreen { get; private set; }

        /// <summary>
        /// Is the model loading
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Recent images manager
        /// </summary>
        public RecentImageManager RecentImageManager { get; private set; }
        #endregion

        public String fodpath { get;  set; }

        public Tour VirtualTour
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            OpenCommand = new RelayCommand(a => Open());
            OpenWithFilenameCommand = new RelayCommand(a => Open((string)a));
            ExitCommand = new RelayCommand(a => Exit());
            FullscreenCommand = new RelayCommand(a => FullScreen());
            ControlsCommand = new RelayCommand(a => Controls());
            AboutCommand = new RelayCommand(a => About());
            //OpenFolderCommand = new RelayCommand(a => OpenFolder()); 

            RecentImageManager = new Model.RecentImageManager(); RaisePropertyChanged("RecentImages");
            
            Image = null; RaisePropertyChanged("Image");
            IsFullscreen = false; RaisePropertyChanged("IsFullscreen");
            IsLoading = false; RaisePropertyChanged("IsLoading");

            VirtualTour = null;

        }

        //Irfan

        public MainViewModel(String p)
        {

            String folderpath = p;
            System.Windows.MessageBox.Show("open img with path is" + folderpath);

        
            OpenCommand = new RelayCommand(a => Open1(folderpath));
            OpenWithFilenameCommand = new RelayCommand(a => Open((string)folderpath));
            ExitCommand = new RelayCommand(a => Exit());
            FullscreenCommand = new RelayCommand(a => FullScreen());
            ControlsCommand = new RelayCommand(a => Controls());
            AboutCommand = new RelayCommand(a => About());

            RecentImageManager = new Model.RecentImageManager(); RaisePropertyChanged("RecentImages");

            Image = null; RaisePropertyChanged("Image");
            IsFullscreen = false; RaisePropertyChanged("IsFullscreen");
            IsLoading = false; RaisePropertyChanged("IsLoading");





        }

        // Private methods
        #region private_methods

        // Open image with dialog
        private async void Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (ofd.ShowDialog() == true)
            {
                await Open(ofd.FileName);
            }
        }

        //Irfan


        private async void Open1(String j)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Images (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            String fpp = j;
            if (fpp!=null)
            {
                await Open(fpp);
            }
        }


        // Open image by file name
        public async Task Open(string path)
        {
            
            
            
            Image = null; RaisePropertyChanged("Image");
            IsLoading = true; RaisePropertyChanged("IsLoading");

            //try
            //{
            //    VirtualTour = new Tour(System.IO.Path.GetDirectoryName(path), path);
            //}
            //catch
            //{
            //    VirtualTour = null;

            //}

                await Task.Factory.StartNew(() =>
            {
                Image = new BitmapImage();
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.UriSource = new Uri(path);
                Image.EndInit();
                Image.Freeze();
            });

            if (Math.Abs(Image.Width / Image.Height - 2) > 0.001)
                WarningMessage("Warning", "The opened image is not equirectangular (2:1)! Rendering may be improper.");

            RecentImageManager.AddAndSave(path);

            IsLoading = false; RaisePropertyChanged("IsLoading");
            RaisePropertyChanged("Image");
        }

        // Exit application
        private void Exit()
        {
            App.Current.Shutdown();
        }

        // Toggle fullscreen
        private void FullScreen()
        {
            IsFullscreen = !IsFullscreen;
            RaisePropertyChanged("IsFullscreen");
        }

        //Display controls
        private void Controls()
        {
            InfoMessage("Controls", "Click and drag the mouse to move camera.\r\nScroll to zoom.");
        }

        // Display about information
        private void About()
        {
            InfoMessage("About", "Virtual Tour Creator Version 1.0, 2016.");
        }

        // Helper function to display an information
        private void InfoMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Helper function to display a warning
        private void WarningMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        //open folder dialoge box

        //private List<ImageDetails> imagesddd;

        //public void OpenFolder()
        //{
        //    var dialog = new System.Windows.Forms.FolderBrowserDialog();

        //    //System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        string foldpath = dialog.SelectedPath;
        //        MessageBox.Show(foldpath);

        //        //string root = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //        string root = foldpath;
        //        string[] supportedExtensions = new[] { ".bmp", ".jpeg", ".jpg", ".png", ".tiff" };
        //        // var files = Directory.GetFiles(Path.Combine(root, "Images"), "*.*").Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));
        //        var files = Directory.GetFiles(root, "*.*").Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));

        //        List<ImageDetails> images = new List<ImageDetails>();

        //        foreach (var file in files)
        //        {
        //            ImageDetails id = new ImageDetails()
        //            {
        //                Path = file,
        //                FileName = Path.GetFileName(file),
        //                Extension = Path.GetExtension(file)
        //            };

        //            BitmapImage img = new BitmapImage();
        //            img.BeginInit();
        //            img.CacheOption = BitmapCacheOption.OnLoad;
        //            img.UriSource = new Uri(file, UriKind.Absolute);
        //            img.EndInit();
        //            id.Width = img.PixelWidth;
        //            id.Height = img.PixelHeight;

        //            // I couldn't find file size in BitmapImage
        //            FileInfo fi = new FileInfo(file);
        //            id.Size = fi.Length;
        //            images.Add(id);
        //        }

        //        //ImageList.ItemsSource = images;
        //        imagesddd = images;

        //    }


        //}
        ////load folder ends


        #endregion
    }
}
