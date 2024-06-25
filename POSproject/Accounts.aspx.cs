using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Accounts : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            divpopup.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "showModal();", true);
            Session["Products"] = null;
        }

    }
    protected void btnNewCustomer(object sender, EventArgs e)
    {
        divpopup.Visible = false;
        pnlNewCustomer.Visible = true;
    }
    protected void btnExistingCustomer(object sender, EventArgs e)
    {
        divpopup.Visible = false;
    }
    protected void btnViewPayment(object sender, EventArgs e)
    {
        divpopup.Visible = false;
    }
    protected void btnSubmit(object sender, EventArgs e)
    {
        string errorMessage = ValidateCustomer();
        if (string.IsNullOrEmpty(errorMessage))
        {
            // Assuming 'products' is correctly defined and accessible in this context
            List<Product> products = Session["Products"] as List<Product> ?? new List<Product>();
            string totalAmount = lblTotalAmount.Text.Replace("Total Amount: PKR ", "");
            Customer customer = new Customer();
            customer.createInvoice(txtFirstName.Text, txtLastName.Text, txtPhoneNumber.Text, txtNIC.Text, products, totalAmount, txtAmountPaid.Text, (Convert.ToInt32(totalAmount) - Convert.ToInt32(txtAmountPaid.Text)).ToString());
            // Refresh the page at the end of the operation
            PDFHelper.GenerateInvoice(Response);
            Response.Redirect(Request.RawUrl);
        }
        else
        {
            SendAlertClientSide(errorMessage);
        }
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProduct.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            return;
        }
        List<Product> products = Session["Products"] as List<Product> ?? new List<Product>();
        Product product = new Product();
        product.ProductName = txtProduct.Text;
        product.Quantity = Convert.ToInt32(txtQuantity.Text);
        product.ProductPrice = Convert.ToDecimal(txtPrice.Text);
        products.Add(product); // Add a new product (adjust according to how you're capturing product details)
        Session["Products"] = products; // Update the Session
        txtProduct.Text = "";
        txtQuantity.Text = "";
        txtPrice.Text = "";
        gvProducts.DataSource = products;
        gvProducts.DataBind();
        lblTotalAmount.Text = "Total Amount: PKR ";
        decimal totalAmount = 0;
        foreach (Product p in products)
        {
            totalAmount = totalAmount + Convert.ToDecimal(p.ProductPrice) * Convert.ToDecimal(p.Quantity);
        }
        lblTotalAmount.Text = lblTotalAmount.Text + totalAmount.ToString();
    }
    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // Convert the CommandArgument to the index of the row to be removed
            int index = Convert.ToInt32(e.CommandArgument);

            // Assuming you have your product list stored in a session or view state
            List<Product> products = (List<Product>)Session["Products"];

            // Remove the product at the specified index
            if (index >= 0 && index < products.Count)
            {
                products.RemoveAt(index);
            }

            // Update the session or view state
            Session["Products"] = products;

            // Rebind the GridView to reflect changes
            gvProducts.DataSource = products;
            gvProducts.DataBind();
            lblTotalAmount.Text = "Total Ammount: PKR ";
            decimal totalAmount = 0;
            foreach (Product p in products)
            {
                totalAmount = totalAmount + Convert.ToDecimal(p.ProductPrice) * Convert.ToDecimal(p.Quantity);
            }
            lblTotalAmount.Text = lblTotalAmount.Text + totalAmount.ToString();
        }
    }
    public void SendAlertClientSide(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUserScript", "alertUserScript('" + message + "');", true);
    }
    public string ValidateCustomer()
    {
        string errorMessage = "";
        List<Product> products = Session["Products"] as List<Product> ?? new List<Product>();
        if (products.Count == 0)
        {
            errorMessage="Please add products to the cart.\n";
        }
        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            errorMessage += "Please enter customer name.\n";
        }
        if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
        {
            errorMessage += "Please enter customer phone number.\n";
        }
        if (string.IsNullOrWhiteSpace(txtNIC.Text))
        {
            errorMessage += "Please enter customer National Identity Card Number.\n";
        }
        if (string.IsNullOrWhiteSpace(txtAmountPaid.Text))
        {
            errorMessage += "Please enter amount paid by customer.\n";
        }
        int paidAmount;
        int totalAmount;
        bool isPaidAmountValid = int.TryParse(txtAmountPaid.Text, out paidAmount);
        bool isTotalAmountValid = int.TryParse(lblTotalAmount.Text.Replace("Total Amount: PKR ", ""), out totalAmount);

        if (!isPaidAmountValid || !isTotalAmountValid)
        {
            errorMessage += "Invalid amount format.\n";
        }
        else if (paidAmount > totalAmount)
        {
            errorMessage += "Amount paid by customer is greater than total amount.\n";
        }
        return errorMessage;
    }
}