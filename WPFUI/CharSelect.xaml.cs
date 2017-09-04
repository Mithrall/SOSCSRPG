using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFUI {
    public partial class CharSelect:Window {
        Grid charselected;
        public CharSelect() {
            InitializeComponent();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {
            var temp = (Grid)sender;
            if (charselected != temp) {
                temp.Background = Brushes.Bisque;
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {
            var temp = (Grid)sender;
            if (charselected != temp) {
                temp.Background = Brushes.Beige;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (charselected != null) {
                charselected.Background = Brushes.Beige;
            }
            charselected = (Grid)sender;
            charselected.Background = Brushes.Red;
            string s = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + (string)charselected.Children.OfType<Label>().ToList()[3].Content + ".png";

            var imagebrush = new ImageBrush();
            imagebrush.ImageSource = new BitmapImage(new Uri(s));
            Display.Background = imagebrush;
        }
    }
}
