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
    public partial class ProductReference : Form
    {
        public ProductReference()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadProductReference();
        }

        private void LoadProductReference()
        {
            listView1.Items.Clear();
            int result = Program.MakeQuery
            (
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Product.Code, Product.Name, Product.Price FROM Product",
                    }
                ),
                x =>
                {
                    ListViewItem item = new ListViewItem(Program.DecodeUTF8(x, "Code") ?? String.Empty);
                    item.SubItems.Add(Program.DecodeUTF8(x, "Name") ?? String.Empty);
                    item.SubItems.Add(Program.GetInteger(x, "Price").ToString());
                    listView1.Items.Add(item);
                }
            );
        }
    }
}
