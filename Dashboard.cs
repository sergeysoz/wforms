using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaxPaymentsReports
{
    public partial class Dashboard : Form
    {
        Thread? th;
        public Dashboard(int level, string user)
        {
            InitializeComponent();
            this.Text = Program.Concat
            (
                new string[]
                {
                    $"Учет уплаты налогов ({user}):",
                    $"уровень доступа {level} |",
                    $"{(level == 1 ? "максимальные права" : "ограниченные права")}"
                }
            );
            this.CenterToScreen();
            this.button1.Enabled = level == 1 ? true : false;
            this.button3.Enabled = level == 1 ? true : false;
            LoadInvoices();
            LoadLaborHours();
        }

        private void LoadInvoices()
        {
            string user = String.Empty;
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Invoices.Count AS Count,",
                        "Invoices.Quarter AS Quarter,",
                        "Client.CompanyName AS ClientName,",
                        "Product.Name AS ProductName,",
                        "Product.Price AS ProductPrice,",
                        "Company.Name AS CompanyName",
                        "FROM (((Invoices",
                        "INNER JOIN Client ON Invoices.RClient = Client.Oid)",
                        "INNER JOIN Product ON Invoices.RProduct = Product.Oid)",
                        "INNER JOIN Company ON Invoices.RCompany = Company.Oid)"
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.GetInteger(x, "Count").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "Quarter").ToString());
                    item.SubItems.Add(Program.DecodeUTF8(x, "ClientName") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "ProductName") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "CompanyName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "ProductPrice").ToString());
                    listView1.Items.Add(item);
                }
            );
        }

        private void LoadLaborHours()
        {
            listView2.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT LaborHours.HoursWorked AS HoursWorked,",
                        "LaborHours.Quarter AS Quarter,",
                        "Employer.FirstName AS FirstName,",
                        "Employer.LastName AS LastName,",
                        "Employer.Rate AS Rate,",
                        "Company.Code AS CompanyCode,",
                        "Company.Name AS CompanyName,",
                        "Position.Name AS PositionName,",
                        "City.Name AS CityName",
                        "FROM ((LaborHours",
                        "INNER JOIN Company ON LaborHours.RCompany = Company.Oid",
                        "LEFT OUTER JOIN City ON Company.RCity = City.Oid)",
                        "INNER JOIN Employer ON LaborHours.REmployer = Employer.Oid ",
                        "LEFT OUTER JOIN Position ON Employer.RPosition = Position.Oid)"
                    }
                ),
                x =>
                {
                    string firstName = Program.DecodeUTF8(x, "FirstName") ?? String.Empty;
                    string lastName = Program.DecodeUTF8(x, "LastName") ?? String.Empty;
                    ListViewItem item = new ListViewItem($"{lastName} {firstName}");
                    item.SubItems.Add(Program.DecodeUTF8(x, "CompanyName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Rate").ToString());
                    item.SubItems.Add(Program.DecodeUTF8(x, "PositionName") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "CityName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Quarter").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "HoursWorked").ToString());
                    listView2.Items.Add(item);
                }
            );
        }

        private void button1_Click(object? sender, System.EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new SalesTaxReport()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(x => Application.Run(new Authorization()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new NDFLReport()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new ClientReference()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new EmployerReference()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new ProductReference()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // this.Close();
            th = new Thread(x => Application.Run(new CompanyReference()));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
