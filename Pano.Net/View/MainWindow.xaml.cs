using Microsoft.Win32;
using Pano.Net.Commands;
using Pano.Net.Model;
using Pano.Net.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pano.Net.View
{
    /// <summary>
    ///hello
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Dictionary<int, string> dictImg = new Dictionary<int, string>();
        int count = 0;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
           


            // If the window style is set to none when the window is maximized, the taskbar will not
            // be covered. Therefore, the window is restored to normal and maximized again.
            DependencyPropertyDescriptor d = DependencyPropertyDescriptor.FromProperty(
                Window.WindowStyleProperty, typeof(Window));
            d.AddValueChanged(this, (sender, args) =>
            {
                Window w = (Window)sender;
                //Testing pushing
                if (w.WindowStyle == System.Windows.WindowStyle.None)
                {
                    w.WindowState = System.Windows.WindowState.Normal;
                    w.WindowState = System.Windows.WindowState.Maximized;
                }
            });

            //Irfan

            //image load code

            //Irfan

        }






        //Irfan
        string Droppedimg;


        //Image drag handling method
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = e.Source as Image;
            DataObject data = new DataObject(DataFormats.Text, img.Source);
            Droppedimg=img.Source.ToString();
           
            DragDrop.DoDragDrop((DependencyObject)e.Source, data, DragDropEffects.Move);
            //Minimizing memory usage.
            MinimizeFootprint();
        }


        //Deleting Image
        private void Delete_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            String deletepath = @"/Resources/noimage.png";
            Uri imageUrid = new Uri(deletepath, UriKind.Relative);
            BitmapImage imageBitmapd = new BitmapImage(imageUrid);
            var imgd = (Image)sender;
            imgd.Source = imageBitmapd;

            //Getting the sender name and deleting the values from dictionaries
            String sendernamed = ((Image)sender).Name;
            String hd = sendernamed.Remove(0, 1);
            String ElemtImg = null;
            dict.TryGetValue(hd, out ElemtImg);
            String DelDictImg = hd + ":" + ElemtImg;
            dict.Remove(hd);
            //MessageBox.Show(DelDictImg);
            try
            {
                var itemm = dictImg.First(kvp => kvp.Value == DelDictImg);

                dictImg.Remove(itemm.Key);
            }
            catch
            {
                MessageBox.Show("There is no image in the selected cell");
            }
            hd = null;
            //Minimizing memory usage.
            MinimizeFootprint();

        }

        //deleting ends


        //Image drop handling method
        private void DropHandler(object sender, DragEventArgs e)
        {

        //Getting the sender name
            String sendername = ((Image)sender).Name;
           string DropfileName = Path.GetFileName(Droppedimg);
            //MessageBox.Show(DropfileName);         
            String h = sendername.Remove(0, 1);

            //Getting the image name

            //int c1 = (Droppedimg.Length)-5;
            String Imagename = Droppedimg;
            String g= Path.GetFileName(Droppedimg);
            //String g = Imagename.Remove(0, c1);
           // MessageBox.Show("c1...." + g);

            //Stroring them in dictionary

            dict.Add(h, g);

            //Adding to elements to dictImg
           
            String cbdV = h + ":" + g;
            dictImg.Add(count, cbdV);
            count++;



//setting the dropped image into the grid
            String stringPath = Droppedimg;
            Uri imageUri = new Uri(stringPath);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            var img = (Image)sender;
            img.Source = imageBitmap;
            Droppedimg = null;
        }


        //codec
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        //Process Button Handler

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Total Number of Images on the grid
            int TotalImages = dict.Count;
            string l = TotalImages.ToString();
            String dctl = dictImg.Count.ToString();
            //MessageBox.Show(l);
            //MessageBox.Show("Combined dict vales:"+dctl);
            List<string> Tour_Lines = new List<string>();
             
            Tour_Lines.Add(l); //adding total number of images to file


            var pathWithEnv = @"%USERPROFILE%";
            var fp = Environment.ExpandEnvironmentVariables(pathWithEnv);

            //MessageBox.Show(fp);

            String filepath1 = fp + "\\Desktop\\Tour";
            bool exists = System.IO.Directory.Exists(filepath1);

            if (!exists)
                System.IO.Directory.CreateDirectory(filepath1);

           System.IO.File.Delete(@filepath1 + "\\TourNet_store.txt");
           
            String filepath = @filepath1+ "\\TourNet_store.txt";
            //string filePath = Environment.ExpandEnvironmentVariables("C:\\Users\\%USERPROFILE%\\Desktop\\Fall2016\\Capstone\\TourNet_store.txt");


            foreach (var pair in dictImg)
            {
                var key = pair.Key;
                var value = pair.Value;
                
                string[] words = value.Split(':');
                // String rowcol = value.Remove(2,6).ToString();
                
                String rowcol = words[0];
                //MessageBox.Show(rowcol);
                // String rowcolImgName = value.Remove(0, 3).ToString();
                String rowcolImgName = words[1];
                 //MessageBox.Show(rowcolImgName);
                String row = rowcol.Remove(1, 1);
                int rowi = Convert.ToInt32(row);
                String col = rowcol.Remove(0, 1);
                int coli= Convert.ToInt32(col);

                //main
                Tour_Lines.Add("main");

                Tour_Lines.Add(rowcolImgName);

                String nor = (rowi - 1).ToString();
                String north = nor + col;
                String northImage=null;
                dict.TryGetValue(north, out northImage);
                if(northImage!=null)
                {
                    Tour_Lines.Add("north");
                    Tour_Lines.Add(northImage);
                }
               

                String sou= (rowi + 1).ToString();
                String south = sou + col;
                String southImage=null;
                dict.TryGetValue(south, out southImage);
                if (southImage != null)
                {
                    Tour_Lines.Add("south");
                    Tour_Lines.Add(southImage);
                }

                String Eas = (coli + 1).ToString();
                String East = row + Eas;
                String EastImage=null;
                dict.TryGetValue(East, out EastImage);
                if (EastImage != null)
                {
                    Tour_Lines.Add("east");
                    Tour_Lines.Add(EastImage);
                }


                String Wes = (coli - 1).ToString();
                String West = row + Wes;
                String WestImage=null;
                dict.TryGetValue(West, out WestImage);
                if (WestImage != null)
                {
                    Tour_Lines.Add("west");
                    Tour_Lines.Add(WestImage);
                }
                Tour_Lines.Add("end");

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    //MessageBox.Show(Tour_Lines.Count.ToString());
                    for (int i = 0; i < Tour_Lines.Count; i++)
                    {
                        //MessageBox.Show(Tour_Lines[i]);
                        sw.WriteLine(Tour_Lines[i]);
                    }
                }
                Tour_Lines.Clear();


                //adding images on the grid to the Tour folder. if the image files are already present
                //it will delete them and copy the new one's to the folder

                string filll = @selfodpath +"/"+rowcolImgName;
               // MessageBox.Show(filll);
                using (System.Drawing.Bitmap bmp1 = new System.Drawing.Bitmap(filll))
                {
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                    System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;



                    var pathWithEnv1 = @"%USERPROFILE%";
                    var fp1 = Environment.ExpandEnvironmentVariables(pathWithEnv1);

                    string fileName = Path.GetFileNameWithoutExtension(filll);
                    string fileExt = Path.GetExtension(filll);
                    String filepath11 = fp1 + "\\Desktop\\Tour";
                    string cttt = Path.Combine(filepath11, fileName + fileExt);

                    System.IO.File.Delete(@cttt);

                    bmp1.Save(cttt, jpgEncoder, myEncoderParameters);

                }



            }
            MessageBox.Show("Tour file is created!!");


        }

  


        //Irfan

        public void sett(List<ImageDetails> stt)
        {
           
            ImageList.ItemsSource = stt;
        }

   //minimizing memory usage.
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        static void MinimizeFootprint()
        {
            EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle);
        }


        //sending images to the front end to display thumnails.
        String selfodpath;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            //System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            //imp
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string foldpath = dialog.SelectedPath;
                //MessageBox.Show(foldpath);

                //string root = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string root = foldpath;
                selfodpath = foldpath;
                //MessageBox.Show("selpath:"+selfodpath);
                string[] supportedExtensions = new[] { ".bmp", ".jpeg", ".jpg", ".png", ".tiff" };
                // var files = Directory.GetFiles(Path.Combine(root, "Images"), "*.*").Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));
                var files = Directory.GetFiles(root, "*.*").Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));

                //List<ImageDetails> images = new List<ImageDetails>();
                //changed list to observable collection to avoid memeory leaks
               System.Collections.ObjectModel.ObservableCollection<ImageDetails> images = new System.Collections.ObjectModel.ObservableCollection<ImageDetails>();


                //string cttt;
                foreach (var file in files)
                {

                    //MessageBox.Show(file);
                    ImageDetails id = new ImageDetails()
                    {
                        Path = file,
                        FileName = Path.GetFileName(file),
                        Extension = Path.GetExtension(file)
                    };

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.UriSource = new Uri(file, UriKind.Absolute);
                    img.EndInit();
                    //id.Width = img.PixelWidth;
                    //id.Height = img.PixelHeight;
                    //hardcoding height and width to improve performance.
                    id.Width = 8000;
                    id.Height = 4000;
                    // I couldn't find file size in BitmapImage
                    FileInfo fi = new FileInfo(file);
                    id.Size = fi.Length;
                    images.Add(id);
                }

                ImageList.ItemsSource = images;
                //MinimizeFootprint();
               

            }



        }

       
    }


}
