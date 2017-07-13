using System.Collections.ObjectModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/13 10:54:49
* FileName   : SummaryInfo
* Description: 用于摘要信息显示
* Version：V1
* ===============================
*/
namespace SCA.Model.BusinessModel
{
    public class SummaryInfo
    {
        public SummaryInfo() { }
        public SummaryInfo(string p_name, int? p_number, string p_icon)
        {

            this._icon = p_icon;
            this._number = p_number;
            this._name = p_name;

        }
        private string _icon;
        private int? _number;
        private string _name;
        private int _level; //显示级别，用于控制不同级别的缩进量


        public string Icon
        {
            get 
            {
                if (this._icon.IndexOf(':') == -1)
                {
                    return System.AppDomain.CurrentDomain.BaseDirectory + _icon;
                }
                return this._icon; 
            }
            set { this._icon = value; }
        }
        public int? Number
        {
            get { return this._number; }
            set { this._number = value; }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

            }
        }
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        private ObservableCollection<SummaryInfo> _childNode = new ObservableCollection<SummaryInfo>();
        public ObservableCollection<SummaryInfo> ChildNodes
        {
            get { return _childNode; }
        }
    }
}
