using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaxPaymentsReports
{
    public partial class SalesTaxReport : Form
    {
        public SalesTaxReport()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadTaxReport();
        }

        private void LoadTaxReport()
        {
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Company.Code AS CompanyCode, Company.Name AS CompanyName, Invoices.Quarter AS Quarter, City.Name AS CityName,",
                        "SUM(Invoices.Count * Product.Price) AS SumPriceMulCount,",
                        "SUM(Invoices.Count) AS SumCount,",
                        "CAST(SUM(Invoices.Count * Product.Price * 0.2) AS DECIMAL(18,4)) AS TaxTotal",
                        "FROM ((Invoices",
                        "INNER JOIN Company ON Invoices.RCompany = Company.Oid",
                        "INNER JOIN City ON Company.RCity = City.Oid)",
                        "INNER JOIN Product ON Invoices.RProduct = Product.Oid)",
                        "GROUP BY City.Name, Invoices.Quarter"
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.DecodeUTF8(x, "CompanyCode") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "CompanyName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Quarter").ToString());
                    item.SubItems.Add(Program.DecodeUTF8(x, "CityName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "SumPriceMulCount").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "SumCount").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "TaxTotal").ToString());
                    item.SubItems[6].BackColor = System.Drawing.Color.FromArgb(243, 235, 185);
                    item.UseItemStyleForSubItems = false;
                    listView1.Items.Add(item);
                }
            );
        }
    }
}
