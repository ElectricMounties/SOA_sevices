using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;

namespace testingsoap
{
    public partial class Form1 : Form
    {
        public struct ServiceStruct
        {
            public string Name;
            public string IP;
            public string[] Methods;
            public string[] MethodNames;
        }
        ArrayList configInfo = new ArrayList();

        public Form1()
        {

            InitializeComponent();
            label1.Text = "Ready...     ";
            parseConfig();
            populateDrops();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Connecting...     ";
            TcpClient myclient = new TcpClient();//create a client
            ServiceStruct serv = (ServiceStruct)configInfo[ServiceCmb.SelectedIndex];
            byte[] mydata = new byte[1000];//i assumed the xml data will not be larger than 1000 bytes
            myclient.Connect(serv.IP, 80);//the ip of the server for the webservice it is for me is http://www.webserviceX.NET/
            if (myclient.Connected == true)//check to be sure that the connection is done
            {
                label1.Text = "Connected!!";
            }


            //mydata = new byte[1000];
            mydata = System.Text.Encoding.UTF8.GetBytes(serv.Methods[0]);

            FileStream data = new FileStream("response.txt", FileMode.Create);//create another file to can see the response
            Stream clientstream = myclient.GetStream();
            clientstream.Write(mydata, 0, mydata.Length);//send my xml data
            label1.Text = "Data Sent....";
            Thread.Sleep(4000);//wait for some time
            StreamReader sr = new StreamReader(clientstream);

            textBox1.Text = sr.ReadToEnd();//read the response

             data.Write(Encoding.ASCII.GetBytes(textBox1.Text), 0, Encoding.ASCII.GetByteCount(textBox1.Text));
             data.Close();
            //ReadXmlFile();
            
        }

        public void ReadXmlFile()
        {
            //string path = "response.txt"; // Finds the location of App_Data on server.
            //XmlTextReader reader = new XmlTextReader(path); //Combines the location of App_Data and the file name

            //DataTable workTable = new DataTable("Customers");

            //while (reader.Read())
            //{
            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element:
            //            break;
            //        case XmlNodeType.Text:
            //            workTable.Columns.Add(reader.Value);
            //            break;
            //        case XmlNodeType.EndElement:
            //            break;
            //    }
            //}
            //resultDGV.DataSource = workTable;
        }

        public void parseConfig()
        {
            StreamReader data = new StreamReader("config.ini");//open the config file
            string temp = "";

            ServiceStruct tempStruct = new ServiceStruct { Methods = new string[10], MethodNames = new string[10] };
            temp = data.ReadLine();
            do
            {
                int i = 0;
                
                if (temp.Contains("--NAME"))
                {
                    tempStruct.Name = data.ReadLine();
                    if (data.ReadLine().Contains("IP"))
                    {
                        tempStruct.IP = data.ReadLine();
                        temp = data.ReadLine();
                        while (data.EndOfStream != true && temp.Contains("--METHOD"))
                        {
                            tempStruct.MethodNames[i] += data.ReadLine();
                            temp = data.ReadLine();

                            while (!data.EndOfStream && !temp.Contains("--END"))
                            {
                                tempStruct.Methods[i] += temp + Environment.NewLine;
                                temp = data.ReadLine();
                            }
                            i++;
                            temp = data.ReadLine();
                        }
                        configInfo.Add(tempStruct);
                      //  temp = data.ReadLine();
                    }
                }
            } while (data.EndOfStream != true) ;
        }

        public void populateDrops()
        {
            ServiceCmb.Items.Clear();
            foreach (ServiceStruct s in configInfo)
            {
                ServiceCmb.Items.Add(s.Name);
            }
        }
    }
}
