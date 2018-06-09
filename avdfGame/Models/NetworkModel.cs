using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace avdfGame
{
    public class NetworkModel : INotifyPropertyChanged
    {
        //private Socket senderObject;
        //private byte[] bytes;
        private TcpClient client;

        public NetworkModel()
        {
        }

        public string StartNetwork(string ip, string port)
        {

            // Connect to a remote device.  
            try
            {
                // Create a TCP/IP  socket.  
                client = new TcpClient(ip, int.Parse(port));

                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                ns.BeginRead(buffer, 0, buffer.Length, GetMessage, buffer);

                return "Server Connected";

            }
            catch (Exception e)
            {
                return (e.ToString());
            }
        }

        public string SendMessage(string umsg)
        {
            try
            {
                NetworkStream networkStream = client.GetStream();
                byte[] myWriteBuffer = Encoding.ASCII.GetBytes(umsg);
                networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

                return String.Format("Message sent: {0}", umsg);
            }
            catch(Exception e)
            {
                return (e.ToString());
            }
        }

        // Declare the event
        private string messageContent;
        public string MessageContent
        {
            get { return messageContent; }
            set
            {
                if(messageContent != value)
                {
                    messageContent = value;
                    OnPropertyChanged("MessageContent");
                }
            }
        }
        // Create the OnPropertyChanged method to raise the event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            //PropertyChangedEventHandler handler = PropertyChanged;
            //if (handler != null)
            //{
            //    handler(this, new PropertyChangedEventArgs(name));
            //}
        }

        private void GetMessage(IAsyncResult result)
        {
            try
            {
                NetworkStream networkStream = client.GetStream();
                byte[] buffer = result.AsyncState as byte[];
                string data = ASCIIEncoding.ASCII.GetString(buffer, 0, buffer.Length).TrimEnd(new char[] { '\0' });

                //owner.HandleNetworkMessage(data);
                MessageContent = data;

                networkStream.BeginRead(buffer, 0, buffer.Length, GetMessage, buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //owner.WriteToLog(e.ToString(), true);
            }
        }

        public string CloseNetworkConnection(bool h, string n)
        {
            //if (h)
            //{
            //    SendMessage(String.Format("QUIT|H|{0}", n));

            //}
            //else
            //{
            //    SendMessage(String.Format("QUIT|P|{0}", n));
            //}
            try
            {
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "Network Connection Closed";
        }


    }
}
