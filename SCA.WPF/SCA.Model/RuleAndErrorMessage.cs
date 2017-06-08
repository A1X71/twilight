using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/19 11:22:52
* FileName   : RuleAndMessage
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class RuleAndErrorMessage
    {

        public RuleAndErrorMessage(string a, string b)
        {
            Rule = a;
            ErrorMessage = b;
        }
        public string Rule { get; set; }
        public string ErrorMessage { get; set; }
    }
}
