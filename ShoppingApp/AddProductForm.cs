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
    public partial class AddProductForm : Form
    {
        ShoppingDBEntities db = new ShoppingDBEntities();
        Product selectedProduct;

        public void FillCategoryCombo()
        {
            cmbCat.Items.AddRange(db.Catagories.Select(x => x.Name).ToArray());
        }
        public AddProductForm()
        {
            InitializeComponent();
        }
        public void FillDataGrid()
        {
            dtgProductList.DataSource = db.Products.Where(x=>!x.Status).Select(x => new
            {
                x.ID,
                CategoryName = x.Catagory.Name,
                ProductName = x.Name,
                x.Price,
                x.Quantity
            }).ToList();
            dtgProductList.Columns[0].Visible = false;
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FillCategoryCombo();
        }

        public void btnVisible(string tx)
        {
            if (tx == "add")
            {
                btnAddProduct.Visible = true;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnAddProduct.Visible = false;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
            }
        }
        private void dtgProductList_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnVisible("ed");
            int proId = (int)dtgProductList.Rows[e.RowIndex].Cells[0].Value;
            selectedProduct = db.Products.Find(proId);
            cmbCat.Text = selectedProduct.Catagory.Name;
            txtProducts.Text = selectedProduct.Name;
            nmPrice.Value = selectedProduct.Price;
            nmCount.Value = selectedProduct.Quantity;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string proname = txtProducts.Text;
            string catname = cmbCat.Text;
            decimal price = nmPrice.Value;
            decimal count = nmCount.Value;
            if (Utilities.IsEmpty(new string[] { proname,catname},string.Empty))
            {
                int catId = db.Catagories.First(x => x.Name == catname).ID;
                Product pro = new Product();
                pro.Name = proname;
                pro.CategoryId = catId;
                pro.Price = price;
                pro.Quantity = (int)count;
                db.Products.Add(pro);
                db.SaveChanges();
                MessageBox.Show("Added Succesfully");
                FillDataGrid();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int catId = db.Catagories.First(x => x.Name == cmbCat.Text).ID;
            selectedProduct.Name = txtProducts.Text;
            selectedProduct.Price = nmPrice.Value;
            selectedProduct.Quantity = (int)nmCount.Value;
            selectedProduct.CategoryId = catId;
            db.SaveChanges();
            FillDataGrid();
            btnVisible("add");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            selectedProduct.Status = true;
            db.SaveChanges();
            FillDataGrid();
            btnVisible("add");
        }
    }
}
