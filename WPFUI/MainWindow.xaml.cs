using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WPFUI {

    public partial class MainWindow:Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e) {
            Login();
        }

        private void Login_EnterKey(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Login();
            }
        }

        private void Login() {
            string ip = IPTextBox.Text;
            string userName = UserNameTextBox.Text;

            Regex regex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            MatchCollection matches = regex.Matches(ip);
            if (matches.Count > 0) {
                foreach (Match match in matches) {
                    ip = match.Value;
                    CharSelect charSelect = new CharSelect();
                    if (charSelect.Connect(ip)) {
                        charSelect.Show();
                        if (charSelect.IsNew(userName)) {
                            charSelect.StartNew(userName);
                        } else {
                            charSelect.StartExisting(userName);
                        }
                        this.Hide();
                    }
                }
            } else {
                MessageBox.Show("Wrong IP");
            }
        }
    }
}
