using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RimageKorea
{
    public class DESHasher
    {
        const string desKey = "rimage00";

        public DESHasher()
        {
            //
            // TODO: 생성자 논리를 여기에 추가합니다.
            //

        }

        //문자열 암호화
        public static string DESEncrypt(string inStr)
        {
            return DesEncrypt(inStr, desKey);
        }

        //문자열 암호화
        private static string DesEncrypt(string str, string key)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            //소스 문자열
            byte[] btSrc = ConvertStringToByteArray(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desencrypt = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);

            cs.Write(btSrc, 0, btSrc.Length);
            cs.FlushFinalBlock();

            byte[] btEncData = ms.ToArray();

            return (ConvertByteArrayToStringB(btEncData));
        }

        // Public Function
        public static string DESDecrypt(string inStr) // 복호화
        {
            return DesDecrypt(inStr, desKey);
        }

        //문자열 복호화
        private static string DesDecrypt(string str, string key)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }


            byte[] btEncData = ConvertStringToByteArrayB(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desdecrypt = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desdecrypt, CryptoStreamMode.Write);

            cs.Write(btEncData, 0, btEncData.Length);
            cs.FlushFinalBlock();

            byte[] btSrc = ms.ToArray();

            return (ConvertByteArrayToString(btSrc));

        }//end of func DesDecrypt

        //문자열->유니코드 바이트 배열
        private static Byte[] ConvertStringToByteArray(String s)
        {
            return (new UnicodeEncoding()).GetBytes(s);
        }

        //유니코드 바이트 배열->문자열
        private static string ConvertByteArrayToString(byte[] b)
        {
            return (new UnicodeEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->안시 바이트 배열
        private static Byte[] ConvertStringToByteArrayA(String s)
        {
            return (new ASCIIEncoding()).GetBytes(s);
        }

        //안시 바이트 배열->문자열
        private static string ConvertByteArrayToStringA(byte[] b)
        {
            return (new ASCIIEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->Base64 바이트 배열
        private static Byte[] ConvertStringToByteArrayB(String s)
        {
            return Convert.FromBase64String(s);
        }

        //Base64 바이트 배열->문자열
        private static string ConvertByteArrayToStringB(byte[] b)
        {
            return Convert.ToBase64String(b);
        }
    }
}
