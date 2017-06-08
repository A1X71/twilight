using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ==============================
*
* Author     : William
* Create Date: 2016/11/4 15:32:37
* FileName   : PortTestingDemo
* Description:
* Version：V1
* ===============================
*/
namespace Test.Console
{
    class PortTestingDemo
   {
        private PortDataDisplay m_portDispl = new PortDataDisplay("COM1", 19200);
        public void Main()
        {
          //  if (btnOpen.Text == "打开串口")
           // {
                m_portDispl.whenGetNew -= portDispl_whenGetNew;
                m_portDispl.whenGetNew += new WhenGetNew(portDispl_whenGetNew);
                m_portDispl.ConnectDeveice();
           //     btnOpen.Text = "关闭串口";
          //  }
          //  else if (btnOpen.Text == "关闭串口")
            {
                m_portDispl.DisconnectDeveice();
          //      btnOpen.Text = "打开串口";
            }
        }
        /// <summary>
        /// 事件
        /// </summary>
        private void portDispl_whenGetNew()
        {
            WhenGetNew ehan = delegate
            {
                //txtDisplay.AppendText(m_portDispl.dataSrc);
            };

            try
            {
               // if (InvokeRequired)
              //  {
              //      this.Invoke(ehan);
              //  }
            }
            catch
            {
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
          //  txtDisplay.Clear();
        }
    }
}
