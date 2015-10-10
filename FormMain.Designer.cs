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
 
namespace SHA1GenUtility
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelFile = new System.Windows.Forms.Label();
            this.combHashType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelFile
            // 
            this.labelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFile.Location = new System.Drawing.Point(1, 101);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(428, 13);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "Drop files where";
            this.labelFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // combHashType
            // 
            this.combHashType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combHashType.DisplayMember = "0";
            this.combHashType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combHashType.FormattingEnabled = true;
            this.combHashType.Items.AddRange(new object[] {
            "Generate SHA1",
            "Generate SHA256",
            "Generate MD5"});
            this.combHashType.Location = new System.Drawing.Point(12, 12);
            this.combHashType.Name = "combHashType";
            this.combHashType.Size = new System.Drawing.Size(405, 21);
            this.combHashType.TabIndex = 1;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 261);
            this.Controls.Add(this.combHashType);
            this.Controls.Add(this.labelFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "SHA1GenUtility";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.ComboBox combHashType;
    }
}

