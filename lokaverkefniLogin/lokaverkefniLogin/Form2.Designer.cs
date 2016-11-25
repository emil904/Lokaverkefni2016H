namespace lokaverkefniLogin
{
    partial class Form2
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
            this.txtUsers = new System.Windows.Forms.TextBox();
            this.txtChatbox = new System.Windows.Forms.TextBox();
            this.txtMessagebox = new System.Windows.Forms.TextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUsers
            // 
            this.txtUsers.Location = new System.Drawing.Point(13, 13);
            this.txtUsers.Multiline = true;
            this.txtUsers.Name = "txtUsers";
            this.txtUsers.Size = new System.Drawing.Size(133, 408);
            this.txtUsers.TabIndex = 0;
            // 
            // txtChatbox
            // 
            this.txtChatbox.Location = new System.Drawing.Point(153, 216);
            this.txtChatbox.Multiline = true;
            this.txtChatbox.Name = "txtChatbox";
            this.txtChatbox.Size = new System.Drawing.Size(429, 178);
            this.txtChatbox.TabIndex = 1;
            // 
            // txtMessagebox
            // 
            this.txtMessagebox.Location = new System.Drawing.Point(153, 400);
            this.txtMessagebox.Name = "txtMessagebox";
            this.txtMessagebox.Size = new System.Drawing.Size(367, 20);
            this.txtMessagebox.TabIndex = 2;
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(526, 400);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(56, 23);
            this.btSend.TabIndex = 3;
            this.btSend.Text = "Send";
            this.btSend.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(594, 433);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.txtMessagebox);
            this.Controls.Add(this.txtChatbox);
            this.Controls.Add(this.txtUsers);
            this.Name = "Form2";
            this.Text = "Lokaverkefni -Main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsers;
        private System.Windows.Forms.TextBox txtChatbox;
        private System.Windows.Forms.TextBox txtMessagebox;
        private System.Windows.Forms.Button btSend;
    }
}