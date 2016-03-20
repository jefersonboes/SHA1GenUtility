using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SHA1GenUtility
{
    class HashComputer
    {
        private byte[] ComputeHashSha1(Stream stream)
        {
            SHA1Managed sha1 = new SHA1Managed();

            return sha1.ComputeHash(stream);
        }

        private byte[] ComputeHashSha256(Stream stream)
        {
            SHA256Managed sha256 = new SHA256Managed();

            return sha256.ComputeHash(stream);
        }

        private byte[] ComputeHashMD5(Stream stream)
        {
            MD5 md5 = MD5.Create();

            return md5.ComputeHash(stream);
        }

        public string compute(String filename, int hashType)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs);
            try
            {
                byte[] hash = null;

                switch (hashType)
                {
                    case 0:
                        hash = ComputeHashSha1(bs);
                        break;
                    case 1:
                        hash = ComputeHashSha256(bs);
                        break;
                    case 2:
                        hash = ComputeHashMD5(bs);
                        break;
                }
                
                StringBuilder formatted = new StringBuilder(2 * hash.Length);
                foreach (byte b in hash)
                {
                    formatted.AppendFormat("{0:X2}", b);
                }

                return formatted.ToString();
            }
            finally
            {
                bs.Close();
                fs.Close();
            }
        }
    }
}
