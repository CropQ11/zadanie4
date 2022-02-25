using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using System.Drawing.Imaging;
using System.ComponentModel;


namespace WpfApp5
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap;

        BitmapImage bitmapa;
        int x = 90;
        int a = -1;
        int be = 1;
        Bitmap bmp;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            canvas.Source = bitmapa;


        }
        public Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmapa = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmapa);
            }
        }


        public static BitmapSource Invert(BitmapSource source)
        {

            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];


            source.CopyPixels(data, stride, 0);


            for (int i = 0; i < length; i += 4)
            {
                data[i] = (byte)(255 - data[i]); //R
                data[i + 1] = (byte)(255 - data[i + 1]); //G
                data[i + 2] = (byte)(255 - data[i + 2]); //B
                data[i + 3] = (byte)(255 - data[i + 3]); //A
            }


            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                bitmapa = new BitmapImage(new Uri(ofdPicture.FileName));
                bitmap = BitmapImage2Bitmap(bitmapa);
                canvas.Source = bitmapa;
            }
        }

        private void negat_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource beta = new BitmapImage();
            beta = Invert(bitmapa);
            bitmapa = beta as BitmapImage;
            canvas.Source = beta;

        }
        private void b_Click(object sender, RoutedEventArgs e)
        {

            RotateTransform rotateTransform = new RotateTransform(x);
            x = x + 90;

            rotateTransform.CenterX = 200;
            rotateTransform.CenterY = 100;
            canvas.RenderTransform = rotateTransform;



        }
        private void a_Click(object sender, RoutedEventArgs e)
        {
            ///lustrzane odbicie
            /// TransformedBitmap transformBmp = new TransformedBitmap();
            /// transformBmp.BeginInit();
            /// transformBmp.Source = bitmap;
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = a };
            int z = a;
            a = be;
            be = z;
            scaleTransform.CenterX = 200;
            scaleTransform.CenterY = 100;
            canvas.RenderTransform = scaleTransform;


        }
        private BitmapImage Bitmap2BitmapImage()
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }



        private void zielony_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                bitmap = BitmapImage2Bitmap(bitmapa);
                System.Drawing.Color kolor;

                for (int o = 0; o < bitmap.Width; o++)
                {
                    for (int j = 0; j < bitmapa.Height; j++)
                    {
                        kolor = bitmap.GetPixel(o, j);
                        if (!(kolor.R < 100 && kolor.G > 100 && kolor.B < 100))
                        {
                            kolor = System.Drawing.Color.FromArgb(255, 255, 255);
                            bitmap.SetPixel(o, j, kolor);

                        }
                    }
                }


                bitmapa = Bitmap2BitmapImage();
                canvas.Source = bitmapa;




            }

        }
       
    }
}