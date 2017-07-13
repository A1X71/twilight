using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/13 14:40:10
* FileName   : CollectionEquality
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public class CollectionEqualityComparer<T> : IEqualityComparer<T>
    {
        readonly Func<T, T, bool> _comparer;
        readonly Func<T, int> _hash;
        public CollectionEqualityComparer(Func<T, T, bool> comparer) :
            this(comparer, o => 0)
    {
    }
        public CollectionEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            _comparer = comparer;
            _hash = hash;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hash(obj);
        }

        //public bool Equals(IDevice x, IDevice y)
        //{
        //    //if (x.TypeCode == y.TypeCode)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //    if (Object.ReferenceEquals(x, y)) return true;

        //    if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
        //        return false;
        //    return x.TypeCode == y.TypeCode;
        //}

        //public int GetHashCode(IDevice obj)
        //{
        //    //Check whether the object is null
        //    if (Object.ReferenceEquals(obj, null)) return 0;

        //    //Get hash code for the Name field if it is not null.
        //    int hashDeviceCode = obj.TypeCode == null ? 0 : obj.TypeCode.GetHashCode();

        //    //Get hash code for the Code field.
        //    int hashDeviceTypeCode = obj.TypeCode.GetHashCode();

        //    //Calculate the hash code for the product.
        //    return hashDeviceCode ^ hashDeviceTypeCode;
        //}

    }
}
