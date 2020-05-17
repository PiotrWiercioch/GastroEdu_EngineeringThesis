using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Praca_InzynierskaV2
{
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
            this.DataContext = new WindowViewModel(this);
        }

        private string user_type;

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private void BtnSumbit_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("server=127.0.0.1;uid=root;database=database;");
            try
            {
                using (MD5 hash = MD5.Create())
                {
                    txtPassword.Password = GetMd5Hash(hash, txtPassword.Password);
                }
                connection.Open();
                string query = "SELECT COUNT(*) FROM loginuser WHERE UserName = '" + txtUsername.Text + "' AND Password = '" + txtPassword.Password + "'";
                MySqlCommand command = new MySqlCommand(query, connection);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 1)
                {
                    string query2 = "SELECT user_type FROM loginuser WHERE UserName = '" + txtUsername.Text + "'";
                    MySqlCommand command2 = new MySqlCommand(query2, connection);
                    MySqlDataReader reader = command2.ExecuteReader();
                    reader.Read();
                    user_type = reader[0].ToString();
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    dashboard.User_rule(user_type);
                    this.Close();
                }
                else
                {
                    txtPassword.Clear();
                    MessageBox.Show("Nazwa użytkownika lub hasło są nieprawidłowe.");
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Unable to connect"))
                {
                    txtPassword.Clear();
                    MessageBox.Show("Problem z połączeniem się z serwerem.");
                }
                else
                txtPassword.Clear();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
