using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.DatabaseAccess
{
    public static  class Database
    {
        public static ProjectModel[] GetProject()
        {
            return new ProjectModel[]{new ProjectModel(1,"碧水华庭",1)};
        }
        public static ControllerModel[] GetControllers(ProjectModel project)
        {
            return new ControllerModel[] 
            {
                new ControllerModel(1, "NT8001",ControllerType.NT8001,2),
                new ControllerModel(2,"NT8036",ControllerType.NT8036,2) 
            };
        }
        public static LoopModel[] GetLoops(int controllerID)
        {
            return new LoopModel[]
            {
                new LoopModel(4,1,"0101","Loop1",1,4),
                new LoopModel(4,1,"0102","Loop2",1,4),
                new LoopModel(4,1,"0103","Loop3",1,4)
            };
        }
        public static ControllerNodeModel[] GetControllerNodes(ControllerModel controller)
        {
            if (controller.Type == ControllerType.NT8001)
            {
                return new ControllerNodeModel[]
                {
                    //new ControllerNodeModel(ControllerNodeType.Loop,"回路",3),
                    //new ControllerNodeModel(ControllerNodeType.Standard,"标准组态",3),
                    //new ControllerNodeModel(ControllerNodeType.General,"通用组态",3),
                    //new ControllerNodeModel(ControllerNodeType.Mixed,"混合组态",3),
                    //new ControllerNodeModel(ControllerNodeType.Board,"网络手动盘",3)
                };
            }
            else
            {
                return new ControllerNodeModel[]
                {
                    //new ControllerNodeModel(ControllerNodeType.Loop,"回路",3),
                    //new ControllerNodeModel(ControllerNodeType.Standard,"标准组态",3),
                    
                };
            }
        }
    }
}
