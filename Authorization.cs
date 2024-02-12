using System.Threading;

namespace TaxPaymentsReports
{
    public partial class Authorization : Form
    {
        Thread? th;
        public Authorization()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = String.Empty;
            int result = Program.MakeQuery(
                Program.Concat
                (
                    new string[]
                    {
                        "SELECT Permission.Level AS Level, Profile.UserName AS UserName FROM Permission",
                        "INNER JOIN Profile ON Permission.Oid = Profile.RPermission",
                        "OUTER LEFT JOIN Accountants ON Profile.Oid = Accountants.RProfile",
                        $"WHERE Profile.Password = '{textBox2.Text}' AND Profile.UserName = '{textBox1.Text}'"
                    }
                ),
                x => 
                { 
                    int level = Program.GetInteger(x, "Level"); user = Program.DecodeUTF8(x, "UserName");
                    // Program.Trace(Program.GetInteger(x, "Level").ToString(), "LEVEL");
                    this.Close();
                    th = new Thread(x => Application.Run(new Dashboard(level, user)));
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //
        }
    }
}