using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/6 15:39:27
* FileName   : BytesOperation
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    class BytesOperation
    {


        /// <summary>
        /// 字节操作类
        /// </summary>
        public struct BytesIO
        {
            #region Field

            private byte[] _Raw;

            private int _Position;
            /// <summary>
            /// 获取当前位置
            /// </summary>
            public int Position
            {
                get { return _Position; }
            }

            public BytesIO(byte[] raw)
            {
                _Raw = raw;
                _Position = 0;
            }

            #endregion

            #region Input
            /// <summary>
            /// 写入布尔值,并移动指针
            /// </summary>
            public void Put(bool value)
            {
                _Raw[_Position++] = value ? (byte)1 : (byte)0;
            }
            /// <summary>
            /// 写入布尔值到指定位置
            /// </summary>
            public void Put(int index, bool value)
            {
                _Raw[index] = value ? (byte)1 : (byte)0;
            }
            /// <summary>
            /// 写入字节值,并移动指针
            /// </summary>
            public void Put(byte value)
            {
                _Raw[_Position++] = value;
            }
            /// <summary>
            /// 写入字节值到指定位置,并移动指针
            /// </summary>
            public void Put(int index, byte value)
            {
                _Raw[index] = value;
            }
            /// <summary>
            /// 写入字节数组,并移动指针
            /// </summary>
            public void Put(byte[] value)
            {
                if (value == null) return;
                foreach (var item in value)
                    _Raw[_Position++] = item;
            }
            /// <summary>
            /// 写入ASCII字符,并移动指针
            /// </summary>
            public void Put(char ch)
            {
                _Raw[_Position++] = Convert.ToByte(ch);
            }
            /// <summary>
            /// 写入ASCII字符到指定位置
            /// </summary>
            public void Put(int index, char ch)
            {
                _Raw[index] = Convert.ToByte(ch);
            }
            /// <summary>
            /// 写入ASCII字符串,并移动指针
            /// </summary>
            public void Put(char[] chars)
            {
                if (chars == null) return;
                foreach (var item in chars)
                    _Raw[_Position++] = Convert.ToByte(item);
            }
            /// <summary>
            /// 从指定位置写入ASCII字符串
            /// </summary>
            public void Put(int index, char[] chars)
            {
                if (chars == null) return;
                foreach (var item in chars)
                    _Raw[index++] = Convert.ToByte(item);
            }
            /// <summary>
            /// 写入16位有符号整数,并移动指针
            /// </summary>
            public void Put(short value)
            {
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)value;
            }
            /// <summary>
            /// 写入16位有符号整数到指定位置
            /// </summary>
            public void Put(int index, short value)
            {
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)value;
            }
            /// <summary>
            /// 写入16位无符号整数,并移动指针
            /// </summary>
            public void Put(ushort value)
            {
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)value;
            }
            /// <summary>
            /// 写入16位无符号整数到指定位置
            /// </summary>
            public void Put(int index, ushort value)
            {
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)value;
            }
            /// <summary>
            /// 写入32位有符号的整数,并移动指针
            /// </summary>
            public void Put(int value)
            {
                _Raw[_Position++] = (byte)(value >> 0x18);
                _Raw[_Position++] = (byte)(value >> 0x10);
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)(value);
            }
            /// <summary>
            /// 写入32位有符号的整数到指定位置
            /// </summary>
            public void Put(int index, int value)
            {
                _Raw[index++] = (byte)(value >> 0x18);
                _Raw[index++] = (byte)(value >> 0x10);
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)(value);
            }
            /// <summary>
            /// 写入32位无符号的整数,并移动指针
            /// </summary>
            public void Put(uint value)
            {
                _Raw[_Position++] = (byte)(value >> 0x18);
                _Raw[_Position++] = (byte)(value >> 0x10);
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)(value);
            }
            /// <summary>
            /// 写入32位有符号的整数到指定位置
            /// </summary>
            public void Put(int index, uint value)
            {
                _Raw[index++] = (byte)(value >> 0x18);
                _Raw[index++] = (byte)(value >> 0x10);
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)(value);
            }
            /// <summary>
            /// 写入64位有符号的整数，并移动指针
            /// </summary>
            public void Put(long value)
            {
                _Raw[_Position++] = (byte)(value >> 0x38);
                _Raw[_Position++] = (byte)(value >> 0x30);
                _Raw[_Position++] = (byte)(value >> 0x28);
                _Raw[_Position++] = (byte)(value >> 0x20);
                _Raw[_Position++] = (byte)(value >> 0x18);
                _Raw[_Position++] = (byte)(value >> 0x10);
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)value;
            }
            /// <summary>
            /// 写入64位有符号的整数到指定位置
            /// </summary>
            public void Put(int index, long value)
            {
                _Raw[index++] = (byte)(value >> 0x38);
                _Raw[index++] = (byte)(value >> 0x30);
                _Raw[index++] = (byte)(value >> 0x28);
                _Raw[index++] = (byte)(value >> 0x20);
                _Raw[index++] = (byte)(value >> 0x18);
                _Raw[index++] = (byte)(value >> 0x10);
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)value;
            }
            /// <summary>
            /// 写入64位无符号的整数，并移动指针
            /// </summary>
            public void Put(ulong value)
            {
                _Raw[_Position++] = (byte)(value >> 0x38);
                _Raw[_Position++] = (byte)(value >> 0x30);
                _Raw[_Position++] = (byte)(value >> 0x28);
                _Raw[_Position++] = (byte)(value >> 0x20);
                _Raw[_Position++] = (byte)(value >> 0x18);
                _Raw[_Position++] = (byte)(value >> 0x10);
                _Raw[_Position++] = (byte)(value >> 8);
                _Raw[_Position++] = (byte)value;
            }
            /// <summary>
            /// 写入64位有符号的整数到指定位置
            /// </summary>
            public void Put(int index, ulong value)
            {
                _Raw[index++] = (byte)(value >> 0x38);
                _Raw[index++] = (byte)(value >> 0x30);
                _Raw[index++] = (byte)(value >> 0x28);
                _Raw[index++] = (byte)(value >> 0x20);
                _Raw[index++] = (byte)(value >> 0x18);
                _Raw[index++] = (byte)(value >> 0x10);
                _Raw[index++] = (byte)(value >> 8);
                _Raw[index] = (byte)value;
            }
            /// <summary>
            /// 写入Guid值，并移动指针
            /// </summary>
            public void Put(Guid guid)
            {
                Put(guid.ToByteArray());
            }

            #endregion

            #region Output
            /// <summary>
            /// 读取布尔值，并移动指针
            /// </summary>
            public bool GetBoolean()
            {
                return Get() == 0 ? false : true;
            }
            /// <summary>
            /// 从指定位置读取布尔值
            /// </summary>
            public bool GetBoolean(int index)
            {
                return _Raw[index] == 0 ? false : true;
            }
            /// <summary>
            /// 读取一个字节，并移动指针
            /// </summary>
            public byte Get()
            {
                var value = _Raw[_Position++];
                return value;
            }
            /// <summary>
            /// 从指定位置读取一个字节
            /// </summary>
            public byte Get(int index)
            {
                return _Raw[index];
            }
            /// <summary>
            /// 读取指定长度的字节数组, 并移动指针
            /// </summary>
            public byte[] GetBytes(int count)
            {
                if (count < 0)
                    return null;

                if (_Position + count > _Raw.Length)
                    return null;

                byte[] buffer = new byte[count];

                Buffer.BlockCopy(_Raw, _Position, buffer, 0, count);
                _Position += count;

                return buffer;
            }
            /// <summary>
            /// 从指定位置读取指定长度的字节数组
            /// </summary>
            public byte[] GetBytes(int index, int count)
            {
                if (count < 0)
                    return null;

                if (index + count > _Raw.Length)
                    return null;

                byte[] buffer = new byte[count];

                Buffer.BlockCopy(_Raw, index, buffer, 0, count);

                return buffer;
            }
            /// <summary>
            /// 读取一个Ascii字符，并移动指针
            /// </summary>
            public char GetChar()
            {
                return Convert.ToChar(Get());
            }
            /// <summary>
            /// 从指定位置读取一个Ascii字符
            /// </summary>
            public char GetChar(int index)
            {
                return Convert.ToChar(_Raw[index]);
            }
            /// <summary>
            /// 读取16位有符号整数, 并移动指针
            /// </summary>
            public short GetShort()
            {
                return (short)(Get() << 8 | Get());
            }
            /// <summary>
            /// 从指定位置读取16位有符号整数
            /// </summary>
            public short GetShort(int index)
            {
                return (short)(_Raw[index++] << 8 | _Raw[index]);
            }
            /// <summary>
            /// 读取16位无符号整数, 并移动指针
            /// </summary>
            public ushort GetUShort()
            {
                return (ushort)(Get() << 8 | Get());
            }
            /// <summary>
            /// 从指定位置读取16位无符号整数
            /// </summary>
            public ushort GetUShort(int index)
            {
                return (ushort)(_Raw[index++] << 8 | _Raw[index]);
            }
            /// <summary>
            /// 读取32位有符号整数，并移动指针
            /// </summary>
            public int GetInt()
            {
                return (int)(Get() << 0x18 | Get() << 0x10 | Get() << 8 | Get());
            }
            /// <summary>
            /// 从指定位置读取32位有符号整数
            /// </summary>
            public int GetInt(int index)
            {
                return (int)(_Raw[index++] << 0x18 | _Raw[index++] << 0x10 | _Raw[index++] << 8 | _Raw[index]);
            }
            /// <summary>
            /// 读取32位无符号整数，并移动指针
            /// </summary>
            public uint GetUInt()
            {
                return (uint)GetInt();
            }
            /// <summary>
            /// 从指定位置读取32位无符号整数
            /// </summary>
            public uint GetUInt(int index)
            {
                return (uint)GetInt(index);
            }
            /// <summary>
            /// 读取64位有符号整数，并移动指针
            /// </summary>
            public long GetLong()
            {
                ulong x = (ulong)GetUInt();
                ulong y = (ulong)GetUInt();
                return (long)((x << 0x20) | y);
            }
            /// <summary>
            /// 从指定位置读取64位有符号整数
            /// </summary>
            public long GetLong(int index)
            {
                ulong x = (ulong)GetUInt(index);
                ulong y = (ulong)GetUInt(index + 4);
                return (long)((x << 0x20) | y);
            }
            /// <summary>
            /// 读取64位无符号整数，并移动指针
            /// </summary>
            public ulong GetULong()
            {
                return (ulong)GetLong();
            }
            /// <summary>
            /// 从指定位置读取64位无符号整数
            /// </summary>
            public ulong GetULong(int index)
            {
                return (ulong)GetLong(index);
            }
            /// <summary>
            /// 读取Guid值,并移动指针
            /// </summary>
            public Guid GetGuid()
            {
                return new Guid(GetBytes(16));
            }

            #endregion

            #region Public Methods
            /// <summary>
            /// 重设指针
            /// </summary>
            public void ResetPosition(int index)
            {
                if (index < 0)
                    _Position = 0;
                else if (_Raw != null && index >= _Raw.Length)
                    _Position = _Raw.Length - 1;
                else
                    _Position = index;
            }
            /// <summary>
            /// 查看是否还可读
            /// </summary>
            public bool Peek()
            {
                if (_Raw == null) return false;
                return _Position < _Raw.Length;
            }
            /// <summary>
            /// 查看还有多少字节可读
            /// </summary>
            /// <returns></returns>
            public int BytesLeft()
            {
                return _Raw.Length - _Position;
            }

            #endregion

        }

    }
}
