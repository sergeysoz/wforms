using System.Data.SQLite;
using System.Text;

namespace TaxPaymentsReports
{
    internal static class Program
    {

        public static string DecodeUTF8(SQLiteDataReader reader, string name)
        {
            byte[] readBytes = System.Text.Encoding.UTF8.GetBytes((string)(reader[$"{name}"]));
            string readName = Encoding.UTF8.GetString(readBytes);
            return readName;
        }

        public static int GetInteger(SQLiteDataReader reader, string name)
        {
            return Convert.ToInt32(reader[name]);
        }

        public static string GenerateOid()
        {
            return String.Join("", BitConverter.GetBytes(Guid.NewGuid().GetHashCode())
                .Select(x => x.ToString("X2")));
        }

        public static string ToBlob(string text)
        {
            return String.Join("", System.Text.Encoding.ASCII.GetBytes(text).Select(x => x.ToString("X2")));
        }

        public static string Concat(string[] query)
        {
            if (query.Count() == 0) return String.Empty;
            return query.Aggregate("", (x, y) => x + " " + y);
        }

        public static void Trace(string message, string title)
        {
            System.Windows.Forms.MessageBox.Show(message, title);
        }

        public static int MakeQuery(string request, Action<SQLiteDataReader> handler)
        {
            string cs = @"URI=file:" + System.Windows.Forms.Application.StartupPath + "\\taxpayments.db";
            SQLiteConnection connection;
            SQLiteCommand command;
            SQLiteDataReader reader;

            connection = new SQLiteConnection(cs);
            connection.Open();

            command = new SQLiteCommand(request, connection);

            using (reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    reader.Close();
                    command.Dispose();
                    connection.Close();
                    return 1;
                }
                while (reader.Read()) { handler(reader); }
            }
            reader.Close();
            command.Dispose();
            connection.Close();
            return 0;
        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Authorization());
        }
    }
}