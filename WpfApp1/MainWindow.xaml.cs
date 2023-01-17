using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var ipAddress = IPAddress.Parse("10.2.11.19");
            var port = 27001;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipAddress, port);
                socket.Bind(ep);
                socket.Listen(10);
                Console.WriteLine($"Listen over {socket.LocalEndPoint}");

                while (true)
                {
                    var client = socket.Accept();
                    Task.Run(() =>
                    {
                        Console.WriteLine($"{client.RemoteEndPoint} connected . . .");

                        var length = 0;
                        var bytes = new byte[1024];
                        do
                        {
                            length = client.Receive(bytes);
                            var msg = Encoding.UTF8.GetString(bytes, 0, length);
                            Console.WriteLine($"CLIENT : {client.RemoteEndPoint} : {msg}");
                            if (msg == "exit")
                            {
                                client.Shutdown(SocketShutdown.Both);
                                client.Dispose();
                                break;
                            }
                        } while (true);
                    });

                }
            }
        }
    }
}
