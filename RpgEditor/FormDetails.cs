using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RpgLibrary.DataClasses;
using RpgLibrary.ItemClasses;

namespace RpgEditor
{
    public partial class FormDetails : Form
    {
        public static ItemDataManager? ItemDataManager { get; private set; }
        public FormDetails()
        {
            InitializeComponent();

            FormDetails.ItemDataManager ??= new();

            this.FormClosing += new(FormDetails_FormClosing);
        }

        void FormDetails_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                e.Cancel = false;
                this.Close();
            }
        }

        public static void WriteItemData()
        {
            foreach (string s in ItemDataManager.ArmorData.Keys)
            {
                XnaSerializer.Serialize<ArmorData>(
                FormMain.ItemPath + @"\Armor\" + s + ".xml",
                ItemDataManager.ArmorData[s]);
            }

            foreach (string s in ItemDataManager.WeaponData.Keys)
            {
                XnaSerializer.Serialize<WeaponData>(
                FormMain.ItemPath + @"\Weapon\" + s + ".xml",

                ItemDataManager.WeaponData[s]);
            }

            //TODO do the same for consumables
        }

        public static void ReadItemData()
        {
            ItemDataManager = new ItemDataManager();

            string[] fileNames = Directory.GetFiles(
            Path.Combine(FormMain.ItemPath, "Armor"),
            "*.xml");
            foreach (string s in fileNames)
            {
                ArmorData armorData = XnaSerializer.Deserialize<ArmorData>(s);
                ItemDataManager.ArmorData.Add(armorData.Name, armorData);


                System.Diagnostics.Debug.WriteLine("Armour data " + armorData.ToString());
                System.Diagnostics.Debug.WriteLine("Armour count1 " + FormDetails.ItemDataManager.ArmorData.Count);
            }

            fileNames = Directory.GetFiles(
            Path.Combine(FormMain.ItemPath, "Weapon"),
            "*.xml");
            foreach (string s in fileNames)
            {
                WeaponData weaponData = XnaSerializer.Deserialize<WeaponData>(s);
                ItemDataManager.WeaponData.Add(weaponData.Name, weaponData);
            }


            //TODO do the same for consumables
        }





    }
}
