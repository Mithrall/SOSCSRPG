using System.Windows;

namespace WPFUI {
    /// <summary>
    /// Interaction logic for CharCreation.xaml
    /// </summary>
    public partial class CharCreation:Window {
        public CharCreation() {
            InitializeComponent();
        }

        private void CreateChar_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            
        }
    }
}
