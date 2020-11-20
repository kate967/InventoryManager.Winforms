using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using InventoryManager.Data;
using Newtonsoft.Json;
using InventoryManager.Winforms.Controls;
using InventoryManager.Winforms.ViewModels;
using InventoryManager.Winforms.Forms;
using System.Reflection;

namespace InventoryManager.Winforms.Forms
{
    public partial class MainForm : Form
    {
        public static string AssemblyTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

        private WorldViewModel viewModel;

        private WorldViewModel ViewModel
        {
            get => viewModel;
            set
            {
                if(viewModel != value)
                {
                    viewModel = value;
                    worldViewModelBindingSource.DataSource = viewModel;
                }
            }
        }

        private bool isWorldLoaded;

        private bool IsWorldLoaded
        {
            get => isWorldLoaded;
            set
            {
                isWorldLoaded = value;
                mainTabControl.Enabled = isWorldLoaded;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            ViewModel = new WorldViewModel();
            IsWorldLoaded = false;

            mEquippedItemControlMap = new Dictionary<EquipLocations, EquippedItemControl>
            {
                {EquipLocations.LeftHand, leftHandEquippedItemControl },
                {EquipLocations.RightHand, rightHandEquippedItemControl},
                {EquipLocations.Head, headEquippedItemControl },
                {EquipLocations.Feet, feetEquippedItemControl }
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            using (AddPlayerForm addPlayerForm = new AddPlayerForm())
            {
                if(addPlayerForm.ShowDialog() == DialogResult.OK)
                {
                    Player player = new Player { Name = addPlayerForm.PlayerName };
                    ViewModel.Players.Add(player);
                }
            }
        }

        private void playersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deletePlayerButton.Enabled = playersListBox.SelectedItem != null;
            
            Player selectedPlayer = playersListBox.SelectedItem as Player;
            foreach (var control in mEquippedItemControlMap.Values)
            {
                control.Player = selectedPlayer;
            }
        }

        private void deletePlayerButton_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Delete this player?", AssemblyTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ViewModel.Players.Remove((Player)playersListBox.SelectedItem);
                playersListBox.SelectedItem = ViewModel.Players.FirstOrDefault();
            }
        }

        private void openWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ViewModel.World = JsonConvert.DeserializeObject<World>(File.ReadAllText(openFileDialog.FileName));
                ViewModel.Filename = openFileDialog.FileName;

                Player selectedPlayer = playersListBox.SelectedItem as Player;
                foreach (var control in mEquippedItemControlMap.Values)
                {
                    control.Player = selectedPlayer;
                }

                IsWorldLoaded = true;
            }
        }

        #region Main Menu
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewModel.SaveWorld();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ViewModel.Filename = saveFileDialog.FileName;
                ViewModel.SaveWorld();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        private readonly Dictionary<EquipLocations, EquippedItemControl> mEquippedItemControlMap;
    }
}
