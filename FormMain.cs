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
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text;

namespace SHA1GenUtility
{
    public partial class FormMain : Form
    {
        private Semaphore pool;

        public FormMain()
        {
            InitializeComponent();

            combHashType.SelectedIndex = 0;

            pool = new Semaphore(4, 4);
        }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            int hashType = combHashType.SelectedIndex;

            listBox1.Items.Clear();

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < files.Length; i++)
            {
                String fileName = files[i];

                if (!File.GetAttributes(fileName).HasFlag(FileAttributes.Directory))
                {
                    new Thread(() => GenerateHash(fileName, hashType, i == files.Length - 1)).Start();
                }
            }
        }
        private void UIThread(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        private void GenerateHash(string filename, int hashType, bool last)
        {
            pool.WaitOne();
            try
            {
                try
                {
                    UIThread(() =>
                    {
                        labelFile.Text = "Please wait...";
                        this.Cursor = Cursors.WaitCursor;
                    });

                    HashComputer hashComputer = new HashComputer();
                    String hash = hashComputer.compute(filename, hashType);

                    UIThread(() =>
                    {
                        labelFile.Text = "Drop files here";
                        listBox1.Items.Add(Path.GetFileName(filename) + " - " + hash);
                        this.Cursor = Cursors.Default;

                        if (last)
                            WinAPI.SetForegroundWindow(Handle);
                    });
                }
                catch (Exception)
                {
                    UIThread(() =>
                    {
                        String error = "Error generating the hash";
                        labelFile.Text = error;
                        this.Cursor = Cursors.Default;
                        WinAPI.SetForegroundWindow(Handle);
                        MessageBox.Show(this, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            }
            finally
            {
                pool.Release();
            }
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder itens = new StringBuilder();

            foreach (String item in listBox1.Items)
            {
                itens.AppendLine(item);
            }
            
            if (itens.Length > 0)
                Clipboard.SetText(itens.ToString());
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            listBox1.Focus();
        }
    }
}
