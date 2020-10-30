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
    public partial class Dashboard : Form
    {
        ShoppingDBEntities db = new ShoppingDBEntities();
        private Worker actWork;
        Product selectedProduct;
        public Dashboard(Worker worker)
        {
            actWork = worker;
            InitializeComponent();
        }

        public void FillCategoryCombo()
        {
            cmbCategory.Items.AddRange(db.Catagories.Select(c=>c.Name).ToArray());
            cmbCategoryFilter.Items.AddRange(db.Catagories.Select(c => c.Name).ToArray());
        }
        public void FillOrderDataGrid()
        {
            dtgOrders.DataSource = db.Orders.Where(ord => ord.WorkerID == actWork.ID &&
            ord.Product.Catagory.Name.StartsWith(cmbCategoryFilter.Text) &&
            ord.Product.Price>=nmMinPrice.Value &&
            ord.Product.Price<=nmMaxPrice.Value).Select(x => new {
                CategoryName = x.Product.Catagory.Name,
                ProductName = x.Product.Name,
                x.Product.Price,
                x.Count,
                x.PurchaseDate
            }).ToList();
        }
        decimal maxPrice;
        decimal minPrice;
        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblUser.Text = actWork.Name;
            FillCategoryCombo();
            maxPrice = db.Orders.Where(x => x.WorkerID == actWork.ID).Select(x => x.Product).Max(x => x.Price);
            minPrice = db.Orders.Where(x => x.WorkerID == actWork.ID).Select(x => x.Product).Min(x => x.Price);
            nmMinPrice.Minimum = minPrice;
            nmMinPrice.Maximum = maxPrice;
            nmMaxPrice.Minimum = minPrice;
            nmMaxPrice.Maximum = maxPrice;
            nmMaxPrice.Value = maxPrice;
            FillOrderDataGrid();
        }

        public void IsBuy(string txt)
        {
            if (txt == "buy")
            {
                btnBuy.Enabled = true;
                lblPrice.Visible = true;
                cmbCategory.Text = "";
                lblQuantity.Visible = false;
            }
            else
            {
                btnBuy.Enabled = false;
                lblPrice.Visible = false;
                nmCount.Value = 1;
            }
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cat = cmbCategory.Text;
            int catId = db.Catagories.First(x => x.Name == cat).ID;
            IsBuy("b");
            cmbProducts.Items.Clear();
            cmbProducts.Items.AddRange(db.Products.Where(x => x.CategoryId == catId && !x.Status).Select(x => x.Name).ToArray());
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string proText = cmbProducts.Text;
            if (proText != "")
            {
                selectedProduct = db.Products.First(x => x.Name == proText);
                lblPrice.Text = selectedProduct.Price+" Azn";
                nmCount.Maximum = selectedProduct.Quantity;
                IsBuy("buy");
                if (selectedProduct.Quantity <= 0)
                {
                    lblQuantity.Text = "Product have not in stock";
                    lblQuantity.Visible = true;
                    lblQuantity.ForeColor = Color.Red;
                    btnBuy.Enabled = false;
                    nmCount.Visible = false;
                    lblPrice.Visible = false;
                }
                else
                {
                    lblQuantity.Text = selectedProduct.Quantity + " avaible";
                    lblQuantity.Visible = true;
                    lblQuantity.ForeColor = Color.Green;
                    btnBuy.Enabled = true;
                    nmCount.Visible = true;
                    lblPrice.Visible = true;
                }

                
            }
        }

        private void nmCount_ValueChanged(object sender, EventArgs e)
        {
            lblPrice.Text = (selectedProduct.Price * nmCount.Value) + " Azn";
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            ord.WorkerID = actWork.ID;
            ord.ProductID = selectedProduct.ID;
            ord.PurchaseDate = DateTime.Now;
            ord.Count = (int)nmCount.Value;
            db.Orders.Add(ord);
            selectedProduct.Quantity -= (int)nmCount.Value;
            db.SaveChanges();
            FillOrderDataGrid();
            MessageBox.Show($"Product: {selectedProduct.Name} successfully purchased");
            IsBuy("b");
            cmbProducts.Items.Clear();
        }

        private void cmbCategoryFilter_KeyUp(object sender, KeyEventArgs e)
        {
            FillOrderDataGrid();
        }

        private void cmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillOrderDataGrid();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            decimal minPriceIn = nmMinPrice.Value;
            decimal maxPriceIn = nmMaxPrice.Value;
            if (minPrice > maxPrice) 
            {
                nmMaxPrice.Value = maxPrice;
            }
            FillOrderDataGrid();
        }
    }
}
