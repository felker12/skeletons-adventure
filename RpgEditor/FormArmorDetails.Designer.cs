using System.Xml.Linq;

namespace RpgEditor
{
    partial class FormArmorDetails
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
            btnCancel = new Button();
            btnOK = new Button();
            nudWeight = new NumericUpDown();
            label4 = new Label();
            mtbPrice = new MaskedTextBox();
            label3 = new Label();
            tbType = new TextBox();
            label2 = new Label();
            tbName = new TextBox();
            label1 = new Label();
            label5 = new Label();
            cboArmorLocation = new ComboBox();
            label6 = new Label();
            mtbDefenseValue = new MaskedTextBox();
            label7 = new Label();
            mtbDefenseModifier = new MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)nudWeight).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(127, 238);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 30;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(15, 239);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 31;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // nudWeight
            // 
            nudWeight.DecimalPlaces = 2;
            nudWeight.Location = new Point(126, 95);
            nudWeight.Margin = new Padding(4, 3, 4, 3);
            nudWeight.Name = "nudWeight";
            nudWeight.Size = new Size(117, 23);
            nudWeight.TabIndex = 23;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(68, 97);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(48, 15);
            label4.TabIndex = 22;
            label4.Text = "Weight:";
            // 
            // mtbPrice
            // 
            mtbPrice.Location = new Point(126, 65);
            mtbPrice.Margin = new Padding(4, 3, 4, 3);
            mtbPrice.Mask = "000000";
            mtbPrice.Name = "mtbPrice";
            mtbPrice.Size = new Size(116, 23);
            mtbPrice.TabIndex = 21;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(79, 68);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 20;
            label3.Text = "Price:";
            // 
            // tbType
            // 
            tbType.Location = new Point(126, 35);
            tbType.Margin = new Padding(4, 3, 4, 3);
            tbType.Name = "tbType";
            tbType.Size = new Size(116, 23);
            tbType.TabIndex = 19;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(79, 38);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 18;
            label2.Text = "Type:";
            // 
            // tbName
            // 
            tbName.Location = new Point(126, 5);
            tbName.Margin = new Padding(4, 3, 4, 3);
            tbName.Name = "tbName";
            tbName.Size = new Size(116, 23);
            tbName.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(75, 8);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 16;
            label1.Text = "Name:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 128);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(93, 15);
            label5.TabIndex = 32;
            label5.Text = "Armor Location:";
            // 
            // cboArmorLocation
            // 
            cboArmorLocation.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArmorLocation.FormattingEnabled = true;
            cboArmorLocation.Location = new Point(127, 125);
            cboArmorLocation.Margin = new Padding(4, 3, 4, 3);
            cboArmorLocation.Name = "cboArmorLocation";
            cboArmorLocation.Size = new Size(115, 23);
            cboArmorLocation.TabIndex = 33;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 159);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 34;
            label6.Text = "Defense Value:";
            // 
            // mtbDefenseValue
            // 
            mtbDefenseValue.Location = new Point(127, 156);
            mtbDefenseValue.Margin = new Padding(4, 3, 4, 3);
            mtbDefenseValue.Mask = "000";
            mtbDefenseValue.Name = "mtbDefenseValue";
            mtbDefenseValue.Size = new Size(116, 23);
            mtbDefenseValue.TabIndex = 35;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 189);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(100, 15);
            label7.TabIndex = 34;
            label7.Text = "Defense Modifier:";
            // 
            // mtbDefenseModifier
            // 
            mtbDefenseModifier.Location = new Point(127, 186);
            mtbDefenseModifier.Margin = new Padding(4, 3, 4, 3);
            mtbDefenseModifier.Mask = "000";
            mtbDefenseModifier.Name = "mtbDefenseModifier";
            mtbDefenseModifier.Size = new Size(116, 23);
            mtbDefenseModifier.TabIndex = 35;
            // 
            // FormArmorDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(418, 439);
            Controls.Add(mtbDefenseModifier);
            Controls.Add(label7);
            Controls.Add(mtbDefenseValue);
            Controls.Add(label6);
            Controls.Add(cboArmorLocation);
            Controls.Add(label5);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(nudWeight);
            Controls.Add(label4);
            Controls.Add(mtbPrice);
            Controls.Add(label3);
            Controls.Add(tbType);
            Controls.Add(label2);
            Controls.Add(tbName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FormArmorDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Armor Details";
            ((System.ComponentModel.ISupportInitialize)nudWeight).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown nudWeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox mtbPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboArmorLocation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox mtbDefenseValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox mtbDefenseModifier;
    }
}