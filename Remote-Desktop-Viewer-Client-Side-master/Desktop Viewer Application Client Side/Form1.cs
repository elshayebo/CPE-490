using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;

namespace Desktop_Viewer_Application_Client_Side
{
    public partial class Form1 : Form
    {

        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainStream;
        private int portNumber;

        private static Image GrabDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(screenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        private void sendDesktopImage()
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            mainStream = client.GetStream();
            binFormatter.Serialize(mainStream, GrabDesktop());
        }



        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            portNumber = int.Parse(txtPort.Text);
           
            
             
            try
            {
                client.Connect(txtIP.Text, portNumber);
                MessageBox.Show("Connected to Server!!");
            }

            catch (Exception)
            {
                MessageBox.Show("Failed connection, check for correct Port and IP addresses");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (btnSend.Text.StartsWith("Share My Screen"))
            {
                timer1.Start();
                btnSend.Text = "Stop Sharing";
            }

            else
            {
                timer1.Stop();
                btnSend.Text = "Share My Screen";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sendDesktopImage();
        }
    }
}
