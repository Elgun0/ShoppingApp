using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShoppingApp
{
    public partial class AdminDashboard : Form
    {
        private Worker activeAdmin;
        public AdminDashboard(Worker worker)
        {
            activeAdmin = worker;
            InitializeComponent();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProductForm admProduct = new AddProductForm();
            admProduct.Show();
        }
    }
}
