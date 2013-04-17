namespace FikrPos.Modules.Dashboard
{
    partial class SessionProperty
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNnotes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numStartOfCash = new System.Windows.Forms.NumericUpDown();
            this.numEndOfCash = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numStartOfCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndOfCash)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start of Cash";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "End of Cash";
            // 
            // txtNnotes
            // 
            this.txtNnotes.Location = new System.Drawing.Point(86, 63);
            this.txtNnotes.Multiline = true;
            this.txtNnotes.Name = "txtNnotes";
            this.txtNnotes.Size = new System.Drawing.Size(247, 84);
            this.txtNnotes.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Notes";
            // 
            // numStartOfCash
            // 
            this.numStartOfCash.Location = new System.Drawing.Point(86, 11);
            this.numStartOfCash.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numStartOfCash.Name = "numStartOfCash";
            this.numStartOfCash.Size = new System.Drawing.Size(120, 20);
            this.numStartOfCash.TabIndex = 0;
            // 
            // numEndOfCash
            // 
            this.numEndOfCash.Location = new System.Drawing.Point(86, 37);
            this.numEndOfCash.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numEndOfCash.Name = "numEndOfCash";
            this.numEndOfCash.Size = new System.Drawing.Size(120, 20);
            this.numEndOfCash.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(177, 151);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 151);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SessionProperty
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(345, 186);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.numEndOfCash);
            this.Controls.Add(this.numStartOfCash);
            this.Controls.Add(this.txtNnotes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SessionProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Session Property";
            ((System.ComponentModel.ISupportInitialize)(this.numStartOfCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndOfCash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNnotes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numStartOfCash;
        private System.Windows.Forms.NumericUpDown numEndOfCash;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}