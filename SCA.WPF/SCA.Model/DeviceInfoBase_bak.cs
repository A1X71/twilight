using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Model
{
    /// <summary>
    /// 器件信息基类
    /// </summary>
   public  class DeviceInfoBase
    {
       private LoopModel _loop; //Added at 2016-12-06 回路信息
       /// <summary>
       /// 回路ID
       /// </summary>
       public Int32 LoopID { get; set; }　//Commented at 2016-12-06 有了回路信息，此字段或许可去掉
       /// <summary>
       /// 标识ID
       /// </summary>
       public Int32 ID { get; set;}
       /// <summary>
       /// 器件编码
       /// </summary>
       public string Code { get; set; }
       /// <summary>
       /// 器件类型
       /// </summary>
       public Int16 Type { get; set; }       
       /// <summary>
       /// 区号
       /// </summary>
       public  Int16  ZoneNo { get; set; }             
       /// <summary>
       /// 安装地点
       /// </summary>
       public string Location { get; set; }

       public Int16 Disable { get; set; }
        //public string BuildingNo { get; set; }
        //public string FloorNo { get; set; }
        //public string RoomNo { get; set; }

       public virtual Int16 GetDevType(Int16 deviceType)
       {
           switch (ConvertSwitchCondition(deviceType))
           {
               case 0: return 0;
               case -1: return deviceType; //1-10
               case -2:return 12; //11-12
               case 13: return 13;
               case 14: return 17;
               case 15: return 15;
               case -3: return 4;  //16 - 17
               case -4: return 2;  //18 - 22
               case 23: return 18;
               case -5: return 4; //24 - 30
               case 31: return 23;
               case 32: return 24;
               case 33: return 21;
               case 34: return 22;
               case 35: return 24;
               case 36: return 29;
               case -6: return 24; //37 - 65
               case 66: return 29;
               case 67: return 20;
               case 68: return 30;
               case -7 : return 31; //69-70               
               case -8: return 24; //71 - 73
               case 74: return 28;
               case 75: return 25;
               case 76: return 26;
               case 77: return 27;
               case 78: return 28;
               case 79: return 18;
               case 80: return 19;
               case 81: return 14;
               case -9: return 16; //82 - 86
               case -10: return 11; //87 - 89
               case -11: return 23;//101 - 129
               default: return 16;

           }
       }
       /// <summary>
       /// 将一定范围内的值转换为一个值，方便Switch case判断
       /// </summary>
       /// <param name="originalValue"></param>
       /// <returns></returns>
       private Int16 ConvertSwitchCondition(Int16 originalValue)
       {
           if (originalValue >= 1 && originalValue <= 10)
               return -1;
           else if (originalValue >= 11 && originalValue <= 12)
               return -2;
           else if (originalValue >= 16 && originalValue <= 17)
               return -3;
           else if (originalValue >= 18 && originalValue <= 22)
               return -4;
           else if (originalValue >= 24 && originalValue <= 30)
               return -5;
           else if (originalValue >= 37 && originalValue <= 65)
               return -6;
           else if (originalValue >= 69 && originalValue <= 70)
               return -7;
           else if (originalValue >= 71 && originalValue <= 73)
               return -8;
           else if (originalValue >= 82 && originalValue <= 86)
               return -9;
           else if (originalValue >= 87 && originalValue <= 89)
               return -10;
           else if (originalValue >= 101 && originalValue <= 129)
               return -11;
           else
               return originalValue;
       }
       public void test()
       {
           int switchExpression = 3;
           switch (switchExpression)
           {
               // A switch section can have more than one case label. 
               case 0:
               case 1:
                   Console.WriteLine("Case 0 or 1");
                   // Most switch sections contain a jump statement, such as 
                   // a break, goto, or return. The end of the statement list 
                   // must be unreachable. 
                   break;
               case 2:
                   Console.WriteLine("Case 2");
                   break;
                   // The following line causes a warning.
                   Console.WriteLine("Unreachable code");
               // 7 - 4 in the following line evaluates to 3. 
               case 7 - 4:
                   Console.WriteLine("Case 3");
                   break;
               // If the value of switchExpression is not 0, 1, 2, or 3, the 
               // default case is executed.
               case 66 - 41:
                   break;
               default:
                   Console.WriteLine("Default case (optional)");
                   // You cannot "fall through" any switch section, including
                   // the last one. 
                   break;
           }
       }
       public LoopModel Loop
       {
           get 
           {
               return _loop;
           }
           set
           {
               _loop = value;
           }
       }

    }
}
