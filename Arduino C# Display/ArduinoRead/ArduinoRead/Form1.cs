using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoRead
{
    public partial class Form1 : Form
    {
        string bufferedData = "";
        string StartChar = ">";
        string endChar = ";";
        bool warning;
        public Form1()
        {
            InitializeComponent();
            Arduino.BaudRate = 19200;
            Arduino.PortName = "COM4";
            timer.Enabled = true;
        }
        public void Append(String data)
        {
            if (data != null)
            {
                bufferedData += data;
            }
        }
        public String NextMessage()
        {
            int beginIndex = bufferedData.IndexOf(StartChar);
            if (beginIndex != -1)
            {
                int endIndex = bufferedData.IndexOf(endChar, beginIndex);
                if (endIndex != -1)
                {
                    String foundMessage = bufferedData.Substring(beginIndex, (endIndex - beginIndex) + 1);
                    bufferedData = bufferedData.Substring(endIndex + 1);
                    checkmessages(foundMessage);
                    foundMessage = NextMessage();
                    return foundMessage;
                }
            }
            return null;
        }

        private void checkmessages(string message)
        {
            if (message.StartsWith(">Dens:"))
            {
                string meta = message.Substring(6);
                meta = meta.Substring(0, meta.Length - 2);
                MessageBox.Show("Density: " + meta);
            }
            if (message.StartsWith(">TEMP:"))
            {
                string meta = message.Substring(6);
                meta = meta.Substring(0, meta.Length - 2);
                MessageBox.Show("Temperature: " + meta);
            }
        }

        private void btnSonarOn_Click(object sender, EventArgs e)
        {
            Arduino.Write(">12:On;");
        }

        private void btnSonarOff_Click(object sender, EventArgs e)
        {
            Arduino.Write(">12:Off;");
        }

        private void btnGetDensity_Click(object sender, EventArgs e)
        {
            Arduino.Write(">12:Read;");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Arduino.IsOpen)
            {
                if (Arduino.BytesToRead > 0)
                {
                    string message = Arduino.ReadExisting();
                    Append(message);
                    NextMessage();
                }
            }

        }

        private void btnSendSpeed_Click(object sender, EventArgs e)
        {
            Arduino.Write(">15:" + nudSpeed.Value.ToString() + ";");
        }

        private void btnWarningOn_Click(object sender, EventArgs e)
        {
            warning = !warning;
            if (warning)
            {
                Arduino.Write(">15:" + nudSpeed.Value.ToString() + ";");

                Arduino.Write(">15:Warning;");
            }
            else
            {
                Arduino.Write(">15:NoWarning;");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Arduino.Write(">14:;");
        }

        private void btnDisplayOff_Click(object sender, EventArgs e)
        {
            Arduino.Write(">15:Off;");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Arduino.Open();
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Arduino.Close();
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
        }
    }
}
