using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaxPaymentsReports
{
    public partial class NDFLReport : Form
    {
        public NDFLReport()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadNDFLReport();
        }

        private void LoadNDFLReport()
        {
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Company.Code AS CompanyCode, Company.Name AS CompanyName, LaborHours.Quarter AS Quarter, City.Name AS CityName,",
                        "SUM(LaborHours.HoursWorked) AS SumHoursWorked,",
                        "SUM(LaborHours.HoursWorked * Employer.Rate) AS SalaryPayed,",
                        "CAST(SUM(LaborHours.HoursWorked * Employer.Rate * 0.13) AS DECIMAL(18,4)) AS TaxTotal",
                        "FROM ((LaborHours",
                        "INNER JOIN Company ON LaborHours.RCompany = Company.Oid",
                        "INNER JOIN City ON Company.RCity = City.Oid)",
                        "INNER JOIN Employer ON LaborHours.REmployer = Employer.Oid)",
                        "GROUP BY City.Name, LaborHours.Quarter"
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.DecodeUTF8(x, "CompanyCode") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "CompanyName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Quarter").ToString());
                    item.SubItems.Add(Program.DecodeUTF8(x, "CityName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "SumHoursWorked").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "SalaryPayed").ToString());
                    item.SubItems.Add(Program.GetInteger(x, "TaxTotal").ToString());
                    item.SubItems[6].BackColor = System.Drawing.Color.FromArgb(243, 235, 185);
                    item.UseItemStyleForSubItems = false;
                    listView1.Items.Add(item);
                }
            );
        }
    }
}
