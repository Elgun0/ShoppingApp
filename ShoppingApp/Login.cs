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
    public partial class Login : Form
    {
        ShoppingDBEntities db = new ShoppingDBEntities();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            if (Utilities.IsEmpty(new string[] { email,password },string.Empty))
            {
                lblError.Visible = false;
                Worker activeWorker = db.Workers.FirstOrDefault(x => x.Email == email);
                if (activeWorker != null)
                {
                    if (activeWorker.Password == password.HashMe())
                    {
                        if (ckRemember.Checked)
                        {
                            Properties.Settings.Default.email = email;
                            Properties.Settings.Default.password = password;
                            Properties.Settings.Default.isChecked = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.email = "";
                            Properties.Settings.Default.password = "";
                            Properties.Settings.Default.isChecked = false;
                            Properties.Settings.Default.Save();
                        }
                        if (activeWorker.Role.Name == "User")
                        {
                            this.Close();
                            Dashboard dash = new Dashboard(activeWorker);
                            dash.ShowDialog();
                        }
                        else if (activeWorker.Role.Name == "Admin")
                        {
                            this.Close();
                            AdminDashboard dash = new AdminDashboard(activeWorker);
                            dash.ShowDialog();
                        }
                    }
                    else
                    {
                        lblError.Text = "Password doesn't correct";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Email doesn't correct";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Email and password fill!";
                lblError.Visible = true;
                
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isChecked)
            {
                txtEmail.Text = Properties.Settings.Default.email;
                txtPassword.Text = Properties.Settings.Default.password;
                ckRemember.Checked = true;
            }
        }
    }
}
