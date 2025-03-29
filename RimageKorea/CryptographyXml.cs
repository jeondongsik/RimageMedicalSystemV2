using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace RimageKorea
{
    public class CryptographyXml
    {
        /// <summary>
        /// Xml 노드 하위 암호화
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="ElementToEncrypt"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        public static void EncryptXml(XmlDocument Doc, byte[] Key, byte[] IV)
        {
            XmlElement element = Doc.DocumentElement;

            if (element == null)
                throw new Exception("암호화할 루트 요소가 없습니다.");

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = Key;
                aes.IV = IV;

                EncryptedXml encryptedXml = new EncryptedXml();
                byte[] encryptedElement = encryptedXml.EncryptData(element, aes, false);

                EncryptedData edElement = new EncryptedData
                {
                    Type = EncryptedXml.XmlEncElementUrl,
                    EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url),
                    CipherData = new CipherData(encryptedElement)
                };

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoName("aesKey"));  // KeyName 지정
                edElement.KeyInfo = keyInfo;

                EncryptedXml.ReplaceElement(element, edElement, false);
            }
        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        public static void DecryptXml(XmlDocument Doc, byte[] Key, byte[] IV)
        {
            try
            {
                using (AesManaged aes = new AesManaged())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    EncryptedXml encryptedXml = new EncryptedXml(Doc);
                    encryptedXml.AddKeyNameMapping("aesKey", aes);
                    encryptedXml.DecryptDocument();
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static SymmetricAlgorithm CreateAes(byte[] Key, byte[] IV)
        {
            AesManaged aes = new AesManaged();
            aes.Key = Key;
            aes.IV = IV;
            return aes;
        }
    }
}
