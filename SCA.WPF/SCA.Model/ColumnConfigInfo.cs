using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Model
{
    public class ColumnConfigInfo
    {
       public  string ColumnOrder{get;set;}

       public string ColumnName{get;set;}
       public  string ColumnType{get;set;}
       public string ColumnConstraints{get;set;}
       public ColumnConfigInfo(string name)
       {
           ColumnName = name;           
       }
       public ColumnConfigInfo()
       { 
       
       }
       public ColumnConfigInfo(string name, string type, string order, string constraints)
       {
           ColumnName = name;
           ColumnType = type;
           ColumnOrder = order;
           ColumnConstraints = constraints;
       }
        
    }
}
