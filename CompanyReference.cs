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
    public partial class CompanyReference : Form
    {
        public CompanyReference()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadCompanyReference();
        }

        private void LoadCompanyReference()
        {
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Company.Code AS Code, Company.Name AS Name,",
                        "City.Name AS CityName, Area.Name AS AreaName",
                        "FROM Company",
                        "INNER JOIN City ON Company.RCity = City.Oid",
                        "LEFT OUTER JOIN Area ON City.RArea = Area.Oid",
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.DecodeUTF8(x, "Code") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "Name") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "CityName") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "AreaName") ?? String.Empty);
                    listView1.Items.Add(item);
                }
            );
        }
    }
}
