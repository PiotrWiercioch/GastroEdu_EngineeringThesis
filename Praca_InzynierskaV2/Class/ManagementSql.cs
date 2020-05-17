using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Praca_InzynierskaV2
{
    class ManagementSql
    {
        DataSet dataSet;
        string query;
        public void Connection_Data_From_DataBase(MySqlConnection connection, string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "LoadDataBinding");
        }

        #region provider

        public void Data_In_DataGrid_Provider(MainWindow window, MySqlConnection connection)
        {
            query = "select * from provider;";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridProvider.DataContext = dataSet;
        }

        public void Search_Data_In_DataGrid_Provider(MainWindow window, MySqlConnection connection, string valueToSearch)
        {
            query = "SELECT * FROM provider WHERE CONCAT(`name_provider`,`nip`,`city`,`street`,`houseNumber`,`phoneNumber`,`bankAccount`) like '%" + valueToSearch + "%'";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridProvider.DataContext = dataSet;
        }

        public void Data_In_ComboBox_Provider(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT * FROM provider";
            Connection_Data_From_DataBase(connection, query);
            window.cbbProvider.ItemsSource = dataSet.Tables[0].DefaultView;
            window.cbbProvider.SelectedValuePath = dataSet.Tables[0].Columns["id_provider"].ToString();
        }

        public MySqlCommand Add_to_Provider(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "INSERT INTO provider(name_provider,nip,city,street,houseNumber,phoneNumber,bankAccount) VALUES ('" + window.txtNazwa.Text + "','" + window.txtNip.Text + "','" + window.txtMiejscowosc.Text + "','" + window.txtUlica.Text + "','" + window.txtNr_domu.Text + "','" + window.txtNr_tel.Text + "','" + window.txtNr_konta.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Edit_in_Provider(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "UPDATE provider SET name_provider = '" + window.txtNazwa.Text + "', nip = '" + window.txtNip.Text + "', city = '" + window.txtMiejscowosc.Text + "', street = '" + window.txtUlica.Text + "', houseNumber = '" + window.txtNr_domu.Text + "', phoneNumber = '" + window.txtNr_tel.Text + "', bankAccount = '" + window.txtNr_konta.Text + "' WHERE id_provider= '" + window.txtId.Text + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Delete_in_Provider(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "DELETE FROM provider WHERE id_provider=" + window.txtId.Text + "";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }

        #endregion

        #region card index

        public void Data_In_DataGrid_CardIndex(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT `id`, `file`, `name`, `price`, `unit`,`id_provider`, `name_provider`, `nip`, `total_quantity_inventory` FROM `card_index` JOIN `provider` ON card_index.id_provider_index = provider.id_provider";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridCardIndex.DataContext = dataSet;
        }

        public void Search_Data_In_DataGrid_CardIndex(MainWindow window, MySqlConnection connection, string valueToSearch)
        {
            query = "SELECT `file`, `name`, `price`, `unit`,`id_provider`, `name_provider`, `nip`, `total_quantity_inventory` FROM card_index JOIN provider ON card_index.id_provider_index = provider.id_provider WHERE CONCAT(`file`,`name`,`price`,`unit`, `name_provider`, `nip`) like '%" + valueToSearch + "%'";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridCardIndex.DataContext = dataSet;
        }
        public MySqlCommand Add_to_Card_index(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "INSERT INTO card_index(`file`, `name`, `price`, `unit`, `id_provider_index`, `total_quantity_inventory`) VALUES ('" + window.txtIKartoteki_CI.Text + "','" + window.txtNazwa_CI.Text + "','" + window.txtCena_CI.Text.Replace(",", ".") + "','" + window.cbbJMiary_CI.Text + "', '" + window.txt_id_cbbSupp.Text + "','" + window.txtStan_poczatkowy_inv.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Edit_in_Card_index(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "UPDATE card_index SET file = '" + window.txtIKartoteki_CI.Text + "', name = '" + window.txtNazwa_CI.Text + "', price = '" + window.txtCena_CI.Text.Replace(",", ".") + "', unit = '" + window.cbbJMiary_CI.Text + "', id_provider_index = '" + window.txt_id_cbbSupp.Text + "', total_quantity_inventory = '" + window.txtStan_poczatkowy_inv.Text + "'  WHERE id= '" + window.txtId_CI.Text + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Delete_in_Card_index(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "DELETE FROM card_index WHERE id=" + window.txtId_CI.Text + "";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }

        #endregion

        #region inventory
        public void Data_In_ComboBox_File(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT * FROM card_index";
            Connection_Data_From_DataBase(connection, query);
            window.cbbFile.ItemsSource = dataSet.Tables[0].DefaultView;
            window.cbbFile.SelectedValuePath = dataSet.Tables[0].Columns["id"].ToString();
        }

        public void Data_In_ComboBox_Shortcut_name_invoice(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT * FROM invoice";
            Connection_Data_From_DataBase(connection, query);
            window.cbb_shortcut_name_invoice.ItemsSource = dataSet.Tables[0].DefaultView;
            window.cbb_shortcut_name_invoice.SelectedValuePath = dataSet.Tables[0].Columns["id_invoice"].ToString();
        }

        public void Data_In_DataGrid_Invenotory(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT SUB2.*,card_index.file,card_index.name,card_index.unit,SUB2.total_after* card_index.price AS total_after_price,SUB2.total_before* card_index.price AS total_before_price,SUB2.income_total* card_index.price AS income_total_price,SUB2.expense_total* card_index.price AS expense_total_price,invoice.shortcut_name_invoice FROM(SELECT SUB1.*,SUB1.income_total - SUB1.expense_total + card_index.total_quantity_inventory AS total_after, SUB1.income_total - inventory.income_inventory - SUB1.expense_total + inventory.expenditure_inventory + card_index.total_quantity_inventory AS total_before FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, T1.id_invoice_index, T1.income_inventory, T1.expenditure_inventory, SUM(T2.income_inventory) AS income_total, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.date_add_inventory >= T2.date_add_inventory AND T1.id_ci_index = T2.id_ci_index GROUP BY T1.id_ci_index, T1.id_inventory, T1.date_add_inventory) SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id) SUB2 JOIN card_index ON SUB2.id_ci_index = card_index.id INNER JOIN invoice ON SUB2.id_invoice_index = invoice.id_invoice ORDER BY date_add_inventory DESC, id_inventory DESC;";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridInventory.DataContext = dataSet;
        }
        public void Search_Data_In_DataGrid_Inventory(MainWindow window, MySqlConnection connection, string valueToSearch)
        {
            query = "SELECT SUB3.* FROM(SELECT SUB2.*,card_index.file,card_index.name,card_index.unit,SUB2.total_after* card_index.price AS total_after_price,SUB2.total_before* card_index.price AS total_before_price,SUB2.income_total* card_index.price AS income_total_price,SUB2.expense_total* card_index.price AS expense_total_price, invoice.shortcut_name_invoice FROM(SELECT SUB1.*,SUB1.income_total - SUB1.expense_total + card_index.total_quantity_inventory AS total_after, SUB1.income_total - inventory.income_inventory - SUB1.expense_total + inventory.expenditure_inventory + card_index.total_quantity_inventory AS total_before FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, T1.id_invoice_index, T1.income_inventory, T1.expenditure_inventory, SUM(T2.income_inventory) AS income_total, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.date_add_inventory >= T2.date_add_inventory AND T1.id_ci_index = T2.id_ci_index GROUP BY T1.id_ci_index, T1.id_inventory, T1.date_add_inventory) SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id) SUB2 JOIN card_index ON SUB2.id_ci_index = card_index.id INNER JOIN invoice ON SUB2.id_invoice_index = invoice.id_invoice) SUB3 WHERE CONCAT(`file`,`name`,`total_after_price`,`total_before_price`,`unit`,`income_total_price`,`expense_total_price`,`total_after`,`total_before`,`date_add_inventory`,`income_inventory`,`expenditure_inventory`,`income_total`,`expense_total`,`shortcut_name_invoice`) LIKE '%" + valueToSearch + "%' ORDER BY date_add_inventory DESC, id_inventory DESC";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridInventory.DataContext = dataSet;
        }

        public MySqlCommand Add_to_Inventory_automatic_date(MainWindow window, MySqlConnection connection)
        {
            DateTime dt = DateTime.Now;
            connection.Open();
            query = "INSERT INTO inventory(`income_inventory`, `expenditure_inventory`, `date_add_inventory`, `id_ci_index`, `id_invoice_index`) VALUES ('" + window.txtPrzychod_inv.Text + "','" + window.txtRozchod_inv.Text + "','" + dt.Date.ToString("yyyy-MM-dd") + "', '" + window.txtId_ci_index.Text + "', '" + window.txtId_invoice_index.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Add_to_Inventory_manual_date(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate = window.DatePickerManual.SelectedDate.Value.Date;
            connection.Open();
            query = "INSERT INTO inventory(`income_inventory`, `expenditure_inventory`, `date_add_inventory`, `id_ci_index`, `id_invoice_index`) VALUES ('" + window.txtPrzychod_inv.Text + "','" + window.txtRozchod_inv.Text + "','" + selectedDate.Date.ToString("yyyy-MM-dd") + "', '" + window.txtId_ci_index.Text + "', '" + window.txtId_invoice_index.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Edit_in_Inventory(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate = window.DatePickerManual.SelectedDate.Value.Date;
            connection.Open();
            query = "UPDATE inventory SET income_inventory = '" + window.txtPrzychod_inv.Text + "', expenditure_inventory = '" + window.txtRozchod_inv.Text + "', date_add_inventory = '" + selectedDate.Date.ToString("yyyy-MM-dd") + "', id_ci_index = '" + window.txtId_ci_index.Text + "', id_invoice_index = '" + window.txtId_invoice_index.Text + "' WHERE id_inventory= '" + window.txtId_inventory.Text + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Delete_in_Inventory(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "DELETE FROM inventory WHERE id_inventory=" + window.txtId_inventory.Text + "";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }

        public void Data_total_quantity_from_inventory(MainWindow window, MySqlConnection connection)
        {
            bool checkIsEmpty = String.IsNullOrWhiteSpace(window.txtId_ci_index.Text);
            if (checkIsEmpty != true)
            {
                query = "SELECT SUM(T2.income_inventory) - SUM(T2.expenditure_inventory) + card_index.total_quantity_inventory AS stan_magazynu FROM inventory T1 INNER JOIN card_index ON T1.id_ci_index = card_index.id INNER JOIN inventory T2 ON T1.id_ci_index = T2.id_ci_index WHERE T1.id_ci_index = " + window.txtId_ci_index.Text + " GROUP BY T1.id_ci_index";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    window.txtTotall_quantity.Text = reader[0].ToString();
                }
                else
                {
                    window.txtTotall_quantity.Text = "8*8";
                }
                    reader.Close();

                if (window.txtTotall_quantity.Text == "8*8")
                {
                    string query2 = "SELECT total_quantity_inventory AS stan_magazynu FROM card_index WHERE id = " + window.txtId_ci_index.Text + ";";
                    MySqlCommand command2 = new MySqlCommand(query2, connection);
                    MySqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.Read())
                    {
                        window.txtTotall_quantity.Text = reader2[0].ToString();
                    }
                    reader2.Close();
                }
            }
        }

        #endregion

        #region invoice

        public void Data_In_DataGrid_Invoice(MainWindow window, MySqlConnection connection)
        {
            query = "SELECT `id_invoice`, `name_invoice`, `net_amount_invoice`, `gross_amount_invoice`, DATE_FORMAT(`date_invoice`, '%d.%m.%Y') AS date_invoice, `shortcut_name_invoice` FROM invoice";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridInvoice.DataContext = dataSet;
        }
        public void Search_Data_In_DataGrid_Invoice(MainWindow window, MySqlConnection connection, string valueToSearch)
        {
            query = "SELECT `id_invoice`, `name_invoice`, `net_amount_invoice`, `gross_amount_invoice`, DATE_FORMAT(`date_invoice`, '%d.%m.%Y') AS date_invoice, `shortcut_name_invoice` FROM invoice WHERE CONCAT(`name_invoice`,`net_amount_invoice`,`gross_amount_invoice`,`date_invoice`,`shortcut_name_invoice`) like '%" + valueToSearch + "%'";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridInvoice.DataContext = dataSet;
        }

        public MySqlCommand Add_to_Invoice(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate = window.DatePickerInvoice.SelectedDate.Value.Date;
            connection.Open();
            query = "INSERT INTO invoice(`name_invoice`,`net_amount_invoice`,`gross_amount_invoice`,`date_invoice`,`shortcut_name_invoice`) VALUES ('" + window.txtNumer_faktury.Text + "','" + window.txtWartosc_netto.Text.Replace(",", ".") + "','" + window.txtWartosc_brutto.Text.Replace(",", ".") + "','" + selectedDate.Date.ToString("yyyy-MM-dd") + "', '" + window.txtSkrot_faktury.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Edit_in_Invoice(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate = window.DatePickerInvoice.SelectedDate.Value.Date;
            connection.Open();
            query = "UPDATE invoice SET name_invoice = '" + window.txtNumer_faktury.Text + "', net_amount_invoice = '" + window.txtWartosc_netto.Text.Replace(",", ".") + "', gross_amount_invoice = '" + window.txtWartosc_brutto.Text.Replace(",", ".") + "', date_invoice = '" + selectedDate.Date.ToString("yyyy-MM-dd") + "', shortcut_name_invoice = '" + window.txtSkrot_faktury.Text + "' WHERE id_invoice= '" + window.txtId_invoice.Text + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        public MySqlCommand Delete_in_Invoice(MainWindow window, MySqlConnection connection)
        {
            connection.Open();
            query = "DELETE FROM invoice WHERE id_invoice=" + window.txtId_invoice.Text + "";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }

        #endregion

        #region report summary

        public void Data_In_DataGrid_ReportSummary(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate1 = window.DatePickerSummary1.SelectedDate.Value.Date;
            DateTime selectedDate2 = window.DatePickerSummary2.SelectedDate.Value.Date;
            query = "SELECT date_add_inventory AS Data, SUM(Wartosc) AS Wartosc1 FROM(SELECT card_index.price AS 'Cena jednostki', ROUND(SUB1.expense_total, 2) AS 'Ilosc', ROUND(SUB1.expense_total * card_index.price, 2) AS Wartosc, card_index.file, SUB1.date_add_inventory FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.id_inventory = T2.id_inventory AND t1.date_add_inventory BETWEEN '"+ selectedDate1.Date.ToString("yyyy-MM-dd") + "' AND '" + selectedDate2.Date.ToString("yyyy-MM-dd") + "' GROUP BY T1.date_add_inventory)SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id ORDER BY NAME DESC)SUB2 GROUP BY date_add_inventory;";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridReportSummary.DataContext = dataSet;
        }

        #endregion

        #region report invoice

        public void Data_In_DataGrid_InvoiceSummary(MainWindow window, MySqlConnection connection)
        {
            DateTime selectedDate1 = window.DatePickerSummary1.SelectedDate.Value.Date;
            DateTime selectedDate2 = window.DatePickerSummary2.SelectedDate.Value.Date;
            query = "SELECT `name_invoice`, `net_amount_invoice`, `gross_amount_invoice`, DATE_FORMAT(`date_invoice`, '%d.%m.%Y') AS date_invoice FROM invoice WHERE date_invoice BETWEEN '" + selectedDate1.Date.ToString("yyyy-MM-dd") + "' AND '" + selectedDate2.Date.ToString("yyyy-MM-dd") + "'";
            Connection_Data_From_DataBase(connection, query);
            window.DataGridInvoiceSummary.DataContext = dataSet;
        }

        #endregion


        #region InfoScreen
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
        public MySqlCommand Add_to_loginuser(InfoScreen window, MySqlConnection connection)
        {
            using (MD5 hash = MD5.Create())
            {
                window.txtPassword.Password = GetMd5Hash(hash, window.txtPassword.Password);
            }
            connection.Open();
            query = "INSERT INTO loginuser(UserName, Password, user_type) VALUES ('" + window.txtUsername.Text + "','" + window.txtPassword.Password + "','" + window.cbb_user_type.Text + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }
        #endregion

    }
}
