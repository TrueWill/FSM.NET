namespace SampleWinFormsApp
{
    partial class MainForm
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
            this.comboAvailableEvents = new System.Windows.Forms.ComboBox();
            this.buttonAct = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textCurrentState = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboAvailableEvents
            // 
            this.comboAvailableEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAvailableEvents.FormattingEnabled = true;
            this.comboAvailableEvents.Location = new System.Drawing.Point(112, 35);
            this.comboAvailableEvents.Name = "comboAvailableEvents";
            this.comboAvailableEvents.Size = new System.Drawing.Size(187, 24);
            this.comboAvailableEvents.TabIndex = 3;
            // 
            // buttonAct
            // 
            this.buttonAct.Location = new System.Drawing.Point(134, 133);
            this.buttonAct.Name = "buttonAct";
            this.buttonAct.Size = new System.Drawing.Size(165, 39);
            this.buttonAct.TabIndex = 4;
            this.buttonAct.Text = "Act!";
            this.buttonAct.UseVisualStyleBackColor = true;
            this.buttonAct.Click += new System.EventHandler(this.buttonAct_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current state:";
            // 
            // textCurrentState
            // 
            this.textCurrentState.Location = new System.Drawing.Point(112, 6);
            this.textCurrentState.Name = "textCurrentState";
            this.textCurrentState.ReadOnly = true;
            this.textCurrentState.Size = new System.Drawing.Size(308, 22);
            this.textCurrentState.TabIndex = 1;
            this.textCurrentState.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select action:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 184);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textCurrentState);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAct);
            this.Controls.Add(this.comboAvailableEvents);
            this.Name = "MainForm";
            this.Text = "FSM Sample WinForms App";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboAvailableEvents;
        private System.Windows.Forms.Button buttonAct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textCurrentState;
        private System.Windows.Forms.Label label2;
    }
}

