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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
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
        private int hashType = 0;

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            hashType = combHashType.SelectedIndex;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                filename = file;
            }

            if (File.GetAttributes(filename).HasFlag(FileAttributes.Directory))
            {
                labelFile.Text = "Drop files where";
            }
            else { 
                Thread thread = new Thread(GenerateHash);
                thread.Start();
            }
        }

        delegate void SetTextCallback(string text);
        delegate void SetCursorCallback(Cursor cursor);

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

        private void SetCursor(Cursor cursor)
        {
            if (InvokeRequired)
            {
                SetCursorCallback d = new SetCursorCallback(SetCursor);
                Invoke(d, new object[] { cursor });
            }
            else
            {
                this.Cursor = cursor;
            }
        }
        
        private void GenerateHash()
        {
            try
            {
                SetText("Please wait...");
                SetCursor(Cursors.WaitCursor);

                HashComputer hashComputer = new HashComputer();
                String hash = hashComputer.compute(filename, hashType);

                SetText(hash);
                SetCursor(Cursors.Default);
            }
            catch (Exception)
            {
                SetError("Error generating the hash");
                SetCursor(Cursors.Default);
            }
        }
    }
}
