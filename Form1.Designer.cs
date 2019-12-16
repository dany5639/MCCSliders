using System;
using System.Diagnostics;

namespace MCCSliders
{
    partial class Form1
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
            this.textBox_X = new System.Windows.Forms.TextBox();
            this.trackBar_X = new System.Windows.Forms.TrackBar();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox_Y = new System.Windows.Forms.TextBox();
            this.trackBar_Y = new System.Windows.Forms.TrackBar();
            this.textBox_Z = new System.Windows.Forms.TextBox();
            this.trackBar_Z = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Z)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_X
            // 
            this.textBox_X.Location = new System.Drawing.Point(12, 41);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.Size = new System.Drawing.Size(71, 20);
            this.textBox_X.TabIndex = 3;
            // 
            // trackBar_X
            // 
            this.trackBar_X.LargeChange = 1;
            this.trackBar_X.Location = new System.Drawing.Point(93, 41);
            this.trackBar_X.Maximum = 200;
            this.trackBar_X.Minimum = -200;
            this.trackBar_X.Name = "trackBar_X";
            this.trackBar_X.Size = new System.Drawing.Size(762, 45);
            this.trackBar_X.TabIndex = 4;
            this.trackBar_X.Scroll += new System.EventHandler(this.trackBar_X_Scroll);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(12, 296);
            this.textBox6.MaxLength = 8;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(111, 20);
            this.textBox6.TabIndex = 11;
            this.textBox6.Text = "00007FF4082B879C";
            // 
            // textBox_Y
            // 
            this.textBox_Y.Location = new System.Drawing.Point(12, 92);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.Size = new System.Drawing.Size(71, 20);
            this.textBox_Y.TabIndex = 13;
            // 
            // trackBar_Y
            // 
            this.trackBar_Y.LargeChange = 1;
            this.trackBar_Y.Location = new System.Drawing.Point(93, 92);
            this.trackBar_Y.Maximum = 200;
            this.trackBar_Y.Minimum = -200;
            this.trackBar_Y.Name = "trackBar_Y";
            this.trackBar_Y.RightToLeftLayout = true;
            this.trackBar_Y.Size = new System.Drawing.Size(762, 45);
            this.trackBar_Y.TabIndex = 14;
            this.trackBar_Y.Scroll += new System.EventHandler(this.trackBar_Y_Scroll);
            // 
            // textBox_Z
            // 
            this.textBox_Z.Location = new System.Drawing.Point(12, 143);
            this.textBox_Z.Name = "textBox_Z";
            this.textBox_Z.Size = new System.Drawing.Size(71, 20);
            this.textBox_Z.TabIndex = 15;
            // 
            // trackBar_Z
            // 
            this.trackBar_Z.LargeChange = 1;
            this.trackBar_Z.Location = new System.Drawing.Point(93, 143);
            this.trackBar_Z.Maximum = 200;
            this.trackBar_Z.Minimum = -200;
            this.trackBar_Z.Name = "trackBar_Z";
            this.trackBar_Z.Size = new System.Drawing.Size(762, 45);
            this.trackBar_Z.TabIndex = 16;
            this.trackBar_Z.Scroll += new System.EventHandler(this.trackBar_Z_Scroll);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 322);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Reload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button_reset_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(839, 21);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 194);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(839, 96);
            this.richTextBox1.TabIndex = 19;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(776, 322);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button_writeToFile_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 322);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Zero";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button_zeroAll_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(695, 322);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 22;
            this.button4.Text = "Apply all";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button_applyToAll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 413);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_Z);
            this.Controls.Add(this.trackBar_Z);
            this.Controls.Add(this.textBox_Y);
            this.Controls.Add(this.trackBar_Y);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox_X);
            this.Controls.Add(this.trackBar_X);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Z)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar_X;
        private System.Windows.Forms.TextBox textBox_X;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox_Y;
        private System.Windows.Forms.TrackBar trackBar_Y;
        private System.Windows.Forms.TextBox textBox_Z;
        private System.Windows.Forms.TrackBar trackBar_Z;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

