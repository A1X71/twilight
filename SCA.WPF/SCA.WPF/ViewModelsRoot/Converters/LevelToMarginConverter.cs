using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/28 11:53:57
* FileName   : LevelToMarginConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Converters
{
    public class LevelToMarginConverter:System.Windows.Data.IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var level = (int)value;
            return new System.Windows.Thickness(8 * level + 10 * (level - 1), 0, 0, 0);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class TreeViewData
    {
        private static TreeViewData _Data = null;

        public static TreeViewData Data
        {
            get
            {
                if (_Data == null)
                {
                    _Data = new TreeViewData();

                    var rn1 = new TreeNode() { Label = "海南棕榈广场项目", Level = 1, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Proj.jpg" };
                    var rn11 = new TreeNode() { Label = "NT8001控制器", Level = 2, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Controller.jpg" };
                    //rn11.ChildNodes.Add(new TreeNode() { Label = "回路(2)", Level = 3 });
                    var rn111 = new TreeNode() { Label = "回路(2)", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg" };
                    rn111.ChildNodes.Add(new TreeNode() { Label = "回路1(一层)", Level = 4, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg" });
                    rn111.ChildNodes.Add(new TreeNode() { Label = "回路2(二层)", Level = 4, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg" });
                    rn11.ChildNodes.Add(rn111);
                    rn11.ChildNodes.Add(new TreeNode() { Label = "标准组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    rn11.ChildNodes.Add(new TreeNode() { Label = "混合组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    rn11.ChildNodes.Add(new TreeNode() { Label = "通用组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    rn11.ChildNodes.Add(new TreeNode() { Label = "网络手动盘组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Board.jpg" });
                    rn1.ChildNodes.Add(rn11);
                    var rn12 = new TreeNode() { Label = "NT8036控制器", Level = 2, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Controller.jpg" };
                    rn12.ChildNodes.Add(new TreeNode() { Label = "回路", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg" });
                    rn12.ChildNodes.Add(new TreeNode() { Label = "标准组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    rn1.ChildNodes.Add(rn12);

                    _Data.RootNodes.Add(rn1);
                }
                return _Data;
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<TreeNode> _RootNodes = null;

        public IList<TreeNode> RootNodes { get { return _RootNodes ?? (_RootNodes = new System.Collections.ObjectModel.ObservableCollection<TreeNode>()); } }

        public class TreeNode
        {
            public string Label { get; set; }
            public string Icon { get; set; }
            public int Level { get; set; }

            private System.Collections.ObjectModel.ObservableCollection<TreeNode> _ChildNodes = null;

            public IList<TreeNode> ChildNodes { get { return _ChildNodes ?? (_ChildNodes = new System.Collections.ObjectModel.ObservableCollection<TreeNode>()); } }
        }
    }
    public class TreeListViewData
    {
        private static TreeListViewData _Data = null;

        public static TreeListViewData Data
        {
            get
            {
                if (_Data == null)
                {
                    _Data = new TreeListViewData();

                    var rn1 = new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Proj.jpg", Name = "工程", Number = 1 };
                    var rn11 = new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Controller.jpg", Name = "控制器", Number = 2 };

                    var rn111 = new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg", Name = "回路", Number = 2 };
                    rn111.ChildNodes.Add(new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg", Name = "回路1(一层)", Number = 0 });
                    rn111.ChildNodes.Add(new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg", Name = "回路2(二层)", Number = 1 });
                    rn11.ChildNodes.Add(rn111);
                    rn11.ChildNodes.Add(new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg", Name = "标准组态", Number = 1 });
                    //rn11.ChildNodes.Add(new SummaryInfo_List() { Label = "混合组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    //rn11.ChildNodes.Add(new SummaryInfo_List() { Label = "通用组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg" });
                    //rn11.ChildNodes.Add(new SummaryInfo_List() { Label = "网络手动盘组态", Level = 3, Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Board.jpg" });
                    rn1.ChildNodes.Add(rn11);
                    var rn12 = new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Controller.jpg", Name = "控制器", Number = 2 };
                    var rn121 = new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg", Name = "回路", Number = 2 };
                    rn12.ChildNodes.Add(rn121);
                    rn12.ChildNodes.Add(new SummaryInfo_List() { Icon = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Linkage.jpg", Name = "标准组态", Number = 1 });
                    rn1.ChildNodes.Add(rn12);

                    _Data.RootNodes.Add(rn1);
                }
                return _Data;
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<SummaryInfo_List> _RootNodes = null;

        public IList<SummaryInfo_List> RootNodes { get { return _RootNodes ?? (_RootNodes = new System.Collections.ObjectModel.ObservableCollection<SummaryInfo_List>()); } }
        public class SummaryInfo_List
        {
            public SummaryInfo_List() { }
            public SummaryInfo_List(string p_name, int? p_number, string p_icon)
            {

                this._icon = p_icon;
                this._number = p_number;
                this._name = p_name;

            }
            private string _icon;
            private int? _number;
            private string _name;


            public string Icon
            {
                get { return this._icon; }
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
            private ObservableCollection<SummaryInfo_List> _childNode = new ObservableCollection<SummaryInfo_List>();
            public ObservableCollection<SummaryInfo_List> ChildNodes
            {
                get { return _childNode; }
            }



        }

    }
}
