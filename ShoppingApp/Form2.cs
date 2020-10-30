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
    public partial class FormRegister : Form
    {
        ShoppingDBEntities db = new ShoppingDBEntities();
        public FormRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullname = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string conpassword = txtConPassword.Text;
            string[] arr = new string[] { fullname, email, password, conpassword };
            if (Utilities.IsEmpty(arr, string.Empty))
            {
                Worker alrWorker = db.Workers.FirstOrDefault(x=>x.Email==email);
                if (alrWorker == null)
                {
                    if (password == conpassword)
                    {
                        int roleId = db.Roles.FirstOrDefault(x => x.Name == "User").ID;
                        lblError.Visible = false;
                        Worker worker = new Worker();
                        worker.Name = fullname;
                        worker.Email = email;
                        worker.Password = password.HashMe();
                        worker.RoleID = roleId;
                        db.Workers.Add(worker);
                        db.SaveChanges();
                        MessageBox.Show("Added Succesfully");
                    }
                    else
                    {
                        lblError.Text = "Password and Confrim Password doesn't match!";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "this email already exists";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Please fill them all";
                lblError.Visible = true;
            }
        }
    }
}
