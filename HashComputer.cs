/* SHA1GenUtility - Generate hashes for files 
 * Copyright (C) 2015-2016 Jeferson Boes
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

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
