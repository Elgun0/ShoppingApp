﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            FormRegister rgForm = new FormRegister();
            rgForm.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login logForm = new Login();
            logForm.ShowDialog();
        }
    }
}
