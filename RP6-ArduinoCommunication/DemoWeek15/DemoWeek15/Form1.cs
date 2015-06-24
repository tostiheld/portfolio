using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;


namespace DemoWeek15
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private int btnCount = 0;
        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            serialPort.BaudRate = 38400;
            btnMaxSpeed.Enabled = false;
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
                    readMessageTimer.Enabled = true;

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


    }
}
