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
    public class GsmConnect
    {
        private readonly SerialPort _port = null;
        private bool IsDeviceFound { get; set; } = false;
        public bool IsConnected { get; set; } = false;



        public GsmConnect()
        {
            _port = new SerialPort();
        }

        //Return list of GSM modems (connectection)
        public GsmCom[] List()
        {
            List<GsmCom> gsmCom = new List<GsmCom>();
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
                    GsmCom com = new GsmCom();
                    com.Name = portName;
                    com.Description = portDescription;
                    gsmCom.Add(com);
                }
            }

            return gsmCom.ToArray();
        }

        public GsmCom Search()
        {
            IEnumerator enumerator = List().GetEnumerator();
            GsmCom com = enumerator.MoveNext() ? (GsmCom)enumerator.Current : null;

            if (com == null)
            {
                IsDeviceFound = false;
                Console.WriteLine("No GSM device found!");
            }
            else
            {
                IsDeviceFound = true;
                Console.WriteLine(com.ToString());
            }

            return com;
        }

        public bool Connect()
        {
            if (_port == null || !IsConnected || !_port.IsOpen)
            {
                IsConnected = false;

                GsmCom com = Search();
                if (com != null)
                {
                    try
                    {
                        _port.PortName = com.Name;
                        _port.BaudRate = 9600; // еще варианты 4800, 9600, 28800 или 56000
                        _port.DataBits = 8; // еще варианты 8, 9
                        _port.StopBits = StopBits.One; // еще варианты StopBits.Two StopBits.None или StopBits.OnePointFive         
                        _port.Parity = Parity.Odd; // еще варианты Parity.Even Parity.Mark Parity.None или Parity.Space
                        _port.ReadTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)
                        _port.WriteTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)
                        _port.NewLine = Environment.NewLine;
                        _port.Handshake = Handshake.RequestToSend;
                        _port.DtrEnable = true;
                        _port.RtsEnable = true;
                        _port.Encoding = Encoding.GetEncoding("windows-1251");

                        _port.Open();
                        _port.DataReceived += SerialPortDataReceived;
                        
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
            if (_port != null || IsConnected || _port.IsOpen)
            {
                _port.Close();
                _port.Dispose();
                IsConnected = false;
            }
        }

        public void ReadFirst()
        {
            Console.WriteLine("Reading first...");

            _port.WriteLine("AT+CMGF=1"); //Set mode to Text(1) or PDU(0)
            Thread.Sleep(500); //Give a second or write
            _port.WriteLine("AT+CPMS=\"SM\""); //Set storage to SIM(SM)
            Thread.Sleep(500);
            _port.WriteLine("AT+CMGL=\"ALL\""); //What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(500);

            string responce = _port.ReadExisting();

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

            _port.WriteLine("AT+CMGL=\"ALL\""); //What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(500);

            string responce = _port.ReadExisting();

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

            _port.WriteLine("AT+CMGF=1");
            Thread.Sleep(500);
            _port.WriteLine($"AT+CMGS=\"{toAdress}\"");
            Thread.Sleep(500);
            _port.WriteLine(message + char.ConvertFromUtf32(26));
            Thread.Sleep(500);

            string responce = _port.ReadExisting();

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

            _port.WriteLine("AT^USSDMODE=0");
            Thread.Sleep(500);
            _port.WriteLine("AT+CUSD=1,\"*110*10#\",15");
            Thread.Sleep(500);

            string responce = _port.ReadExisting();

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

            _port.WriteLine("AT^U2DIAG=0");
            Thread.Sleep(500);
            _port.WriteLine("AT^CARDLOCK?");
            Thread.Sleep(500);

            string responce = _port.ReadExisting();

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

            _port.WriteLine("AT+COPS?");
            Thread.Sleep(500);

            
        }
    }
}
