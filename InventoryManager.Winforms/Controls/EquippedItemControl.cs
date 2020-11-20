using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryManager.Data;

namespace InventoryManager.Winforms.Controls
{
    public partial class EquippedItemControl : UserControl
    {
        private EquipLocations mEquipLocation;

        public EquipLocations EquipLocation 
        {
            get => mEquipLocation; 
            set
            {
                mEquipLocation = value;
                equipLocationTextBox.Text = mEquipLocation.ToString();
            }
        }

        public Item EquippedItem 
        { 
            get => (Item)equippedItemComboBox.SelectedItem; 
            set => equippedItemComboBox.SelectedItem = value; 
        }

        private static readonly Item NoItem = new Item() { Name = "None" };

        private Player mPlayer;

        public Player Player
        {
            get => mPlayer;
            set
            {
                if(mPlayer != value)
                {
                    mPlayer = value;
                    if(mPlayer != null)
                    {
                        var inventory = new List<Item>(mPlayer.Inventory);
                        inventory.Insert(0, NoItem);

                        equippedItemComboBox.SelectedIndexChanged -= equippedItemComboBox_SelectedIndexChanged;
                        equippedItemComboBox.DataSource = inventory;

                        EquippedItem = mPlayer.EquippedItems.TryGetValue(EquipLocation, out Item equippedItem) ? equippedItem : NoItem;

                        equippedItemComboBox.SelectedIndexChanged += equippedItemComboBox_SelectedIndexChanged;
                    }
                    else
                    {
                        equippedItemComboBox.DataSource = null;
                    }
                }    
            }
        }

        public EquippedItemControl()
        {
            InitializeComponent();
        }

        private void equipLocationTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void equippedItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(mPlayer != null)
            {
                Item equippedItem = EquippedItem;
                if(equippedItem == NoItem)
                {
                    mPlayer.EquippedItems.Remove(EquipLocation);
                }
                else
                {
                    mPlayer.EquippedItems[EquipLocation] = equippedItem;
                }
            }
        }
    }
}
