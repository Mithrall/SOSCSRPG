using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WPFUI {

    public partial class MainWindow:Window {

        public MainWindow() {
            InitializeComponent();
            CharSelect charSelect = new CharSelect();
            charSelect.Show();
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
            string UserName = UserNameTextBox.Text;
            string type = TypeComboBox.Text;

            Regex regex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            MatchCollection matches = regex.Matches(ip);
            if (matches.Count > 0) {
                foreach (Match match in matches) {
                    ip = match.Value;
                    Game g = new Game();
                    if (g.Connect(ip)) {
                        g.Show();
                        if (g.isNew(UserName)) {
                            g.StartNew(UserName, type);
                        } else {
                            g.StartExisting(UserName, type);
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
