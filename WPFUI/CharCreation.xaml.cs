using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFUI {
    public partial class CharCreation:Window {
        private CharSelect charSelect;
        private RadioButton _charClassSelected;

        public CharCreation() {
            InitializeComponent();
        }

        public void Setup(CharSelect c) {
            charSelect = c;
        }

        private void CreateChar_OnClick(object sender, RoutedEventArgs e) {
            if (CharName.Text == "") {
                MessageBox.Show("Choose a Name first");
            } else {
                charSelect.Show();
                charSelect.CreateNewChar((string)_charClassSelected.Content, CharName.Text);
                Close();
            }
        }

        private void CharClass_OnChecked(object sender, RoutedEventArgs e) {
            _charClassSelected = (RadioButton)sender;
            var s = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + _charClassSelected.Content + ".png";

            var imagebrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(s)) };
            Display.Background = imagebrush;
        }
    }
}
