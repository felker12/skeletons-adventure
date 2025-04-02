using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RpgLibrary.ItemClasses;

namespace RpgEditor
{
    public partial class FormArmorDetails : Form
    {
        public ArmorData? Armor { get; set; } = null;
        public FormArmorDetails()
        {
            InitializeComponent();

            this.Load += FormArmorDetails_Load;
            this.FormClosing += new FormClosingEventHandler(FormArmorDetails_FormClosing);
            btnOK.Click += new EventHandler(BtnOK_Click);
            btnCancel.Click += new EventHandler(BtnCancel_Click);
        }

        void FormArmorDetails_Load(object? sender, EventArgs e)
        {
            foreach (ArmorLocation location in Enum.GetValues(typeof(ArmorLocation)))
                cboArmorLocation.Items.Add(location);
            cboArmorLocation.SelectedIndex = 0;
            if (Armor != null)
            {
                tbName.Text = Armor.Name;
                tbType.Text = Armor.Type;
                mtbPrice.Text = Armor.Price.ToString();
                nudWeight.Value = (decimal)Armor.Weight;
                cboArmorLocation.SelectedIndex = (int)Armor.ArmorLocation;
                mtbDefenseValue.Text = Armor.DefenseValue.ToString();
            }
        }
        void FormArmorDetails_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }
        void BtnOK_Click(object? sender, EventArgs e)
        {
            float weight = 0f;
            if (string.IsNullOrEmpty(tbName.Text))
            {

                MessageBox.Show("You must enter a name for the item.");
                return;
            }
            if (!int.TryParse(mtbPrice.Text, out int price))
            {
                MessageBox.Show("Price must be an integer value.");
                return;
            }

            weight = (float)nudWeight.Value;

            if (!int.TryParse(mtbDefenseValue.Text, out int defVal))
            {
                MessageBox.Show("Defense valule must be an interger value.");
                return;
            }
            if (!int.TryParse(mtbDefenseModifier.Text, out int defMod))
            {
                MessageBox.Show("Defense valule must be an interger value.");
                return;
            }

            Armor = new()
            {
                Name = tbName.Text,
                Type = tbType.Text,
                Price = price,
                Weight = weight,
                ArmorLocation = (ArmorLocation)cboArmorLocation.SelectedIndex,
                DefenseValue = defVal
            };

            this.FormClosing -= FormArmorDetails_FormClosing;
            this.Close();
        }
        void BtnCancel_Click(object? sender, EventArgs e)
        {
            Armor = null;
            this.FormClosing -= FormArmorDetails_FormClosing;
            this.Close();
        }
    }
}
