using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace testingsoap
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            label1.Text = "Ready...     ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "COnnecting...     ";
            TcpClient myclient = new TcpClient();//create a client
            byte[] mydata = new byte[1000];//i assumed the xml data will not be larger than 1000 bytes
            myclient.Connect("173.201.44.188", 80);//the ip of the server for the webservice it is for me is http://www.webserviceX.NET/
            if (myclient.Connected == true)//check to be sure that the connection is done
            {
                label1.Text = "Connected!!";
            }
            FileStream data = new FileStream("data.txt", FileMode.Open);//open the file that has the xml data
            int i;
            for ( i = 0; i < data.Length; i++)
            {
                mydata[i] = (byte)data.ReadByte();//fill my array with the xml string
            }
            data.Close();//close the file
            data = new FileStream("response.txt", FileMode.Create);//create another file to can see the response
            Stream clientstream = myclient.GetStream();
            clientstream.Write(mydata, 0, 1000);//send my xml data
            label1.Text = "Data Sent....";
            Thread.Sleep(4000);//wait for some time
            StreamReader sr = new StreamReader(clientstream);
             textBox1.Text=sr.ReadToEnd();//read the response
             data.Write(Encoding.ASCII.GetBytes(textBox1.Text), 0, Encoding.ASCII.GetByteCount(textBox1.Text));
             data.Close();
            
        }
    }
}
