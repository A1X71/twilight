using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/8/14 10:10:47
* FileName   : ProjectConfig
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ProjectConfig
    {
        public Dictionary<string, RuleAndErrorMessage> GetProjectInfoRegularExpression()
        {
            Dictionary<string, RuleAndErrorMessage> dictControllerInfoRE = new Dictionary<string, RuleAndErrorMessage>();
            //名称            
            dictControllerInfoRE.Add("Name", new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF()（）]{0,20}$", "允许填写”中文字符、英文字符、阿拉伯数字、圆括号”,最大长度20个字符"));
            return dictControllerInfoRE;

        }
    }
}
