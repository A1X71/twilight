using System.Collections.ObjectModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 11:05:27
* FileName   : DeviceInfo8036Collection
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels
{
    public class DeviceInfo8036Collection:ObservableCollection<SCA.Model.DeviceInfo8036>
    {
        public DeviceInfo8036Collection()
        {
            CreateDeviceInfo8036Data(1);
        }
        public void CreateDeviceInfo8036Data(int multiplier)
        {
            if (multiplier > 0)
            {
                for (int i = 0; i < multiplier; i++)
                {
                    Add(new Model.DeviceInfo8036 { ID = i, Code = i.ToString(), BuildingNo = 7 });
                }
            }
        }

    }
}
