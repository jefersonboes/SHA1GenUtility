/* SHA1GenUtility - Generate hashes for files 
 * Copyright (C) 2015 Jeferson Boes
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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace SHA1GenUtility
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            combHashType.SelectedIndex = 0;
        }

        private string filename;

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragEnter");
            bool validData = GetFilename(e);
            if (validData)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                labelFile.Text = "Drop files where";
            }
        }

        protected bool GetFilename(DragEventArgs e)
        {
            filename = String.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        if (File.Exists(filename))
                            return true;
                    }
                }
            }

            return false;
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (labelFile.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                labelFile.Text = text;
                WinAPI.SetForegroundWindow(Handle);
            }
        }

        private void SetError(string error)
        {
            if (InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetError);
                Invoke(d, new object[] { error });
            }
            else
            {
                labelFile.Text = "Drop files where";
                WinAPI.SetForegroundWindow(Handle);
                MessageBox.Show(this, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
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

        private void GenerateHash()
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                BufferedStream bs = new BufferedStream(fs);
                try
                {
                    SetText("Wait");

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

                    //Convert.ToBase64String(hash))

                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    SetText(formatted.ToString());
                }
                finally
                {
                    bs.Close();
                    fs.Close();
                }
            }
            catch (Exception)
            {
                SetError("Error generating the hash");
            }
        }

        private int hashType = 0;

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            hashType = combHashType.SelectedIndex;
            
            Thread thread = new Thread(GenerateHash);

            thread.Start();
        }
    }
}
