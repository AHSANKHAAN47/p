using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
public class Customer
{
    private static string _connectionString = ConfigurationManager.ConnectionStrings["AppraisalMS"].ConnectionString;
    public void createInvoice(string FirstName, string LastName, string PhoneNo, string NIC, List<Product> products,string TotatAmount,string AmountPaid,string AmountDue)
    {
        try
        {
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string insertQuery = "INSERT INTO tbl_Customer (First_Name, Last_Name, Phone_Number, NIC,Created_By,Created_On) " +
                                 "OUTPUT INSERTED.id " +
                                 "VALUES (@Firstname, @Lastname, @PhoneNumber, @NIC,@CreatedBy,@CreatedOn)";

                SqlCommand insertCommand = new SqlCommand(insertQuery, connect);

                insertCommand.Parameters.Add(new SqlParameter("@Firstname", FirstName));
                insertCommand.Parameters.Add(new SqlParameter("@Lastname", LastName));
                insertCommand.Parameters.Add(new SqlParameter("@PhoneNumber", PhoneNo));
                insertCommand.Parameters.Add(new SqlParameter("@NIC", NIC));
                insertCommand.Parameters.Add(new SqlParameter("@CreatedBy", "admin"));
                insertCommand.Parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));
                int insertedcustomerId = (int)insertCommand.ExecuteScalar();

                foreach (Product product in products)
                {
                    string insertProductQuery = "INSERT INTO tbl_Customer_Details (Customer_ID, Product_Name, Product_Quantity, Product_Price,Created_By,Created_On) " +
                                 "VALUES (@Customer_ID, @ProductName, @Quantity, @Price,@CreatedBy,@CreatedOn)";

                    SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connect);

                    insertProductCommand.Parameters.Add(new SqlParameter("@Customer_ID", insertedcustomerId));
                    insertProductCommand.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
                    insertProductCommand.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
                    insertProductCommand.Parameters.Add(new SqlParameter("@Price", product.ProductPrice));
                    insertProductCommand.Parameters.Add(new SqlParameter("@CreatedBy", "admin"));
                    insertProductCommand.Parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));
                    insertProductCommand.ExecuteNonQuery();
                }
                string insertTransactionQuery = "INSERT INTO tbl_Customer_Transaction (Customer_ID, Total_Amount, Amount_Paid,Amount_Due,Created_By,Created_On) " +
                                 "VALUES (@CustomerID, @TotalAmount, @AmountPaid,@AmountDue,@CreatedBy,@CreatedOn)";

                SqlCommand TransactionCommand = new SqlCommand(insertTransactionQuery, connect);

                TransactionCommand.Parameters.Add(new SqlParameter("@CustomerID", insertedcustomerId));
                TransactionCommand.Parameters.Add(new SqlParameter("@TotalAmount", TotatAmount));
                TransactionCommand.Parameters.Add(new SqlParameter("@AmountPaid", AmountPaid));
                TransactionCommand.Parameters.Add(new SqlParameter("@AmountDue", AmountDue));
                TransactionCommand.Parameters.Add(new SqlParameter("@CreatedBy", "admin"));
                TransactionCommand.Parameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));
                TransactionCommand.ExecuteNonQuery();
            }
        }
        catch
        {
            throw;
        }
    }
}