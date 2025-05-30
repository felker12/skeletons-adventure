using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RpgLibrary.EntityClasses;

namespace RpgEditor
{
    public partial class FormEntity : FormDetails
    {
        public FormEntity()
        {
            InitializeComponent();

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (FormEntityDetails frmEntityDetails = new())
            {
                if (frmEntityDetails.ShowDialog() == DialogResult.OK && frmEntityDetails.Entity != null)
                {
                    AddEntity(frmEntityDetails.Entity);
                }
            }
        }

        void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (lbDetails.SelectedItem is EntityData entity)
            {
                using (FormEntityDetails frmEntityDetails = new())
                {
                    // Optionally, you can implement a LoadEntity method for editing
                    frmEntityDetails.Entity = entity.Clone();
                    if (frmEntityDetails.ShowDialog() == DialogResult.OK && frmEntityDetails.Entity != null)
                    {
                        int idx = lbDetails.SelectedIndex;
                        lbDetails.Items[idx] = frmEntityDetails.Entity;
                        // Update in your data manager if you add persistence
                    }
                }
            }
        }

        void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (lbDetails.SelectedItem is EntityData entity)
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete entity {entity.type} (ID: {entity.id})?",
                    "Delete",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    lbDetails.Items.RemoveAt(lbDetails.SelectedIndex);
                    // Remove from your data manager if you add persistence
                }
            }
        }

        public void FillListBox()
        {
            lbDetails.Items.Clear();
            // If you add a manager for entities, enumerate and add here
            // Example: foreach (var entity in EntityManager.Entities) lbDetails.Items.Add(entity);
        }

        private void AddEntity(EntityData entityData)
        {
            // You can use entityData.type or entityData.id as a key if you add a manager
            lbDetails.Items.Add(entityData);
        }
    }
}
