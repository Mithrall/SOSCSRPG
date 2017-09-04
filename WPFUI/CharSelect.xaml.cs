using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Engine.ViewModels;

namespace WPFUI {
    public partial class CharSelect:Window {
        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;
        CharSelectsSession _charSelectSession;


        public CharSelect() {
            InitializeComponent();

            _charSelectSession = new CharSelectsSession();
            DataContext = _charSelectSession;
        }

        public bool Connect(string ip) {
            try {
                client = new TcpClient(ip, 12345);
                stream = client.GetStream();
                sr = new StreamReader(stream);
                sw = new StreamWriter(stream);
                sw.AutoFlush = true;
                return true;
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        public bool isNew(string name) {
            sw.WriteLine("ISNEW¤" + name);

            if (sr.ReadLine() != "EXISTS") {
                return true;
            } else {
                return false;
            }
        }

        public void StartNew(string userName) {
            _charSelectSession.CurrentUser.UserName = userName;

            sw.WriteLine("NEWUSER¤" + userName);

            //TROR IK DER ER NØDVENDIGT ? 
            //string[] stats = sr.ReadLine().Split('¤');

            //Thread thread = new Thread(Loop);
            //thread.Start();
        }

        public void StartExisting(string userName) {
            _charSelectSession.CurrentUser.UserName = userName;

            sw.WriteLine("LOAD¤" + userName);
            string[] stats = sr.ReadLine().Split('¤');


            //Thread thread = new Thread(Loop);
            //thread.Start();
        }


        

        //UI STUFF
        private Grid _charSelected;

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {
            var temp = (Grid)sender;
            if (_charSelected != temp) {
                temp.Background = Brushes.Bisque;
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {
            var temp = (Grid)sender;
            if (_charSelected != temp) {
                temp.Background = Brushes.Beige;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (_charSelected != null) {
                _charSelected.Background = Brushes.Beige;
            }
            _charSelected = (Grid)sender;
            _charSelected.Background = Brushes.Red;
            string s = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + (string)_charSelected.Children.OfType<Label>().ToList()[3].Content + ".png";

            var imagebrush = new ImageBrush {ImageSource = new BitmapImage(new Uri(s))};
            Display.Background = imagebrush;
        }

        private void CreateNewChar_OnClick(object sender, RoutedEventArgs e)
        {
            CharCreation charCreation = new CharCreation();
            charCreation.Show();
        }

        private void DeleteSelectedChar_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
