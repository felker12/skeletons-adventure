using Microsoft.Xna.Framework.Input;
using RpgLibrary.DataClasses;
using RpgLibrary.GameObjectClasses;
using RpgLibrary.ItemClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RpgEditor
{
    public partial class FormMain : Form
    {
        RPG RPG { get; set; } = new();
        FormArmor? frmArmor;
        FormWeapon? frmWeapon;

        public static string ItemPath { get; set; } = string.Empty;
        public static string GamePath { get; set; } = string.Empty;

        public FormMain()
        {
            InitializeComponent();

            this.FormClosing += FormMain_FormClosing;
        }

        void FormMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Unsaved changes will be lost. Are you sure you want to exit?",
            "Exit?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void NewGameToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using (FormNewGame frmNewGame = new())
            {
                DialogResult result = frmNewGame.ShowDialog();
                if (result == DialogResult.OK && frmNewGame.RPG != null)
                {
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                    folderDialog.Description = "Select folder to create game in.";
                    folderDialog.SelectedPath = Application.StartupPath;
                    DialogResult folderResult = folderDialog.ShowDialog();
                    if (folderResult == DialogResult.OK)
                    {
                        try
                        {
                            GamePath = Path.Combine(folderDialog.SelectedPath, "Game");
                            ItemPath = Path.Combine(GamePath, "Items");

                            if (Directory.Exists(GamePath))
                                throw new Exception("Selected directory already exists.");

                            Directory.CreateDirectory(GamePath);
                            Directory.CreateDirectory(ItemPath + @"\Armor");
                            Directory.CreateDirectory(ItemPath + @"\Weapon");
                            RPG = frmNewGame.RPG;
                            XnaSerializer.Serialize<RPG>(GamePath + @"\Game.xml", RPG);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            return;
                        }
                        itemsToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }

        private void OpenGameToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select Game folder";
            folderDialog.SelectedPath = Application.StartupPath;
            bool tryAgain = false;
            do
            {
                DialogResult folderResult = folderDialog.ShowDialog();
                DialogResult msgBoxResult;
                if (folderResult == DialogResult.OK)
                {
                    if (File.Exists(folderDialog.SelectedPath + @"\Game\Game.xml"))
                    {
                        try
                        {
                            OpenGame(folderDialog.SelectedPath);
                            tryAgain = false;
                        }
                        catch (Exception ex)
                        {
                            msgBoxResult = MessageBox.Show(
                            ex.ToString(),
                            "Error opening game.",
                            MessageBoxButtons.RetryCancel);
                            if (msgBoxResult == DialogResult.Cancel)
                                tryAgain = false;
                            else if (msgBoxResult == DialogResult.Retry)
                                tryAgain = true;
                        }
                    }

                    else
                    {
                        msgBoxResult = MessageBox.Show(
                        "Game not found, try again?",
                        "Game does not exist",
                        MessageBoxButtons.RetryCancel);
                        if (msgBoxResult == DialogResult.Cancel)
                            tryAgain = false;
                        else if (msgBoxResult == DialogResult.Retry)
                            tryAgain = true;
                    }
                }
            } while (tryAgain);
        }

        private void SaveGameToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (RPG != null)
            {
                try
                {
                    XnaSerializer.Serialize<RPG>(GamePath + @"\Game.xml", RPG);
                    FormDetails.WriteItemData();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString(), "Error saving game.");
                }
            }
        }

        private void OpenGame(string path)
        {
            GamePath = Path.Combine(path, "Game");
            ItemPath = Path.Combine(GamePath, "Items");

            RPG = XnaSerializer.Deserialize<RPG>(GamePath + @"\Game.xml");

            FormDetails.ReadItemData();

            PrepareForms();
        }

        private void PrepareForms()
        {
            frmArmor ??= new()
            {
                MdiParent = this
            };
            frmArmor.FillListBox();

            frmWeapon ??= new()
            {
                MdiParent = this
            };
            frmWeapon.FillListBox();

            itemsToolStripMenuItem.Enabled = true;
        }

        private void ExitGameToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void WeaponsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            frmWeapon ??= new()
            {
                MdiParent = this
            };
            frmWeapon.Show();
            frmWeapon.BringToFront();
        }

        private void ArmorToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            frmArmor ??= new()
            {
                MdiParent = this
            };
            frmArmor.Show();
            frmArmor.BringToFront();
        }

    }
}
