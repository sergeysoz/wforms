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
    public partial class EmployerReference : Form
    {
        public EmployerReference()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadEmployerReference();
        }

        private void LoadEmployerReference()
        {
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Employer.FirstName AS FirstName, Employer.LastName AS LastName,",
                        "Employer.Rate AS Rate, Position.Name AS Position",
                        "FROM Employer",
                        "INNER JOIN Position ON Employer.RPosition = Position.Oid",
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.DecodeUTF8(x, "LastName") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "FirstName") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Rate").ToString());
                    item.SubItems.Add(Program.DecodeUTF8(x, "Position") ?? String.Empty);
                    listView1.Items.Add(item);
                }
            );
        }
    }
}
