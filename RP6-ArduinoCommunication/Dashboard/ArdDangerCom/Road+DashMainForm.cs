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

namespace Domotica
{

    public partial class RoadDashMainForm : Form
    {

        String warning = "";

        public RoadDashMainForm()
        {
            InitializeComponent();
         
            //Create and start the http server
            CreateServer();
        }



        /// <summary>
        /// Close the serial connection when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DomoticaMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
           
        }

        /// <summary>
        /// Copied from Stackoverflow
        /// Creates a HTTPListener
        /// </summary>
        void CreateServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://10.211.55.3:8085/");
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

        /// <summary>
        /// Processes the http request  and returns to client
        /// </summary>
        /// <param name="ctx"></param>
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
                else { 

                    buf = Encoding.UTF8.GetBytes(responseText);

                    Debug.WriteLine(buf);

                    ctx.Response.ContentEncoding = Encoding.UTF8;
                    //ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
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
            if (nv["fetch"] != null) {
                string fetch = nv["fetch"];
                if (fetch == "speed")
                {
                    String tmpWarning = warning;
                    String roadDirection = "Een richting";
                    warning = "";
                    if (tbSpeed.Text == "")
                    {
                        tbSpeed.Text = "0";
                    }
                    if (tbAllowedSpeed.Text == "")
                    {
                        tbAllowedSpeed.Text = "0";
                    }

                    return "{\"speed\": "+tbSpeed.Text+",\"speedAllowed\": "+tbAllowedSpeed.Text+",\"warning\":\""+tmpWarning+"\"}";
                }
                
            }

            // Write the result to a label.
            return "index.html";
            
        }

        private void btnNewWarning_Click(object sender, EventArgs e)
        {
            warning = tbNewWarning.Text;
            tbNewWarning.Text = "";
        }

    }
}
