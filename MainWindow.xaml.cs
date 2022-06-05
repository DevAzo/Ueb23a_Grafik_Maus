using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ueb23a_Grafik_Maus
{
    /// <summary>
    /// Ueb23a Grafik Maus
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime dtStart = new DateTime();
        private DateTime dtStop = new DateTime();
        private List<double> reaktionsZeiten = new List<double>();
        private int nCounter = 0;
        private int nKreisDurchMesser = 10;
        private Point ptKreis = new Point();
        private int nWidthCanvas = 0;
        private int nHeightCanvas = 0;

        public MainWindow()
        {
            InitializeComponent();
            UpdateLayout();
        }

        private void myWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ClearValue(SizeToContentProperty);
            nWidthCanvas = (int)this.Width;
            nHeightCanvas = (int)this.Height;
            CreateKreisSizeAndPosition();
            CreateKreis();
            dtStart = DateTime.Now;
        }

        private void CreateKreisSizeAndPosition()
        {
            Random rnd = new Random();
            nKreisDurchMesser = rnd.Next(10, 100);
            Thread.Sleep(4);
            Random rndKreisX = new Random();
            ptKreis.X = rndKreisX.Next(10, nWidthCanvas - nKreisDurchMesser);
            Thread.Sleep(5);
            Random rndKreisY = new Random();
            ptKreis.Y = rndKreisY.Next(10, nHeightCanvas - nKreisDurchMesser);
        }

        private void CreateKreis()
        {
            Ellipse kreis = new Ellipse();
            kreis.Width = nKreisDurchMesser;
            kreis.Height = nKreisDurchMesser;

            SolidColorBrush yellowBrush = new SolidColorBrush();
            yellowBrush.Color = Colors.GreenYellow;
            kreis.Fill = yellowBrush;
            Canvas.SetLeft(kreis, ptKreis.X);
            Canvas.SetTop(kreis, ptKreis.Y);

            myCanvas.Children.Add(kreis);
        }

        private void PreViewMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (IsTargetHit(e))
            {
                nCounter++;
                lblTime.Content = "Getroffen " + nCounter;
                dtStop = DateTime.Now;
                var dtSpan = dtStop - dtStart;
                reaktionsZeiten.Add(dtSpan.TotalSeconds);
                dtStart = DateTime.Now;
            }
            else
            {
                lblTime.Content = "Daneben ";
            }

            if (nCounter >= 4)
            {
                var dAverageTime = reaktionsZeiten.Average();
                lblTime.Content = "Ø " + Math.Round(dAverageTime, 2) + " sek. ";
                reaktionsZeiten.Clear();
                nCounter = 0;
            }
        }

        private bool IsTargetHit(MouseButtonEventArgs e)
        {
            bool bGetroffen = false;

            // Schlaufe über den Logical Tree
            foreach (var item in myCanvas.Children)
            {
                var kreis = item as Ellipse;
                if (kreis != null)
                {
                    if (kreis.IsMouseOver)
                    {
                        Debug.WriteLine("GETROFFEN");
                        bGetroffen = true;
                    }
                    else
                    {
                        Debug.WriteLine("D A N E B E N");
                    }

                    CreateKreisSizeAndPosition();
                    kreis.Width = nKreisDurchMesser;
                    kreis.Height = nKreisDurchMesser;
                    Canvas.SetLeft(kreis, ptKreis.X);
                    Canvas.SetTop(kreis, ptKreis.Y);
                    break;
                }
            }

            return bGetroffen;
        }
    }
}