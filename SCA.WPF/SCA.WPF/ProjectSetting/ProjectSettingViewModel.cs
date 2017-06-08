using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.BusinessLib;
using SCA.Model;
using Caliburn.Micro;
using System.Windows;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/16 13:39:52
* FileName   : ProjectSettingViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ProjectSetting
{
    public class ProjectSettingViewModel:Caliburn.Micro.PropertyChangedBase
    {
        private int _selectedID;
        private int _saveInterval= ProjectManager.GetInstance.Project.SaveInterval;
        public int SelectedID
        {
            get
            {
                ProjectModel project = ProjectManager.GetInstance.Project;
                Dictionary<int, string> dict = new Dictionary<int, string>();
                foreach (var c in project.Controllers)
                {
                    if (c.PrimaryFlag)
                    {
                        _selectedID = c.ID;
                    }
                }
                return _selectedID;
            }
            private set
            {
                _selectedID = value;
            }
        }
        public int SaveInterval
        {
            get
            {                
                return _saveInterval;
            }
            set
            {
                _saveInterval = value;
                NotifyOfPropertyChange("SaveInterval");
            }
        }
        private Dictionary<int, string> _controller { get; set; }
        public Dictionary<int, string> Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
                NotifyOfPropertyChange("Controller");
            }
        }

        /// <summary>
        /// 取得当前项目中的所有控制器信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,string> GetControllers()
        {
            ProjectModel project = ProjectManager.GetInstance.Project;
            Dictionary<int, string> dict = new Dictionary<int, string>();
            foreach (var c in project.Controllers)
            {
                int cID=c.ID;
                string cName = c.Name +" ( " + cID.ToString() + " )";
                dict.Add(cID, cName);
            }
            GetPrimaryControllerID();
            Controller = dict;
            return dict;
         //SelectedValuePath="Value"   
        }
        /// <summary>
        /// 获取主控制器ID
        /// </summary>
        public void  GetPrimaryControllerID()
        {
            ProjectModel project = ProjectManager.GetInstance.Project;
            Dictionary<int, string> dict = new Dictionary<int, string>();
            foreach (var c in project.Controllers)
            {
                if (c.PrimaryFlag)
                {
                    SelectedID = c.ID;
                }                
            }      
        }
    }
}
