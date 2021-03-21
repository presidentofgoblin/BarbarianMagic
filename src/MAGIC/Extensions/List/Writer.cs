﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zlib;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Files.CSV_Helpers;

namespace BL.Servers.CoC.Extensions.List
{
    public static class Writer
    {
        public static void AddByte(this List<byte> _Packet, int _Value)
        {
            _Packet.Add((byte)_Value);
        }
        
        public static void AddInt(this List<byte> _Packet, int _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }
        
        public static void AddIntEndian(this List<byte> _Packet, int _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value));
        }
        
        public static void AddInt(this List<byte> _Packet, int _Value, int _Skip)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse().Skip(_Skip));
        }
        
        public static void AddLong(this List<byte> _Packet, long _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }
        
        public static void AddLong(this List<byte> _Packet, long _Value, int _Skip)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse().Skip(_Skip));
        }
        
        public static void AddBool(this List<byte> _Packet, bool _Value)
        {
            _Packet.Add(_Value ? (byte)1 : (byte)0);
        }
        
        public static void AddString(this List<byte> _Packet, string _Value)
        {
            if (_Value == null)
            {
                _Packet.AddInt(-1);
            }
            else
            {
                byte[] _Buffer = Encoding.UTF8.GetBytes(_Value);

                _Packet.AddInt(_Buffer.Length);
                _Packet.AddRange(_Buffer);
            }
        }
        
        public static void AddUShort(this List<byte> _Packet, ushort _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }

        public static void AddCompressed(this List<byte> _Packet, string _Value, bool addbool = true)
        {
            if (addbool)
                _Packet.AddBool(Constants.PacketCompression);

            if (Constants.PacketCompression)
            {
                if (_Value == null)
                    _Packet.AddInt(-1);
                else
                {
                    byte[] Compressed = ZlibStream.CompressString(_Value);

                    _Packet.AddInt(Compressed.Length + 4);
                    _Packet.AddIntEndian(_Value.Length);
                    _Packet.AddRange(Compressed);
                }
            }
            else
                _Packet.AddString(_Value);
        }

        public static void AddHexa(this List<byte> _Packet, string _Value)
        {
            _Packet.AddRange(_Value.HexaToBytes());
        }
        
        public static byte[] HexaToBytes(this string _Value)
        {
            string _Tmp = _Value.Replace("-", string.Empty);
            return Enumerable.Range(0, _Tmp.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(_Tmp.Substring(x, 2), 16)).ToArray();
        }
        
        internal static void AddData(this List<byte> _Writer, Data _Data)
        {
            int Reference = _Data.Id;
            int RowIndex = _Data.GetID();

            _Writer.AddInt(Reference);
            _Writer.AddInt(RowIndex);
        }

        internal static void Shuffle<T>(this IList<T> List)
        {
            int c = List.Count;

            while (c > 1)
            {
                c--;

                int r = Resources.Random.Next(c + 1);

                T Value = List[r];
                List[r] = List[c];
                List[c] = Value;
            }
        }
    }
}
