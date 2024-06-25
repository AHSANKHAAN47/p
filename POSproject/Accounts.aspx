<%@ Page Title="Accounts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="Accounts" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Blank background -->
    <div id="overlay" class="overlay"></div>
    <!-- Popup Dialog Container -->
    <asp:PlaceHolder ID="divpopup" runat="server">
        <div id="popup" class="popup">
            <div class="popup-content">
                <h2 style="color: #FF6347;">Popup Title</h2>
                <p style="color: #333;">Popup message goes here.</p>
                <asp:Button ID="btnNewCustomers" runat="server" Text="New Customer" OnClick="btnNewCustomer" CssClass="popup-button" />
                <asp:Button ID="btnExistingCustomers" runat="server" Text="Existing Customer" OnClick="btnExistingCustomer" CssClass="popup-button" />
                <asp:Button ID="btnViewPayments" runat="server" Text="View Payments" OnClick="btnViewPayment" CssClass="popup-button" />
            </div>
        </div>
    </asp:PlaceHolder>
    <%--/*Creating New Customer*/--%>
    <asp:Panel ID="pnlNewCustomer" runat="server" Visible="false" CssClass="new-customer-panel">
        <h2>New Customer</h2>
        <div class="form-row-group">
            <div class="form-row">
                <asp:Label ID="lblFirstName" runat="server" Text="First Name" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-textbox"></asp:TextBox>
            </div>
            <div class="form-row">
                <asp:Label ID="lblLastName" runat="server" Text="Last Name" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-textbox"></asp:TextBox>
            </div>
        </div>
        <div class="form-row-group">
            <div class="form-row">
                <asp:Label ID="lblPhoneNo" runat="server" Text="Phone Number" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-textbox" MaxLength="11" onkeypress="return isNumberKey(event)"></asp:TextBox>
            </div>
            <div class="form-row">
                <asp:Label ID="lblNIC" runat="server" Text="National Identity Card Number" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtNIC" runat="server" CssClass="form-textbox"  MaxLength="15" onkeypress="return isNumberKey(event)"></asp:TextBox>
            </div>
        </div>
       <div class="form-row-horizontal">
    <asp:Label ID="lblProduct" runat="server" Text="Product Name" CssClass="form-label" style="min-width: 102px;margin-left: 7px;"></asp:Label>
    <asp:TextBox ID="txtProduct" runat="server" CssClass="form-textbox"></asp:TextBox>
    <asp:Label ID="lblPrice" runat="server" Text="Product Price" CssClass="form-label" style="min-width: 94px;margin-left: 7px;"></asp:Label>
    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-textbox"></asp:TextBox>
    <asp:Label ID="lblQuantity" runat="server" Text="Quantity" CssClass="form-label" style="min-width: 50px;margin-left: 12px;"></asp:Label>
    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-textbox"></asp:TextBox>
    <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" OnClick="btnAddProduct_Click" CssClass="form-button" />
</div>
        <br />
        <br />
        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="false" CssClass="grid-view" OnRowCommand="gvProducts_RowCommand">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="ProductPrice" HeaderText="Product Price" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" runat="server" CommandName="Remove" CommandArgument="<%# Container.DataItemIndex %>" Text="Remove"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <br />

        <asp:Label ID="lblAmountPaid" runat="server" Style="font-size: 20px; font-weight: bold; margin-left: 70%;" Text="Amount Paid" CssClass="form-label"></asp:Label>
<asp:TextBox ID="txtAmountPaid" runat="server" style="font-size:20px;font-weight:bold;margin-left: 70%;" CssClass="form-textbox" onkeyup="updateAmountDue();" />      
        <asp:Label ID="tblAmountDue" runat="server" Style="font-size: 20px; font-weight: bold; margin-left: 70%;" Text="Amount Due: PKR 0.0" CssClass="form-label"></asp:Label>
        <asp:Label ID="lblTotalAmount" runat="server" Style="font-size: 20px; font-weight: bold; margin-left: 70%;" Text="Total Amount: PKR 0.0" CssClass="form-label"></asp:Label>
        <asp:Button ID="btnSave" runat="server" Text="Save" Style="margin-left: 70%;" OnClick="btnSubmit" CssClass="form-button" />
    </asp:Panel>


    <style>
        .overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5); /* Semi-transparent black */
            z-index: 999; /* Ensure it's above other content */
        }

        .popup {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: #f5f5f5; /* Light black background */
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
            z-index: 1000; /* Ensure it's above overlay */
        }

        .popup-content {
            text-align: center;
        }

        .popup-button {
            background-color: #FF6347;
            color: white;
            border: none;
            padding: 10px 20px;
            cursor: pointer;
            margin-right: 10px;
        }

            .popup-button:last-child {
                margin-right: 0;
            }

        .new-customer-panel {
            background-color: #f9f9f9;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin: 20px 0;
        }

        .form-label {
            display: block;
            margin: 10px 0 5px;
            color: #333;
            font-size: 16px;
        }

        .form-textbox {
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .form-button {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }

            .form-button:hover {
                background-color: #45a049;
            }

        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }

            .grid-view th, .grid-view td {
                border: 1px solid #ddd;
                padding: 8px;
                text-align: left;
            }

            .grid-view th {
                background-color: #f2f2f2;
            }

        .form-row-group {
    display: flex;
    justify-content: space-between;
    margin-bottom: 10px; /* Space between groups */
}

.form-row {
    display: flex;
    flex-direction: column; /* Stack label and textbox vertically */
    flex-basis: 48%; /* Adjust based on your layout needs */
    max-width: 48%; /* Adjust based on your layout needs */
}

.form-label {
    margin-bottom: 5px; /* Space between label and textbox */
}

.form-textbox {
    width: 100%; /* Textbox takes up full width of its container */
}
.form-row-horizontal {
    display: flex;
    align-items: center; /* Align items vertically in the center */
    justify-content: space-between; /* Distribute space between items */
     /* Allow items to wrap to next line on small screens */
    gap: 10px; /* Space between items */
}

.form-label, .form-textbox, .form-button {
    flex-grow: 1;
    min-width: 120px; /* Minimum width for each item */
}

.form-textbox, .form-button {
    margin-left: 10px; /* Space between label and textbox/button */
}
    </style>

    <script>
        // Function to show popup and overlay
        function showModal() {
            var popup = document.getElementById('popup');
            var overlay = document.getElementById('overlay');
            popup.style.display = 'block';
            overlay.style.display = 'block';
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            // Allow only numeric input
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            // Check for maximum length inside the TextBox
            var textBox = evt.target || evt.srcElement;
            if (textBox.value.length >= 15)
                return false;

            return true;
        }
        function updateAmountDue() {
            var totalAmountText = document.getElementById('<%= lblTotalAmount.ClientID %>').innerText;
            var amountPaidText = document.getElementById('<%= txtAmountPaid.ClientID %>').value;

            // Extract numeric values more reliably
            var totalAmount = parseFloat(totalAmountText.replace(/[^0-9.-]+/g, ""));
            var amountPaid = parseFloat(amountPaidText) || 0; // Default to 0 if parsing fails

            // Check if Amount Paid is greater than Total Amount
            if (amountPaid > totalAmount) {
                alert('Amount Paid cannot be greater than Total Amount.');
                document.getElementById('<%= txtAmountPaid.ClientID %>').value = ''; // Clear the Amount Paid field
                // Optionally, you might not want to reset the Amount Due here to allow for correction
                document.getElementById('<%= tblAmountDue.ClientID %>').innerText = 'Amount Due: PKR ' + totalAmount.toFixed(2);
                return; // Exit the function to prevent further execution
            }

            var amountDue = totalAmount - amountPaid;

            // Update the Amount Due label
            document.getElementById('<%= tblAmountDue.ClientID %>').innerText = 'Amount Due: PKR ' + amountDue.toFixed(2);
        }
        function alertUser(msg) {
            alert('An error occurred.\nReason: ' + msg);
        }
    </script>
</asp:Content>
