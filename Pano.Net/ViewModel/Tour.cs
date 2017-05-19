using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Pano.Net.Commands;
using Pano.Net.Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Pano.Net.ViewModel
{
    public class Tour
    {
        //private List<ImageNode> picNet;
        //private int currentImageIndex = -1;
        private int numPictures;
        // const string storageFile = "\\TourNet_store.txt";

        public string folderPath
        {
            get;
            private set;
        }
        public Tour() { }


        public Tour(string folderPath, string currentImagePath)
        {
            this.folderPath = folderPath;

            //numPictures = int.Parse(inFile.ReadLine());
            MessageBox.Show("Inside Tour"+folderPath);



        }

       
    }
}
