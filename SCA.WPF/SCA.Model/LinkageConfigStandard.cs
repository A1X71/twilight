using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/18 16:48:26
* FileName   : LinkageConfigStandard
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class LinkageConfigStandard
    {
        private ControllerModel _controller; //Added at 2016-12-06 控制器信息
        public int ID { get; set; }
        /// <summary>
        /// 控制器ID；外键
        /// </summary>
        public int ControllerID { get; set; }
        public ControllerModel Controller
        {
            get 
            {
                return _controller;
            }
            set 
            {
                _controller = value;
            }
        }
        public string Code { get; set; }
        public int ActionCoefficient { get; set; }

        public string DeviceNo1 { get; set; }
        public string DeviceNo2 { get; set; }
        public string DeviceNo3 { get; set; }
        public string DeviceNo4 { get; set; }
        public string DeviceNo5 { get; set; }
        public string DeviceNo6 { get; set; }
        public string DeviceNo7 { get; set; }
        public string DeviceNo8 { get; set; }
        public string DeviceNo9 { get; set; }
        public string DeviceNo10 { get; set; }
        public string LinkageNo1 { get; set; }
        public string LinkageNo2 { get; set; }
        public string LinkageNo3 { get; set; }

        /// <summary>
        /// 取得所有的器件信息
        /// </summary>
        public List<string> GetDeviceNoList {
            get
            {
                
                List<string> lstDeviceNo = new List<string>();
                switch (_controller.Type)
                { 
                    case ControllerType.NT8001:
                            lstDeviceNo.Add(DeviceNo1);
                            lstDeviceNo.Add(DeviceNo2);
                            lstDeviceNo.Add(DeviceNo3);
                            lstDeviceNo.Add(DeviceNo4);
                            lstDeviceNo.Add(DeviceNo5);
                            lstDeviceNo.Add(DeviceNo6);
                            lstDeviceNo.Add(DeviceNo7);
                            lstDeviceNo.Add(DeviceNo8);
                            break;
                    case ControllerType.NT8036:
                            lstDeviceNo.Add(DeviceNo1);
                            lstDeviceNo.Add(DeviceNo2);
                            lstDeviceNo.Add(DeviceNo3);
                            lstDeviceNo.Add(DeviceNo4);
                            break;
                    case ControllerType.NT8007:
                            lstDeviceNo.Add(DeviceNo1);
                            lstDeviceNo.Add(DeviceNo2);
                            lstDeviceNo.Add(DeviceNo3);
                            lstDeviceNo.Add(DeviceNo4);
                            break;
                    case ControllerType.FT8003:
                            lstDeviceNo.Add(DeviceNo1);
                            lstDeviceNo.Add(DeviceNo2);
                            lstDeviceNo.Add(DeviceNo3);
                            lstDeviceNo.Add(DeviceNo4);
                            break;
                    case ControllerType.FT8000:
                            lstDeviceNo.Add(DeviceNo1);
                            lstDeviceNo.Add(DeviceNo2);
                            lstDeviceNo.Add(DeviceNo3);
                            lstDeviceNo.Add(DeviceNo4);
                            lstDeviceNo.Add(DeviceNo5);
                            lstDeviceNo.Add(DeviceNo6);
                            lstDeviceNo.Add(DeviceNo7);
                            lstDeviceNo.Add(DeviceNo8);
                            break;
                }
                
                return lstDeviceNo;

            }
        }

        /// <summary>
        /// 取得所有的器件信息
        /// </summary>
        public List<string> SetDeviceNoList
        {
            set
            {

                List<string> lstDeviceNo = value;                
                switch (_controller.Type)
                {
                    case ControllerType.NT8001:
                        DeviceNo1 = lstDeviceNo[0];
                        DeviceNo2 = lstDeviceNo[1];
                        DeviceNo3 = lstDeviceNo[2];
                        DeviceNo4 = lstDeviceNo[3];
                        DeviceNo5 = lstDeviceNo[4];
                        DeviceNo6 = lstDeviceNo[5];
                        DeviceNo7 = lstDeviceNo[6];
                        DeviceNo8 = lstDeviceNo[7];
                        break;
                    case ControllerType.NT8036:
                        DeviceNo1 = lstDeviceNo[0];
                        DeviceNo2 = lstDeviceNo[1];
                        DeviceNo3 = lstDeviceNo[2];
                        DeviceNo4 = lstDeviceNo[3];
                        break;
                    case ControllerType.NT8007:
                        DeviceNo1 = lstDeviceNo[0];
                        DeviceNo2 = lstDeviceNo[1];
                        DeviceNo3 = lstDeviceNo[2];
                        DeviceNo4 = lstDeviceNo[3];
                        break;
                    case ControllerType.FT8003:
                        DeviceNo1 = lstDeviceNo[0];
                        DeviceNo2 = lstDeviceNo[1];
                        DeviceNo3 = lstDeviceNo[2];
                        DeviceNo4 = lstDeviceNo[3];
                        break;
                    case ControllerType.FT8000:
                        DeviceNo1 = lstDeviceNo[0];
                        DeviceNo2 = lstDeviceNo[1];
                        DeviceNo3 = lstDeviceNo[2];
                        DeviceNo4 = lstDeviceNo[3];
                        DeviceNo5 = lstDeviceNo[4];
                        DeviceNo6 = lstDeviceNo[5];
                        DeviceNo7 = lstDeviceNo[6];
                        DeviceNo8 = lstDeviceNo[7];
                        break;
                }
            }
        }
        #region 非业务数据
        //保存状态
        private bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    this.Controller.IsDirty = true;
                }
            }
        }
        #endregion
    }
}
