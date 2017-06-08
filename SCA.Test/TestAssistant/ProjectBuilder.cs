using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/13 16:16:32
* FileName   : ProjectBuilder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{
    class ProjectBuilder
    {
        int _id=1;
        string _name="秦皇岛火车站";
        string _savePath="E:\\StarUniversity.db";
        int _saveInterval = 30;
        List<ControllerModel> _lstController;
        //ControllerModel _controllerModel;
        public Model.ProjectModel Build()
        {
            ProjectModel projModel = new ProjectModel
            {
                ID = _id,
                Name = _name,
                SavePath = _savePath,
                SaveInterval = _saveInterval                
            };
            if(_lstController!=null )
            { 
                foreach(var c in _lstController)
                {
                    projModel.Controllers.Add(c);
                }
            }
            return projModel;
        }


        public ProjectBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProjectBuilder WithSavePath(string path)
        {
            _savePath = path;
            return this;
        }

        public ProjectBuilder WithController(List<ControllerModel> lstControllerModel)
        {
            _lstController = lstControllerModel;
            return this;
        }
        //public ProjectBuilder WithController(ControllerModel controllerModel)
        //{
        //       //_controllerModel=new ControllerModel();
        //       _controllerModel= controllerModel;
        //    return this;
        //}


    }
}
