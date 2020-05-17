using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace Praca_InzynierskaV2
{
    public partial class InfoScreen : Window
    {
        readonly MySqlConnection connection = new MySqlConnection("server=127.0.0.1;uid=root;database=database;");
        readonly ClearTextBox clearTextBox = new ClearTextBox();
        readonly CheckIsEmpty checkIsEmpty = new CheckIsEmpty();
        readonly ManagementSql managementSql = new ManagementSql();

        public InfoScreen()
        {
            InitializeComponent();
            this.DataContext = new WindowViewModel(this);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInInfoScreen(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola.");
            }
            else if (!string.Equals(txtPassword.Password, txtRepeatPassword.Password))
            {
                MessageBox.Show("Hasła muszą być takie same.");
            }
            else
            {
                try
                {
                    if (managementSql.Add_to_loginuser(this, connection).ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Dodano użytkownika");
                        clearTextBox.ClearTextBoxInInfoScreen(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie dodano.");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Duplicate"))
                    {
                        txtPassword.Clear();
                        txtRepeatPassword.Clear();
                        MessageBox.Show("Użytkownik o takiej nazwie już istnieje!");
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }
    }
}
