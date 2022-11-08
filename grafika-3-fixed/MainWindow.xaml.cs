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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace grafika_3_fixed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte state = 0;
        private double rValue = 0, gValue = 0, bValue = 0;
        private double rP = 0, gP = 0, bP = 0;
        private double cValue = 0, mValue = 0, yValue = 0, kValue = 0;
        private double hValue = 0, sValue = 0, vValue = 0;

        

        private double Cmax = 0, Cmin = 0, delta = 0;
        private bool changeFlag = false;

        public MainWindow()
        {
            InitializeComponent();
            setText();
            changeColor();
        }

        private void RadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            switch(radio.Content.ToString())
            {
                case "RGB":
                    state = 0;
                    changeFlag = true;
                    break;
                case "CMYK":
                    state = 1;
                    changeFlag = true;
                    break;
                case "HSV":
                    state = 2;
                    changeFlag = true;
                    break;
            }
            if(state != 1)
            {

                kPanel.IsEnabled = false;
                kPanel.Visibility = Visibility.Hidden;
            }
            if(state == 0)
            {
                currentFormat.Content = "RGB";
                firstLetter.Content = "R";
                secondLetter.Content = "G";
                thirdLetter.Content = "B";
                firstSlider.Maximum = secondSlider.Maximum = thirdSlider.Maximum = 255;
                firstSlider.Value = rValue;
                secondSlider.Value = gValue;
                thirdSlider.Value = bValue;
            } else if(state == 1)
            {
                currentFormat.Content = "CMYK";
                firstLetter.Content = "C";
                secondLetter.Content = "M";
                thirdLetter.Content = "Y";
                kPanel.IsEnabled = true;
                kPanel.Visibility = Visibility.Visible;
                firstSlider.Maximum = secondSlider.Maximum = thirdSlider.Maximum = 100;
                firstSlider.Value = Math.Round(cValue * 100);
                secondSlider.Value = Math.Round(mValue * 100);
                thirdSlider.Value = Math.Round(yValue * 100);
                kSlider.Value = Math.Round(kValue * 100);
            } else if(state == 2)
            {
                currentFormat.Content = "HSV";
                firstLetter.Content = "H";
                secondLetter.Content = "S";
                thirdLetter.Content = "V";
                firstSlider.Maximum = 359;
                secondSlider.Maximum = thirdSlider.Maximum = 100;
                firstSlider.Value = hValue;
                secondSlider.Value = Math.Round(sValue * 100);
                thirdSlider.Value = Math.Round(vValue * 100);
            }
            changeFlag = false;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (changeFlag) return;
            Slider slider = (Slider)sender;

            if (state == 0)
            {
                switch (slider.Name[0])
                {
                    case 'f':
                        rValue = Convert.ToByte(slider.Value);
                        break;

                    case 's':
                        gValue = Convert.ToByte(slider.Value);
                        break;

                    case 't':
                        bValue = Convert.ToByte(slider.Value);
                        break;
                }

                rP = rValue / 255;
                gP = gValue / 255;
                bP = bValue / 255;

                kValue = 1 - new List<double>() { rP, gP, bP }.Max();

                if (kValue != 1)
                {
                    cValue = (1f - rP - kValue) / (1 - kValue);
                    mValue = (1f - gP - kValue) / (1 - kValue);
                    yValue = (1f - bP - kValue) / (1 - kValue);
                }

                Cmax = Math.Max(Math.Max(rP, gP), bP);
                Cmin = Math.Min(Math.Min(rP, gP), bP);
                delta = Cmax - Cmin;

                if (Cmax != 0)
                    sValue = delta / Cmax;
                else
                    sValue = 0;

                vValue = Cmax;

                if (delta == 0)
                    hValue = 0;
                else if (Cmax == rP)
                    hValue = Math.Round(((gP - bP) / delta % 6) * 60);
                else if (Cmax == gP)
                    hValue = Math.Round(((bP - rP) / delta + 2) * 60);
                else if (Cmax == bP)
                    hValue = Math.Round(((rP - gP) / delta + 4) * 60);

                if (hValue < 0)
                    hValue += 360;
            }
            else if (state == 1)
            {
                switch (slider.Name[0])
                {
                    case 'f':
                        cValue = Convert.ToByte(slider.Value) / 100.0;
                        break;

                    case 's':
                        mValue = Convert.ToByte(slider.Value) / 100.0;
                        break;

                    case 't':
                        yValue = Convert.ToByte(slider.Value) / 100.0;
                        break;

                    case 'k':
                        kValue = Convert.ToByte(slider.Value) / 100.0;
                        break;
                }

                rValue = Math.Round(255 * (1 - cValue * (1 - kValue)));
                gValue = Math.Round(255 * (1 - mValue * (1 - kValue)));
                bValue = Math.Round(255 * (1 - yValue * (1 - kValue)));

                rP = rValue / 255;
                gP = gValue / 255;
                bP = bValue / 255;

                Cmax = Math.Max(Math.Max(rP, gP), bP);
                Cmin = Math.Min(Math.Min(rP, gP), bP);
                delta = Cmax - Cmin;

                if (Cmax != 0)
                    sValue = delta / Cmax;
                else
                    sValue = 0;

                vValue = Cmax;

                if (delta == 0)
                    hValue = 0;
                else if (Cmax == rP)
                    hValue = Math.Round(((gP - bP) / delta % 6) * 60);
                else if (Cmax == gP)
                    hValue = Math.Round(((bP - rP) / delta + 2) * 60);
                else if (Cmax == bP)
                    hValue = Math.Round(((rP - gP) / delta + 4) * 60);

                if (hValue < 0)
                    hValue += 360;

            }
            else if (state == 2)
            {
                switch (slider.Name[0])
                {
                    case 'f':
                        hValue = Convert.ToInt16(slider.Value);
                        break;

                    case 's':
                        sValue = Convert.ToByte(slider.Value) / 100.0;
                        break;

                    case 't':
                        vValue = Convert.ToByte(slider.Value) / 100.0;
                        break;
                }

                double c = vValue * sValue;
                double hP = hValue / 60.0;
                double x = c * (1 - Math.Abs(hP % 2 - 1));
                double m = vValue - c;
                double rQ = 0, gQ = 0, bQ = 0;

                if (0 <= hP && hP < 1)
                {
                    rQ = c;
                    gQ = x;
                }
                else if (1 <= hP && hP < 2)
                {
                    rQ = x;
                    gQ = c;
                }
                else if (2 <= hP && hP < 3)
                {
                    gQ = c;
                    bQ = x;
                }
                else if (3 <= hP && hP < 4)
                {
                    gQ = x;
                    bQ = c;
                }
                else if (4 <= hP && hP < 5)
                {
                    rQ = x;
                    gQ = c;
                }
                else if (5 <= hP && hP < 6)
                {
                    rQ = c;
                    gQ = x;
                }

                rValue = Math.Round((rQ + m) * 255);
                gValue = Math.Round((gQ + m) * 255);
                bValue = Math.Round((bQ + m) * 255);

                rP = rValue / 255;
                gP = gValue / 255;
                bP = bValue / 255;

                kValue = 1 - new List<double>() { rP, gP, bP }.Max();

                if (kValue != 1)
                {
                    cValue = (1f - rP - kValue) / (1 - kValue);
                    mValue = (1f - gP - kValue) / (1 - kValue);
                    yValue = (1f - bP - kValue) / (1 - kValue);
                }

            }
            setText();
            changeColor();
        }



        private void setText()
        {
            rgbText.Text = "RGB\n" + "R: " + rValue + "\nG: " + gValue + "\nB: " + bValue;
            cmykText.Text = "CMYK\n" + "C: " + Math.Round(cValue * 100) + "%\nM: " + Math.Round(mValue * 100) + "%\nY: " + Math.Round(yValue * 100) + "%\nK: " + Math.Round(kValue * 100) + "%";
            hsvText.Text = "HSV\n" + "H: " + hValue + "°\nS: " + Math.Round(sValue * 100, 1) + "\nV: " + Math.Round(vValue * 100, 1);
        }

        private void changeColor()
        {
            colorPreview.Background = new SolidColorBrush(Color.FromRgb((byte)rValue, (byte)gValue, (byte)bValue));
            colorPreview.Foreground = new SolidColorBrush(Color.FromRgb((byte)(255 - rValue), (byte)(255 - gValue), (byte)(255 - bValue)));
            byte[] byteArray = new byte[3];
            byteArray[0] = (byte)rValue;
            byteArray[1] = (byte)gValue;
            byteArray[2] = (byte)bValue;
            colorPreview.Content = "#" + Convert.ToHexString(byteArray);
        }

        private void rotation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            switch (slider.Name[0])
            {
                case 'x':
                    Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), xslider.Value));
                    break;
                case 'y':
                    Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), yslider.Value));
                    break;
            }
        }
    }
}
