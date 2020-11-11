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
using InventoryManager.Winforms.ViewModels;

namespace InventoryManager.Winforms
{
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();
            ViewModel = new WorldViewModel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ViewModel.World = JsonConvert.DeserializeObject<World>(File.ReadAllText(openFileDialog.FileName));
                ViewModel.Filename = openFileDialog.FileName;
            }
        }
    }
}
