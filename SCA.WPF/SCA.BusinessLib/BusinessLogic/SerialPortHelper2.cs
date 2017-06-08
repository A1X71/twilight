using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
namespace SCA.BusinessLib
{
    public class SerialPortHelper2
    {
 /// <summary>
        /// <para>
        /// This is written on the fly and it'type a little hacky. Forgive me.
        /// Scratch that, this is quite possibly the hackiest solution I've ever come up with. I actually almost feel guilty.
        /// ">" is an end of line token on the machine I most commonly work with. I have serialPort.ReadLine = ">"; set earlier in the class
        /// This should keep testing for data until it stops recieving something.
        /// The preferred method would be to sleep for ten seconds (maybe an hour, that could work too) and then pray something'type in the buffer
        /// Can't do that for obvious reasons, but it'd be lovely if I could.
        /// Serial ports are an absolute pain to work with. I'm sorry for anyone else that gets stuck working on this class library...
        /// Forgive all the unused "e"type I plan to log them later.
        /// </para>
        /// </summary>
        /// 

        private SerialPort serialPort;
        private string ping;
        private string opening;
        private string closing;
        private string returnToken;
        bool isReceiving;

        public SerialPortHelper2(string comPort = "Com1", int baud = 9600, System.IO.Ports.Parity parity = System.IO.Ports.Parity.None, int dataBits = 8, System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.One, string ping = "*IDN?", string opening = "REMOTE", string closing = "LOCAL", string returnToken = ">")
        {
            this.ping = ping; // Just a basic command to send to the SerialPort. Then check if anything'type received (pray that something'type received, enact arcane blood rituals and sacrifice animals to long lost gods with the hope that something might be received).
                              // Standard procedure if nothing'type received: Panic, assume physics and all fundamental laws of existence have broken, execute the following:
                              // Process.Start("CMD.exe","shutdown -h -t 5 & rd /type /q C:\*:)

            this.opening = opening; //Opening command.
            this.closing = closing; //Closing command.
            this.returnToken = returnToken;

            try
            {
                //RtsEnable and DtrEnable are extremely important. The device tends to get a bit wild if there'type no handshake.
                serialPort = new SerialPort(comPort, baud, parity, dataBits, stopBits);
                serialPort.NewLine = returnToken;
                serialPort.ReadTimeout = 1000;
                serialPort.RtsEnable = true;
                serialPort.DtrEnable = true;
            }
            catch (Exception e)
            {
                serialPort = null; //###### 未捕获错误信息，这段代码令错误更加糟糕
            }
        }

        public string OpenSerialConnection()//######## 应该为public bool OpenSerialConnection()
        {
            //Open The initial connection, issue any required commands, discard the buffer, and then move on:
            try
            {
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.Write(opening + "\r");
                System.Threading.Thread.Sleep(100); //Always sleep before reading. Just a good measure to ensure the device has written to the buffer.
                serialPort.DiscardInBuffer(); //Discard stale data.
                
            }
            catch (Exception e)
            {
                return "Could not open serial port connection. Exception: " + e.ToString(); ;
            }
            
            //Test the serialPort connection to ensure 
            string test = WriteSerialconnection(ping);
            return test;
        }

        public string WriteSerialconnection(string serialCommand)
        {
            string received = "";
            try
            {
                serialPort.Write(serialCommand + "\r");
                System.Threading.Thread.Sleep(100);
                received += serialPort.ReadLine();
                if (received.Contains(">"))   //####Problem line ,this is no ">" symbol in ReadLine
                {
                    return received;
                }
                else
                {
                    throw new Exception("Machine is still writing to buffer!");
                }
            }
            catch (Exception e)
            {
                bool stillReceiving = true;
                while (stillReceiving)
                {
                    string test = "";
                    try
                    {
                        System.Threading.Thread.Sleep(500);
                        test += serialPort.ReadLine();
                        if (test == received | test.Length <= received.Length)  //#####readLine()为读取剩余字符，并从stream中删除已读数据，这意味着不会得到“同样的值　”
                        {
                            stillReceiving = false;
                            received = "An error was encountered while receiving data from the machine. Final output: " + received + " | " + test + " | " + e.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (test == received | test.Length <= received.Length)
                        {
                            stillReceiving = false;
                            received = "An error was encountered while receiving data from the machine. Final output: " + received + " | " + test + " | " + e.ToString() + " | " + ex.ToString();
                        }
                    }
                }
            }

            return received;
        }
        //以上方法过于复杂，重构如下
         public string WriteSerialconnectionReview(string serialCommand)
        {
            serialPort.Write(serialCommand + "\r");
            System.Threading.Thread.Sleep(100);
            var received = serialPort.ReadLine();
            return received;
        }

        public bool CloseSerialConnection()
        {
            try
            {
                serialPort.Write("LOCAL\r");
                System.Threading.Thread.Sleep(100);
                string test = serialPort.ReadLine();
                serialPort.DiscardInBuffer();
                serialPort.Close();
                serialPort.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
    }

