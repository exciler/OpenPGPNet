
using System;
using SmartCard.Runtime.InteropServices.ISO7816;
using System.IO;

namespace MyCompany.MyOnCardApp
{
    /// <summary>
    /// Summary description for MyService.
    /// </summary>
    public class MyService : MarshalByRefObject
    {
        byte[] AID = new byte[] { 0xD2, 0x76, 0x00, 0x01, 0x24, 0x01, 0x02, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };
        byte[] histBytes = new byte[] { 0x00, 0x73, 0x00, 0x00, 0xC0, 0x05, 0x90, 0x00 };
        byte[] extCap = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF };
        byte[] algoAttr = new byte[] { 0x01, 0x08, 0x00, 0x00, 0x20, 0x01 };

        [APDU("SELECT")]
        public void Select()
        {
        }

        [APDU("00CA0000", Mask = "0000FFFF")]
        public byte[] GetData([APDUParam(APDUHeader.P1P2)] ushort tag)
        {
            switch (tag)
            {
                case 0x004F: return AID;
                case 0x005E: return new byte[0]; //Login data
                case 0x5F50: return new byte[0]; //URL
                case 0x5F52: return histBytes;
                case 0x00C4: return PwStatus();
                case 0x006E:
                    byte[] buf;
                    var ms = new MemoryStream(256);
                    ms.WriteByte(0x6E);
                    ms.Position = 2;
                    ms.WriteByte(0x4F);
                    ms.WriteByte(16);
                    ms.Write(AID, 0, 16);
                    ms.WriteByte(0x5F);
                    ms.WriteByte(0x52);
                    ms.WriteByte(8);
                    ms.Write(histBytes, 0, 8);
                    ms.WriteByte(0xC0);
                    ms.WriteByte(10);
                    ms.Write(extCap, 0, 10);
                    ms.WriteByte(0xC1);
                    ms.WriteByte((byte)algoAttr.Length);
                    ms.Write(algoAttr, 0, algoAttr.Length);
                    ms.WriteByte(0xC2);
                    ms.WriteByte((byte)algoAttr.Length);
                    ms.Write(algoAttr, 0, algoAttr.Length);
                    ms.WriteByte(0xC3);
                    ms.WriteByte((byte)algoAttr.Length);
                    ms.Write(algoAttr, 0, algoAttr.Length);
                    ms.WriteByte(0xC4);
                    ms.WriteByte(7);
                    buf = PwStatus();
                    ms.Write(buf, 0, 7);
                    int len = (int)ms.Position - 2;
                    ms.Position = 1;
                    ms.WriteByte((byte)len);
                    ms.Position = 0;
                    buf = new byte[len + 2];
                    ms.Read(buf, 0, len + 2);
                    return buf;
                default: return new byte[0];
            }
        }

        byte[] PwStatus()
        {
            return new byte[] { 0x00, 0x10, 0x20, 0x10, 0x03, 0x03, 0x03 };
        }
    }
}

