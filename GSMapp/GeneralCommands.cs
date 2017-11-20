using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Entities;
using GSMapp.Hellpers;

namespace GSMapp
{
    public class GeneralCommands
    {
        public string OperatorName { get; set; } = null;

        public SerialDataReceivedEventHandler Receiver;

        public GeneralCommands()
        {
            
        }

        public string Operator(string sender)
        {
            string a = sender;
            int startIndex = a.IndexOf("\"", StringComparison.Ordinal)+4;
            int lastIndex = a.LastIndexOf("\"", StringComparison.Ordinal) - startIndex;
            if (sender.Contains("+COPS:"))
            {
                a = a.Substring(startIndex, lastIndex);
                OperatorCheck(a);
                return a;
            }
            return null;
        }

        private void OperatorCheck(string operatorNumber)
        {
            int number = Int32.Parse(operatorNumber);

            foreach (OperatorList o in Enum.GetValues(typeof(OperatorList)))
            {
                if (number == (int)o)
                {
                    Console.WriteLine("Operator is: "+o);
                }
            }

            
        }

        private string MessageOfNumber(string message)
        {
            int startIndex = message.IndexOf("\"", StringComparison.Ordinal)+1;
            int lastIndex = message.LastIndexOf("\"", StringComparison.Ordinal) - startIndex;
            if (message.Contains("+CUSD:"))
            {
                message = message.Substring(startIndex, lastIndex);
                string result = message.Ucs2StrToUnicodeStr();
                return result;
            }
            return null;
        }

        public void ReceiverTest()
        {
            
                Receiver = (sender, args) =>
                {
                    Console.WriteLine("GeneralCommands");
                    SerialPort sp = (SerialPort)sender;
                    string indata = sp.ReadExisting();
                    string message = MessageOfNumber(indata);
                    if (message != null)
                    {
                        Console.WriteLine("This is my message");
                        Console.WriteLine(message);
                    }
                    

                    Console.WriteLine("Data Received->");
                    Console.Write(indata);
                    Console.WriteLine("End of data received<-");
                };
            
           
        }

        
        //Handler
        public void GeneralHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("GeneralCommands");
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            string op = Operator(indata);

            Console.WriteLine("Data Received->");
            Console.Write(indata);
            Console.WriteLine("End of data received<-");
            
        }
    }

   
}
