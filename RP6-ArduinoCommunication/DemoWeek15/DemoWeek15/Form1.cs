using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;


namespace DemoWeek15
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private int btnCount = 0;
        String warning = "";
        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            serialPort.BaudRate = 38400;
            btnMaxSpeed.Enabled = false;
            CreateServer();
        }

        void CreateServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://145.93.58.182:8085/");
            listener.Start();

            new Thread(
                () =>
                {
                    while (true)
                    {
                        HttpListenerContext ctx = listener.GetContext();
                        ThreadPool.QueueUserWorkItem((_) => ProcessRequest(ctx));
                    }

                }
            ).Start();
        }

        void ProcessRequest(HttpListenerContext ctx)
        {
            try
            {
                string responseText = Execute(ctx);
                byte[] buf;

                if (responseText == "index.html")
                {

                    string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    string path = dir + @"\web\test.html";

                    FileInfo fileInfo = new FileInfo(path);
                    long numBytes = fileInfo.Length;

                    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    buf = binaryReader.ReadBytes((int)numBytes);
                    binaryReader.Close();
                    fileStream.Close();

                    ctx.Response.ContentEncoding = Encoding.UTF8;
                    ctx.Response.ContentType = "text/html";
                    ctx.Response.ContentLength64 = numBytes;
                }
                else
                {

                    buf = Encoding.UTF8.GetBytes(responseText);

                    Debug.WriteLine(buf);

                    ctx.Response.ContentEncoding = Encoding.UTF8;
                    ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                    ctx.Response.ContentType = "application/json";
                    ctx.Response.ContentLength64 = buf.Length;
                }

                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            }
            catch
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            ctx.Response.Close();
        }

        /// <summary>
        /// Excecutes the client call and returns the data
        /// </summary>
        /// <param name="ctx">ctx contains the query</param>
        /// <returns>data</returns>
        string Execute(HttpListenerContext ctx)
        {
            System.Collections.Specialized.NameValueCollection nv = HttpUtility.ParseQueryString(ctx.Request.Url.Query);
            // Iterate through the collection.
            if (nv["fetch"] != null)
            {
                string fetch = nv["fetch"];
                if (fetch == "speed")
                {
                    String tmpWarning = warning;                 
                    warning = "";
                    /*if (txtMaxSpeed.Text == "")
                    {
                        txtMaxSpeed.Text = "0";
                    }
                    if (LblSpeed.Text == "")
                    {
                        LblSpeed.Text = "0";
                    }*/

                    Debug.WriteLine("maxspeed" + Convert.ToInt32(txtMaxSpeed.Text));
                    Debug.WriteLine("speed" + Convert.ToInt32(LblSpeed.Text));

                    return "{\"speed\": " + LblSpeed.Text + ",\"speedAllowed\": " + txtMaxSpeed.Text + ",\"warning\":\"" + tmpWarning + "\"}";
                }

            }

            // Write the result to a label.
            return "index.html";

        }


        private void btnMaxSpeed_Click(object sender, EventArgs e)
        {
            byte getal;

            if (byte.TryParse(txtMaxSpeed.Text, out getal))
            {
                if (SendMessage(getal))
                {
                    Debug.WriteLine("Could not write message!");
                }
            }
      
           
        }

        private bool SendMessage(byte number)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    byte[] buffer = new byte[1];
                    buffer[0] = number;

                    serialPort.Write(buffer, 0, 1);
                }
                catch (ArgumentException ex) 
                {
                    Debug.WriteLine("Could not write to serial port: " + ex.Message);
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine("Could not write to serial port: " + ex.Message);
                    return false;
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine("Could not write to serial port: " + ex.Message);

                }

                return true;
            }
            return false;
        }


        private void UpdateUserInterface()
        {
            bool isConnected = serialPort.IsOpen;
            if (isConnected)
            {
                connectButton.Text = "Disconnect";
                btnMaxSpeed.Enabled = true;

            }
            else
            {
                connectButton.Text = "Connect";


            }
            refreshSerialPortsButton.Enabled = !isConnected;
            serialPortSelectionBox.Enabled = !isConnected;
            refreshSerialPortsButton.Visible = !isConnected;
            serialPortSelectionBox.Visible = !isConnected;


        }

        private void FillSerialPortSelectionBoxWithAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);

            serialPortSelectionBox.Items.Clear();
            foreach (String port in ports)
            {
                serialPortSelectionBox.Items.Add(port);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                readMessageTimer.Enabled = false;
                serialPort.Close();

            }
            else
            {
                String port = serialPortSelectionBox.Text;
                try
                {
                    serialPort.PortName = port;
                    serialPort.Open();
                    if (serialPort.IsOpen && btnCount < 1)
                    {
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();

                        btnCount++;

                    }
                    btnCount--;                   

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Could not connect to the given serial port: " + exception.Message);
                }
            }

            UpdateUserInterface();
        }

        private void refreshSerialPortsButton_Click_1(object sender, EventArgs e)
        {
            FillSerialPortSelectionBoxWithAvailablePorts();
        }

        private void serialPortSelectionBox_Leave(object sender, EventArgs e)
        {
            serialPortSelectionBox.Text = serialPortSelectionBox.Text.ToUpper();
        }

        private void BtnSpeed_Click(object sender, EventArgs e)
        {
            readMessageTimer.Enabled = true;
            
        }

        private void readMessageTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen
                && serialPort.BytesToRead > 0)
            {
                    String dataFromSocket = serialPort.ReadExisting();
                    
                    if (dataFromSocket != "")
                     {
                        MessageReceived(dataFromSocket);
                     }
              }               
         }


        private void MessageReceived(String message)
        {
            LblSpeed.Text = message;
        }

    }
}
