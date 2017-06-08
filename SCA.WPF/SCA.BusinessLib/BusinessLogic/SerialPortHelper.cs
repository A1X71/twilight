using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Microsoft.Win32;
namespace SCA.BusinessLib
{
    public class SerialPortHelper
    {

        /// <summary>
        /// Serial Port public handler
        /// </summary>
        static SerialPort devicePort = new SerialPort();

        /// <summary>
        /// Flag that tells if the device il connected
        /// </summary>
        public static bool DeviceConnected;

        /// <summary>
        /// COM port searching main method.
        /// </summary>
        public static void SearchCOM()
        {
            var QueryCmd = new byte[] { /* Command buffer content to query the device */ };
            var Reply = new byte[] { 0, 0, 0, 0 };
            var BytesRead = 0;

            if (DeviceConnected)
            {
                devicePort.Close();
                DeviceConnected = false;
            }

            while (!DeviceConnected)
            {
                var SerailPortsNames = ComPortNames("0483", "5740");
                if (SerailPortsNames.Count > 0)
                {
                    foreach (string s in SerialPort.GetPortNames())
                    {
                        if (SerailPortsNames.Contains(s))
                        {
                            devicePort.PortName = s;
                            devicePort.BaudRate = 115200;
                            devicePort.Parity = Parity.None;
                            devicePort.DataBits = 8;
                            devicePort.StopBits = StopBits.One;
                            devicePort.RtsEnable = false;
                            devicePort.DtrEnable = false;
                            devicePort.ReadTimeout = 10000;
                            devicePort.WriteTimeout = 1000;
                            devicePort.ReadBufferSize = 60000;
                            devicePort.Handshake = Handshake.None;
                            try
                            {
                                devicePort.Open();
                                if (devicePort.IsOpen)
                                {
                                    devicePort.Write(QueryCmd, 0, QueryCmd.Length);
                                    BytesRead = Read(Reply, 4);
                                    if (BytesRead != 4)
                                    {
                                        devicePort.Close();
                                    }
                                    else
                                    {
                                        DeviceConnected = true;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("{0} Exception caught.", e);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(250);
            }

        }

        /// <summary>
        /// Public method to use for reading from COM port. It'type an override of the already existing method from SerialPort class.
        /// </summary>
        /// <param name="buffer"> Buffer to use for storing data </param>
        /// <param name="lenght"> Number of bytes to read </param>
        /// <returns></returns>
        public static int Read(byte[] buffer, int lenght)
        {
            var i = 0;
            while (i < lenght)
            {
                i += devicePort.BaseStream.Read(buffer, i, (lenght - i));
            }
            return i;
        }

        /// <summary>
        /// Public method to use for writing to COM port. It'type an override of the already existing method from SerialPort class.
        /// </summary>
        /// <param name="buffer"> Buffer to write </param>
        /// <param name="lenght"> Number of bytes to write </param>
        public static void Write(byte[] buffer, int lenght)
        {
            devicePort.BaseStream.Write(buffer, 0, lenght);
        }

        /// <summary>
        /// Public method to close the COM port.
        /// </summary>
        public static void Close()
        {
            devicePort.Close();
        }

        /// <summary>
        /// Static method that returns the list of connected COM ports with given VID and PID.
        /// </summary>
        /// <param name="vid"> Vendor ID </param>
        /// <param name="pid"> Product ID </param>
        /// <returns> List of connected COM ports with given VID and PID </returns>
        static List<string> ComPortNames(string vid, string pid)
        {
            int ID;
            var pattern = string.Format("^VID_{0}.PID_{1}", vid, pid);
            var _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var comports = new List<string>();
            var rk1 = Registry.LocalMachine;
            var rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (string s3 in rk2.GetSubKeyNames())
            {
                var rk3 = rk2.OpenSubKey(s3);
                foreach (string s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        var rk4 = rk3.OpenSubKey(s);
                        foreach (string s2 in rk4.GetSubKeyNames())
                        {
                            if (int.TryParse(s2, out ID))
                            {
                                var rk5 = rk4.OpenSubKey(s2);
                                var rk6 = rk5.OpenSubKey("Device Parameters");
                                if (IsDeviceConnected((string)rk6.GetValue("PortName")))
                                {
                                    comports.Add((string)rk6.GetValue("PortName"));
                                }
                            }
                        }
                    }
                }
            }
            return comports;
        }

        /// <summary>
        /// This method checks whether the device is connected or not.
        /// </summary>
        /// <param name="comName"> Name of the port </param>
        /// <returns> true if device is connected, fals if not. </returns>
        static bool IsDeviceConnected(string comName)
        {
            string GetComName;
            var Rk1 = Registry.LocalMachine;
            var Rk2 = Rk1.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM");
            foreach (string S1 in Rk2.GetValueNames())
            {
                GetComName = (string)Rk2.GetValue(S1);
                if (comName.Equals(GetComName))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
