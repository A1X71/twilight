using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.IO.Ports;
/* ==============================
*
* Author     : William
* Create Date: 2016/12/7 11:21:36
* FileName   : SerialComManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public delegate void ReceivedDataHandler(byte[] dataBuffer);
    public class SerialComManager:IDisposable
    {

        private string _portName;
        private int _baudRate;
        private SerialPort _serialPort;
        private byte[] _buffer;
        public event ReceivedDataHandler ReceivedData;
        public byte[] Buffer
        {
            get
            {
                return _buffer;
            }
        }
        public string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                _portName = value;
            }
        }

        public int BaudRate
        {
            get
            {
                return _baudRate;
            }
            set
            {
                _baudRate = value;
            }
        }
        public SerialComManager()
        {
            _portName = "COM3";
            _baudRate = 38400;
            if (_serialPort == null)
            {
                _serialPort = new SerialPort();
            }
            _serialPort.DataReceived += Data_Received;
        }

        public void OpenPort()
        {
            OpenPort(_portName, _baudRate);
        }
        public string OpenPort(string portName, int baudRate)
        {
            try
            {
                ClosePort();
                _serialPort.PortName = portName;
                _serialPort.BaudRate = baudRate;
                _serialPort.Open();
                string strReturnValue = _portName + "端口打开成功";
                return strReturnValue;
            }
            catch (Exception ex)
            {
                return "打开失败.原因：" + ex.Message;
            }
        }
        /// <summary>
        /// 返回此计算机的可用Com口
        /// </summary>
        /// <returns></returns>
        public string[] GetPortsNameOfComputer()
        {
            return SerialPort.GetPortNames();
        }
        public void ClosePort()
        {
            if (_serialPort.IsOpen == true)
                _serialPort.Close();
        }

        public void Data_Received(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //retrieve number of bytes in the buffer
                int bytes = _serialPort.BytesToRead;
                //create a byte array to hold the awaiting data
                byte[] comBuffer = new byte[bytes];
                //read the data and store it
                _serialPort.Read(comBuffer, 0, bytes);
                _buffer = comBuffer;
                ReceivedData(_buffer);
            }
            catch (Exception ex)
            {
                ClosePort();
            }
            //display the data to the user
            // return comBuffer;
        }
        /// <summary>
        /// 返回当前实例 
        /// </summary>
        /// <returns></returns>
        public bool GetPortStatus()
        {
            return _serialPort.IsOpen;
        }

        public void WriteData(byte[] msg)
        {
            if (!(_serialPort.IsOpen == true))
            {
                // DisplayData(MessageType.Error, "Open Port before sending data!\n");

            }
            else
            { 

                try
                {
                    //convert the message to byte array
                    byte[] newMsg = msg;
                    //send the message to the port
                    _serialPort.Write(newMsg, 0, newMsg.Length);
                    //   ''SendEndOfLine();
                    //convert back to hex and display
                    // ''  DisplayData(MessageType.Outgoing, ByteToHex(newMsg) + "\n");
                }
                catch (FormatException ex)
                {
                    //display error message
                    //'' DisplayData(MessageType.Error, ex.Message + "\n");
                    ClosePort();
                }
                finally
                {
                    //   _displayWindow.SelectAll();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_serialPort != null)
            {
                if(disposing)
                { 
                    _serialPort.Close();
                    _serialPort.Dispose();
                }
                _serialPort = null;

            }
        }
    }
    //internal static class SerialComManager
    //{
    //    public static void Send(byte[] bytes)
    //    {
    //        Console.WriteLine(ByteToHex(bytes));

    //    }
    //    private static string ByteToHex(byte[] comByte)
    //    {
    //        //create a new StringBuilder object
    //        StringBuilder builder = new StringBuilder(comByte.Length * 3);
    //        //loop through each byte in the array
    //        foreach (var data in comByte)
    //            //convert the byte to a string and add to the stringbuilder
    //            builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
    //        //return the converted value
    //        return builder.ToString().ToUpper();
    //    }
    //}
}
