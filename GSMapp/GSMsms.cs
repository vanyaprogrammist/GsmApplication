using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSMapp
{
    public class GSMsms
    {
        private SerialPort gsmPort = null;
        private bool IsDeviceFound { get; set; } = false;
        public bool IsConnected { get; set; } = false;

        public GSMsms()
        {
            gsmPort = new SerialPort();
        }

        public GSMcom[] List()
        {
            List<GSMcom> gsmCom = new List<GSMcom>();
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.EnablePrivileges = true;
            string connectString = $@"\\{Environment.MachineName}\root\cimv2";
            ManagementScope scope = new ManagementScope(connectString,options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_POTSModem");
            ManagementObjectSearcher search = new ManagementObjectSearcher(scope,query);
            ManagementObjectCollection collection = search.Get();

            foreach (ManagementObject obj in collection)
            {
                string portName = obj["AttachedTo"].ToString();
                string portDescription = obj["Description"].ToString();

                if (portName != "")
                {
                    GSMcom com = new GSMcom();
                    com.Name = portName;
                    com.Description = portDescription;
                    gsmCom.Add(com);
                }
            }

            return gsmCom.ToArray();
        }

        public GSMcom Search()
        {
            

            IEnumerator enumerator = List().GetEnumerator();
            GSMcom com = enumerator.MoveNext() ? (GSMcom)enumerator.Current : null;

            if (com == null)
            {
                IsDeviceFound = false;
                Console.WriteLine("No GSM device found!");
            }
            else
            {
                IsDeviceFound = true;
                Console.WriteLine(com.ToString());
                //Connect();
            }

            return com;
        }

        public bool Connect()
        {
            if (gsmPort == null || !IsConnected || !gsmPort.IsOpen)
            {
                IsConnected = false;

                GSMcom com = Search();
                if (com != null)
                {
                    try
                    {
                        gsmPort.PortName = com.Name;
                        gsmPort.BaudRate = 9600; // еще варианты 4800, 9600, 28800 или 56000
                        gsmPort.DataBits = 8; // еще варианты 8, 9
                        gsmPort.StopBits = StopBits.One; // еще варианты StopBits.Two StopBits.None или StopBits.OnePointFive         
                        gsmPort.Parity = Parity.Odd; // еще варианты Parity.Even Parity.Mark Parity.None или Parity.Space
                        gsmPort.ReadTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)
                        gsmPort.WriteTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)
                        gsmPort.NewLine = Environment.NewLine;
                        gsmPort.Handshake = Handshake.RequestToSend;
                        gsmPort.DtrEnable = true;
                        gsmPort.RtsEnable = true;
                        gsmPort.Encoding = Encoding.GetEncoding("windows-1251");

                        gsmPort.Open();
                        gsmPort.DataReceived += SerialPortDataReceived;
                        IsConnected = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        IsConnected = false;
                    }
                }
                else
                {
                    IsConnected = false;
                }

            }
            return IsConnected;
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(indata);
        }

        public void Disconnect()
        {
            if (gsmPort != null || IsConnected || gsmPort.IsOpen)
            {
                gsmPort.Close();
                gsmPort.Dispose();
                IsConnected = false;
            }
        }

        public void ReadFirst()
        {
            Console.WriteLine("Reading first...");

            gsmPort.WriteLine("AT+CMGF=1"); //Set mode to Text(1) or PDU(0)
            Thread.Sleep(500); //Give a second or write
            gsmPort.WriteLine("AT+CPMS=\"SM\""); //Set storage to SIM(SM)
            Thread.Sleep(500);
            gsmPort.WriteLine("AT+CMGL=\"ALL\""); //What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(500);

            string responce = gsmPort.ReadExisting();

            if (responce.EndsWith("\r\nOK\r\n"))
            {
                Console.WriteLine(responce);
            }
            else
            { 
                Console.WriteLine("!!Error text: "+responce); 
            }
        }

        public void Read()
        {
            Console.WriteLine("Reading...");

            gsmPort.WriteLine("AT+CMGL=\"ALL\""); //What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(500);

            string responce = gsmPort.ReadExisting();

            if (responce.EndsWith("\r\nOK\r\n"))
            {
                Console.WriteLine(responce);
            }
            else
            {
                Console.WriteLine("!!Error text: " + responce);
            }
        }

        public void Send(string toAdress, string message)
        {
            Console.WriteLine("Sending...");

            gsmPort.WriteLine("AT+CMGF=1");
            Thread.Sleep(500);
            gsmPort.WriteLine($"AT+CMGS=\"{toAdress}\"");
            Thread.Sleep(500);
            gsmPort.WriteLine(message + char.ConvertFromUtf32(26));
            Thread.Sleep(500);

            string responce = gsmPort.ReadExisting();

            if (responce.EndsWith("\r\nOK\r\n") && responce.Contains("+CMGS"))
            {
                Console.WriteLine(responce);
            }
            else
            {
                Console.WriteLine("!!Error text: " + responce);
            }
        }

        public void Number()
        {
            Console.WriteLine("Number->");

            gsmPort.WriteLine("AT^USSDMODE=0");
            Thread.Sleep(500);
            gsmPort.WriteLine("AT+CUSD=1,\"*201#\",15");
            Thread.Sleep(500);

            string responce = gsmPort.ReadExisting();

            if (responce.EndsWith("\r\nOK\r\n") && responce.Contains("+CMGS"))
            {
                Console.WriteLine(responce);
            }
            else
            {
                Console.WriteLine("!!Error text: " + responce);
            }
        }

        public void Unlock()
        {
            Console.WriteLine("Unlock->");

            gsmPort.WriteLine("AT^U2DIAG=0");
            Thread.Sleep(500);
            gsmPort.WriteLine("AT^CARDLOCK?");
            Thread.Sleep(500);

            string responce = gsmPort.ReadExisting();

            if (responce.EndsWith("\r\nOK\r\n"))
            {
                Console.WriteLine(responce);
            }
            else
            {
                Console.WriteLine("!!Error text: " + responce);
            }
        }

        public void Operator()
        {
            Console.WriteLine("Operator->");

            gsmPort.WriteLine("AT+COPS?");
            Thread.Sleep(500);

            
        }
    }
}
