using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface
{
    public interface ILoopService
    {
        List<Model.LoopModel> CreateLoops<T>(int loopsAmount, int deviceAmount, ControllerModel controller,IDeviceService<T> deviceService);
        bool DeleteLoopBySpecifiedLoopID(string strLoopID);
        bool DeleteLoopBySpecifiedLoopCode(string strLoopCode);

        //bool UpdateLoop(LoopModel loop);
        LoopModel ModifyLoopNameBySpecifiedLoopID(string strLoopID);
        /// <summary>
        /// 对指定的回路信息进行复制
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        List<Model.LoopModel> CopyLoopInfo(LoopModel loop);
        ControllerModel PasteLoopInfo();
        ControllerModel SwapLoopInfo();
        bool SetLoopInfoWithAll();//回路下传
        bool SetLoopInfoWithPartial();//部分下传
        bool CalculateLoop();//传递参数及返回结果类型，仍需要再定义
                
        bool ValidateLoopInfo();//实器检查的应该为器件信息。对回路中包含的所有器件进行有效性检查
        /// <summary>
        /// 将“回路信息”增加至主控制器中。
        /// </summary>
        /// <returns></returns>
        bool AddLoops(LoopModel loop,string machineNumber,int loopsAmount); 
        /// <summary>
        /// 取得最大回路号
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>        
        int[] GetAllLoopCode(ControllerModel controller, Func<int[], int[]> sort);
        
    }
}
