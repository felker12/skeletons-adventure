using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using RpgLibrary.EntityClasses;

namespace RpgEditor
{
    public partial class FormEntityDetails : Form
    {
        public EntityData? Entity { get; set; }

        public FormEntityDetails()
        {
            InitializeComponent();
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbType.Text))
            {
                MessageBox.Show("You must enter a type for the entity.");
                return;
            }

            if (!int.TryParse(tbId.Text, out int id))
            {
                MessageBox.Show("ID must be an integer.");
                return;
            }

            if (!int.TryParse(tbBaseHealth.Text, out int baseHealth))
            {
                MessageBox.Show("Base Health must be an integer.");
                return;
            }

            if (!int.TryParse(tbBaseDefence.Text, out int baseDefence))
            {
                MessageBox.Show("Base Defence must be an integer.");
                return;
            }

            if (!int.TryParse(tbBaseAttack.Text, out int baseAttack))
            {
                MessageBox.Show("Base Attack must be an integer.");
                return;
            }

            if (!int.TryParse(tbEntityLevel.Text, out int entityLevel))
            {
                MessageBox.Show("Entity Level must be an integer.");
                return;
            }

            if (!int.TryParse(tbBaseXP.Text, out int baseXP))
            {
                MessageBox.Show("Base XP must be an integer.");
                return;
            }

            if (!int.TryParse(tbCurrentHealth.Text, out int currentHealth))
            {
                MessageBox.Show("Current Health must be an integer.");
                return;
            }

            float posX = 0, posY = 0, respawnX = 0, respawnY = 0;
            Vector2? position = null, respawnPosition = null;
            if (!string.IsNullOrWhiteSpace(tbPositionX.Text) && !string.IsNullOrWhiteSpace(tbPositionY.Text))
            {
                if (float.TryParse(tbPositionX.Text, out posX) && float.TryParse(tbPositionY.Text, out posY))
                    position = new Vector2(posX, posY);
                else
                {
                    MessageBox.Show("Position X and Y must be valid floats.");
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(tbRespawnX.Text) && !string.IsNullOrWhiteSpace(tbRespawnY.Text))
            {
                if (float.TryParse(tbRespawnX.Text, out respawnX) && float.TryParse(tbRespawnY.Text, out respawnY))
                    respawnPosition = new Vector2(respawnX, respawnY);
                else
                {
                    MessageBox.Show("Respawn X and Y must be valid floats.");
                    return;
                }
            }

            Entity = new EntityData
            {
                id = id,
                type = tbType.Text,
                baseHealth = baseHealth,
                baseDefence = baseDefence,
                baseAttack = baseAttack,
                ntityLevel = entityLevel,
                baseXP = baseXP,
                position = position,
                respawnPosition = respawnPosition,
                currentHealth = currentHealth,
                isDead = cbIsDead.Checked,
                lastDeathTime = TimeSpan.Zero, // You can add a field for this if needed
                Items = new System.Collections.Generic.List<RpgLibrary.ItemClasses.ItemData>()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Entity = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void LoadEntity(EntityData entity)
        {
            tbType.Text = entity.type;
            tbId.Text = entity.id.ToString();
            tbBaseHealth.Text = entity.baseHealth.ToString();
            tbBaseDefence.Text = entity.baseDefence.ToString();
            tbBaseAttack.Text = entity.baseAttack.ToString();
            tbEntityLevel.Text = entity.ntityLevel.ToString();
            tbBaseXP.Text = entity.baseXP.ToString();
            cbIsDead.Checked = entity.isDead;
            tbCurrentHealth.Text = entity.currentHealth.ToString();
            if (entity.position.HasValue)
            {
                tbPositionX.Text = entity.position.Value.X.ToString();
                tbPositionY.Text = entity.position.Value.Y.ToString();
            }
            if (entity.respawnPosition.HasValue)
            {
                tbRespawnX.Text = entity.respawnPosition.Value.X.ToString();
                tbRespawnY.Text = entity.respawnPosition.Value.Y.ToString();
            }
        }

        private void tbBaseHealth_TextChanged(object sender, EventArgs e)
        {
            // Default current health to base health
            tbCurrentHealth.Text = tbBaseHealth.Text; 
        }
    }
}
