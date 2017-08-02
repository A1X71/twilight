using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
/* ==============================
*
* Author     : William
* Create Date: 2017/1/19 14:13:32
* FileName   : LinkageConfigGeneral
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class LinkageConfigGeneral
    {
        private ControllerModel _controller; //Added at 2016-12-06 控制器信息

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
        public int ControllerID{get;set;}
        public int ID { get; set; }
        public string Code { get; set; }
        public int ActionCoefficient { get; set; }
        public int CategoryA { get; set; }          //类别A
        public int? BuildingNoA { get; set; }
        public int? ZoneNoA { get; set; }
        public int? LayerNoA1 { get; set; }
        public int? LayerNoA2 { get; set; }
        public Int16 DeviceTypeCodeA { get; set; }
        //器件类型名称(兼容老版数据文件保留)
        //老版软件的器件类型以“字符名称”存储
        public string DeviceTypeNameA{ get; set; }

        public LinkageType TypeC { get; set; }
        public string MachineNoC { get; set; }
        public string LoopNoC { get; set; }
        public string DeviceCodeC { get; set; }
        public int? BuildingNoC { get; set; }
        public  int? ZoneNoC { get; set; }
        public int? LayerNoC { get; set; }
        public Int16 DeviceTypeCodeC { get; set; }
        //器件类型名称(兼容老版数据文件保留)
        //老版软件的器件类型以“字符名称”存储
        public string DeviceTypeNameC { get; set; }
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
        public string FormattedMachineNoC
        {
            get
            {
                if (MachineNoC != "")
                {
                    return MachineNoC.PadLeft(Controller.MachineNumber.Length, '0');
                }
                return MachineNoC;
            }
        }
        public string FormattedLoopNoC
        {
            get
            {
                if (LoopNoC != "")
                {
                    return LoopNoC.PadLeft(Controller.LoopAddressLength, '0');
                }
                return LoopNoC;
            }
        }
        public string FormattedDeviceCodeC
        {
            get
            {
                if (DeviceCodeC != "")
                {
                    return DeviceCodeC.PadLeft(Controller.DeviceAddressLength, '0');
                }
                return DeviceCodeC;
            }
        }
        #endregion


    }
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LinkageType
    {
        [Description("区层")]
        ZoneLayer=1,//区层
        [Description("本层")]
        SameLayer=2, //本层
        [Description("邻层")]
        AdjacentLayer=3,//邻层
        [Description("地址")]
        Address=4,//地址
        [Description("无")]
        None=0

    }
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LinkageActionType
    { 
        [Description("与")]
        AND=1,//与
        [Description("或")]
        OR=2, //或
        NONE=0
           
    }
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {

        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)) ? attributes[0].Description : value.ToString());
                    }
                }
                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
