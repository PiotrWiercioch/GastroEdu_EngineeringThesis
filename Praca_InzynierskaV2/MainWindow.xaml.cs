using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Praca_InzynierskaV2
{
    public partial class MainWindow : Window
    {
        readonly MySqlConnection connection = new MySqlConnection("server=127.0.0.1;uid=root;database=database;");

        readonly ClearTextBox clearTextBox = new ClearTextBox();
        readonly CheckIsEmpty checkIsEmpty = new CheckIsEmpty();
        readonly ManagementSql managementSql = new ManagementSql();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new WindowViewModel(this);
        }
        public void User_rule(string user_type)
        {
            if (user_type == "użytkownik")
            {
                InfoButton.Visibility = Visibility.Collapsed;
            }
            else if (user_type == "gość")
            {
                InfoButton.Visibility = Visibility.Collapsed;
                btnDodajProvider.IsEnabled = false;
                btnDodaj_CI.IsEnabled = false;
                btnDodaj_Invoice.IsEnabled = false;
                btnDodaj_Inventory.IsEnabled = false;
                btnEdytujProvider.IsEnabled = false;
                btnEdytuj_CI.IsEnabled = false;
                btnEdytuj_Invoice.IsEnabled = false;
                btnEdytuj_Inventory.IsEnabled = false;
                btnUsunProvider.IsEnabled = false;
                btnUsun_CI.IsEnabled = false;
                btnUsun_Invoice.IsEnabled = false;
                btnUsun_Inventory.IsEnabled = false;
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            InfoScreen dashboard = new InfoScreen();
            dashboard.ShowDialog();
        }


        #region card index
        private void Button_Kartoteki(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 4;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Data_In_DataGrid_CardIndex(this, connection);
                managementSql.Data_In_ComboBox_Provider(this, connection);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        private void BtnDodajCI_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInCardIndex(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (managementSql.Add_to_Card_index(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_CardIndex(this, connection);
                        MessageBox.Show("Dodano");
                        clearTextBox.ClearTextBoxInCardIndex(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie dodano");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Duplicate"))
                    {
                        MessageBox.Show("TAKI INDEKS KARTOTEKI JUŻ ISTNIEJE!");
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void BtnEdytujCI_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInCardIndex(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (managementSql.Edit_in_Card_index(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_CardIndex(this, connection);
                        MessageBox.Show("EDYTOWANO");
                        clearTextBox.ClearTextBoxInCardIndex(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie edytowano");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.Close();
        }

        private void BtnUsunCI_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Ostrzeżenie!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (managementSql.Delete_in_Card_index(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_CardIndex(this, connection);
                        MessageBox.Show("Usunieto");
                        clearTextBox.ClearTextBoxInCardIndex(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie usunieto");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Cannot delete or update a parent row"))
                    {
                        MessageBox.Show("Ten wpis jest już gdzieś wykorzystywany! Nie można go usunąć! Jeśli chcesz usunąć ten wpis, nie możesz wykorzystywać go w innym miejscu!");
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Anulowano operację.");
            }

        }

        public void SearchDataCI(string valueToSearch)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Search_Data_In_DataGrid_CardIndex(this, connection, valueToSearch);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        private void TxtSzukaj_CI_TextChanged(object sender, TextChangedEventArgs e)
        {
            string valueToSearch = txtSzukaj_CI.Text.ToString();
            SearchDataCI(valueToSearch);
        }

        private void DataGridCardIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem is DataRowView row_selected)
            {
                try
                {
                    txtId_CI.Text = row_selected[0].ToString();
                    txtIKartoteki_CI.Text = row_selected[1].ToString();
                    txtNazwa_CI.Text = row_selected[2].ToString();
                    txtCena_CI.Text = row_selected[3].ToString();
                    cbbJMiary_CI.Text = row_selected[4].ToString();
                    cbbProvider.SelectedValue = row_selected[5].ToString();
                    txtStan_poczatkowy_inv.Text = row_selected[8].ToString();
                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }
            }
        }

        private void BtnWyczyscCI_Click(object sender, RoutedEventArgs e)
        {
            clearTextBox.ClearTextBoxInCardIndex(this);
        }

        #endregion

        #region inventory

        private void Button_Magazyn(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 5;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Data_In_DataGrid_Invenotory(this, connection);
                managementSql.Data_In_ComboBox_File(this, connection);
                managementSql.Data_In_ComboBox_Shortcut_name_invoice(this, connection);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }
        private void BtnDodaj_Inventory_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInInventory(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                string a = txtPrzychod_inv.Text;
                string b = txtRozchod_inv.Text;
                string c = txtTotall_quantity.Text;
                double numberA = double.Parse(a.Replace(".", ","));
                double numberB = double.Parse(b.Replace(".", ","));
                double numberC = double.Parse(c);

                if ((numberA - numberB + numberC) >= 0)
                {

                    try
                    {
                        if (AutomaticDateRadioButton.IsChecked == true)
                        {
                            if (managementSql.Add_to_Inventory_automatic_date(this, connection).ExecuteNonQuery() == 1)
                            {
                                managementSql.Data_In_DataGrid_Invenotory(this, connection);
                                MessageBox.Show("Dodano");
                            }
                            else
                            {
                                MessageBox.Show("Nie dodano");
                            }
                        }
                        else
                        {
                            if (managementSql.Add_to_Inventory_manual_date(this, connection).ExecuteNonQuery() == 1)
                            {
                                managementSql.Data_In_DataGrid_Invenotory(this, connection);
                                MessageBox.Show("Dodano");
                            }
                            else
                            {
                                MessageBox.Show("Nie dodano");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    clearTextBox.ClearTextBoxInInventory(this);
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("BRAK TYLU PRODUKTÓW NA MAGAZYNIE, ZMNIEJSZ ROZCHÓD!");
                }
            }
        }
        private void BtnEdytuj_Inventory_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInInventory(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (AutomaticDateRadioButton.IsChecked == false)
                    {
                        if (managementSql.Edit_in_Inventory(this, connection).ExecuteNonQuery() == 1)
                        {
                            managementSql.Data_In_DataGrid_Invenotory(this, connection);
                            MessageBox.Show("Edytowano");
                            clearTextBox.ClearTextBoxInInventory(this);
                        }
                        else
                        {
                            MessageBox.Show("Nie dodano");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Przy edycji wybierz datę ręcznie!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }
        private void BtnUsun_Inventory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Ostrzeżenie!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (managementSql.Delete_in_Inventory(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Invenotory(this, connection);
                        MessageBox.Show("Usunięto");
                        clearTextBox.ClearTextBoxInInventory(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie dodano");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Anulowano operację.");
            }


        }

        private void BtnWyczysc_Inventory_Click(object sender, RoutedEventArgs e)
        {
            clearTextBox.ClearTextBoxInInventory(this);
        }

        private void DataGridInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem is DataRowView row_selected)
            {
                txtId_inventory.Text = row_selected[0].ToString();
                ManualDateRadioButton.IsChecked = true;
                string s = row_selected[1].ToString();
                DatePickerManual.SelectedDate = DateTime.ParseExact(s, "dd.MM.yyyy", null);
                cbbFile.SelectedValue = row_selected[2].ToString();
                cbb_shortcut_name_invoice.SelectedValue = row_selected[2].ToString();
                txtPrzychod_inv.Text = row_selected[6].ToString();
                txtRozchod_inv.Text = row_selected[5].ToString();
            }

        }

        public void SearchDataInventory(string valueToSearch)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Search_Data_In_DataGrid_Inventory(this, connection, valueToSearch);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        private void TxtSzukaj_Inv_TextChanged(object sender, TextChangedEventArgs e)
        {
            string valueToSearch = txtSzukaj_Inv.Text.ToString();
            SearchDataInventory(valueToSearch);
        }

        private void TxtId_ci_index_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Data_total_quantity_from_inventory(this, connection);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        #endregion

        #region provider
        private void Button_Dostawcy(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 1;
            try
            {
                connection.Open();
                managementSql.Data_In_DataGrid_Provider(this, connection);
                connection.Close();
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
        }
        private void DataGridProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem is DataRowView row_selected)
            {
                txtId.Text = row_selected[0].ToString();
                txtNazwa.Text = row_selected[1].ToString();
                txtNip.Text = row_selected[2].ToString();
                txtMiejscowosc.Text = row_selected[3].ToString();
                txtUlica.Text = row_selected[4].ToString();
                txtNr_domu.Text = row_selected[5].ToString();
                txtNr_tel.Text = row_selected[6].ToString();
                txtNr_konta.Text = row_selected[7].ToString();
            }
        }



        private void BtnDodajProvider_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInProvider(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (managementSql.Add_to_Provider(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Provider(this, connection);
                        MessageBox.Show("Dodano");
                        clearTextBox.ClearTextBoxInProvider(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie dodano");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Duplicate"))
                    {
                        MessageBox.Show("DOSTAWCA O TAKIM NUMERZE NIP JEST JUŻ W BAZIE!");
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void BtnEdytujProvider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (managementSql.Edit_in_Provider(this, connection).ExecuteNonQuery() == 1)
                {
                    managementSql.Data_In_DataGrid_Provider(this, connection);
                    MessageBox.Show("EDYTOWANO");
                    clearTextBox.ClearTextBoxInProvider(this);
                }
                else
                {
                    MessageBox.Show("Nie edytowano");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void BtnUsunProvider_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Ostrzeżenie!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (managementSql.Delete_in_Provider(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Provider(this, connection);
                        MessageBox.Show("Usunięto");
                        clearTextBox.ClearTextBoxInProvider(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie usunięto");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Cannot delete or update a parent row"))
                    {
                        MessageBox.Show("Ten wpis jest już gdzieś wykorzystywany! Nie można go usunąć! Jeśli chcesz usunąć ten wpis, nie możesz wykorzystywać go w innym miejscu!");
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Anulowano operacje.");
            }

        }

        public void Search_Data_in_Provider(string valueToSearch)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Search_Data_In_DataGrid_Provider(this, connection, valueToSearch);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        private void TxtSzukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            string valueToSearch = txtSzukaj.Text.ToString();
            Search_Data_in_Provider(valueToSearch);
        }
        private void BtnWyczyscProvider_Click(object sender, RoutedEventArgs e)
        {
            clearTextBox.ClearTextBoxInProvider(this);
        }

        #endregion

        #region invoice
        private void Button_Faktury(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 2;
            try
            {
                connection.Open();
                managementSql.Data_In_DataGrid_Invoice(this, connection);
                connection.Close();
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
        }
        private void BtnDodaj_Invoice_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInInvoice(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (managementSql.Add_to_Invoice(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Invoice(this, connection);
                        MessageBox.Show("Dodano");
                        clearTextBox.ClearTextBoxInInvoice(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie dodano");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void BtnEdytuj_Invoice_Click(object sender, RoutedEventArgs e)
        {
            if (checkIsEmpty.CheckIsEmptyInInvoice(this))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    if (managementSql.Edit_in_Invoice(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Invoice(this, connection);
                        MessageBox.Show("EDYTOWANO");
                        clearTextBox.ClearTextBoxInInvoice(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie edytowano");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.Close();
        }

        private void BtnUsun_Invoice_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Ostrzeżenie!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (managementSql.Delete_in_Invoice(this, connection).ExecuteNonQuery() == 1)
                    {
                        managementSql.Data_In_DataGrid_Invoice(this, connection);
                        MessageBox.Show("Usunięto");
                        clearTextBox.ClearTextBoxInInvoice(this);
                    }
                    else
                    {
                        MessageBox.Show("Nie usunięto");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Anulowano operację.");
            }

        }

        public void SearchDataInvoice(string valueToSearch)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                managementSql.Search_Data_In_DataGrid_Invoice(this, connection, valueToSearch);
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        private void TxtSzukaj_Invoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            string valueToSearch = txtSzukaj_Invoice.Text.ToString();
            SearchDataInvoice(valueToSearch);
        }

        private void DataGridInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem is DataRowView row_selected)
            {
                txtId_invoice.Text = row_selected[0].ToString();
                txtNumer_faktury.Text = row_selected[1].ToString();
                txtWartosc_netto.Text = row_selected[2].ToString();
                txtWartosc_brutto.Text = row_selected[3].ToString();
                string s = row_selected[4].ToString();
                DatePickerInvoice.SelectedDate = DateTime.ParseExact(s, "dd.MM.yyyy", null);
                txtSkrot_faktury.Text = row_selected[5].ToString();
            }
        }

        private void BtnWyczysc_Invoice_Click(object sender, RoutedEventArgs e)
        {
            clearTextBox.ClearTextBoxInInvoice(this);
        }

        #endregion

        #region summary
        private void Button_Zestawienia(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 6;
        }

        private void BtnReportSummary_Click(object sender, RoutedEventArgs e)
        {
            if(DatePickerSummary1.SelectedDate == null || DatePickerSummary2.SelectedDate == null)
            {
                MessageBox.Show("Wybierz zakres dat");
            }
            else if(DatePickerSummary1.SelectedDate >= DatePickerSummary2.SelectedDate)
            {
                MessageBox.Show("Data początkowa musi być niższa niż końcowa");
            }
            else
            {
                Zakladki.SelectedIndex = 7;
                try
                {
                    connection.Open();
                    managementSql.Data_In_DataGrid_ReportSummary(this, connection);
                    connection.Close();
                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }
            }

        }

        private void BtnInvoiceSummary_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerSummary1.SelectedDate == null || DatePickerSummary2.SelectedDate == null)
            {
                MessageBox.Show("Wybierz zakres dat");
            }
            else if (DatePickerSummary1.SelectedDate >= DatePickerSummary2.SelectedDate)
            {
                MessageBox.Show("Data początkowa musi być niższa niż końcowa");
            }
            else
            {
                Zakladki.SelectedIndex = 8;
                try
                {
                    connection.Open();
                    managementSql.Data_In_DataGrid_InvoiceSummary(this, connection);
                    connection.Close();
                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }
            }

        }
        private void BtnBackSummary_Click(object sender, RoutedEventArgs e)
        {
            Zakladki.SelectedIndex = 6;
        }

        private void BtnReportSummaryPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime selectedDate1 = DatePickerSummary1.SelectedDate.Value.Date;
                DateTime selectedDate2 = DatePickerSummary2.SelectedDate.Value.Date;
                SaveFileDialog dlg = new SaveFileDialog();
                string initPath = Path.GetTempPath() + @"\Raport";
                dlg.InitialDirectory = Path.GetFullPath(initPath);
                dlg.RestoreDirectory = true;
                dlg.FileName = "ZestawienieRaportow_" + selectedDate1.Date.ToString("MM-yyyy");
                dlg.DefaultExt = ".pdf";
                dlg.Filter = "Pdf documents (.pdf)|*.pdf";

                connection.Open();
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    string query = "SELECT date_add_inventory AS Data, SUM(Wartosc) AS Wartosc FROM(SELECT card_index.price AS 'Cena jednostki', ROUND(SUB1.expense_total, 2) AS 'Ilosc', ROUND(SUB1.expense_total * card_index.price, 2) AS Wartosc, card_index.file, SUB1.date_add_inventory FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.id_inventory = T2.id_inventory AND t1.date_add_inventory BETWEEN '" + selectedDate1.Date.ToString("yyyy-MM-dd") + "' AND '" + selectedDate2.Date.ToString("yyyy-MM-dd") + "' GROUP BY T1.date_add_inventory)SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id ORDER BY NAME DESC)SUB2 GROUP BY date_add_inventory;";
                    string query2 = "SELECT SUM(Wartosc) AS Wartosc FROM(SELECT card_index.price AS 'Cena jednostki', ROUND(SUB1.expense_total, 2) AS 'Ilosc', ROUND(SUB1.expense_total * card_index.price, 2) AS Wartosc, card_index.file, SUB1.date_add_inventory FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.id_inventory = T2.id_inventory AND t1.date_add_inventory BETWEEN '" + selectedDate1.Date.ToString("yyyy-MM-dd") + "' AND '" + selectedDate2.Date.ToString("yyyy-MM-dd") + "' GROUP BY T1.date_add_inventory)SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id ORDER BY NAME DESC)SUB2;";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    MySqlCommand command2 = new MySqlCommand(query2, connection);
                    MySqlDataReader reader = command2.ExecuteReader();
                    reader.Read();
                    string totall_price = reader[0].ToString();
                    reader.Close();

                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();

                    BaseFont f_arial = BaseFont.CreateFont(@"C:\Windows\Fonts\Arial.ttf", BaseFont.CP1250, true);
                    iTextSharp.text.Font fontArial = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    cb.SetFontAndSize(f_arial, 11);

                    PdfPTable table = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName, fontArial))
                        {
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                            VerticalAlignment = PdfPCell.ALIGN_CENTER,
                            BackgroundColor = new iTextSharp.text.BaseColor(192, 192, 192)
                        };

                        table.AddCell(cell);
                    }

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString(), fontArial))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                                VerticalAlignment = PdfPCell.ALIGN_CENTER
                            };

                            table.AddCell(cell);
                        }
                    }

                    Paragraph p = new Paragraph("Kwota wartosci raportow: " + totall_price, fontArial)

                    {
                        Alignment = Element.ALIGN_RIGHT
                    };

                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Zestawienie raportow zywieniowych z Publicznego Przedszkola w Bierawie " + selectedDate1.Date.ToString("MM.yyyy"), 295, 805, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Podpis intendenta:", 405, 70, 0);
                    cb.SetFontAndSize(f_arial, 5);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Dokument wygenerowany przy pomocy programu gastroEdu", 295, 15, 0);


                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("\n"));
                    document.Add(table);
                    document.Add(p);
                    cb.EndText();
                    document.Close();
                    MessageBox.Show("Utworzono raport.");
                }

            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
            connection.Close();
        }

        #endregion

        #region report
        private void Button_Raporty(object sender, RoutedEventArgs e) => Zakladki.SelectedIndex = 3;

        private void ExportToPDF()
        {
            if (DatePickerReport.SelectedDate == null)
            {
                MessageBox.Show("Wybierz datę!");
            }

            else
            {
                try
                {
                    DateTime selectedDate = DatePickerReport.SelectedDate.Value.Date;
                    SaveFileDialog dlg = new SaveFileDialog();
                    string initPath = Path.GetTempPath() + @"\Raport";
                    dlg.InitialDirectory = Path.GetFullPath(initPath);
                    dlg.RestoreDirectory = true;
                    dlg.FileName = "Raport_" + selectedDate.Date.ToString("dd-MM-yyyy");
                    dlg.DefaultExt = ".pdf";
                    dlg.Filter = "Pdf documents (.pdf)|*.pdf";

                    connection.Open();
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        string filename = dlg.FileName;
                        string query = "SELECT card_index.name AS 'Nazwa produktu', card_index.unit AS 'Jedn.miar', card_index.price AS 'Cena jednostki', ROUND(SUB1.expense_total, 2) AS 'Ilosc', ROUND(SUB1.expense_total * card_index.price, 2) AS 'Wartosc', card_index.file AS 'Nr. kartoteki' FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, T1.id_invoice_index, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.id_inventory = T2.id_inventory AND t1.date_add_inventory = '" + selectedDate.Date.ToString("yyyy-MM-dd") + "' GROUP BY T1.id_ci_index) SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id ORDER BY NAME DESC;";
                        string query2 = "SELECT SUM(Wartosc) AS wartosc1 FROM(SELECT card_index.price AS 'Cena jednostki', ROUND(SUB1.expense_total, 2) AS 'Ilosc', ROUND(SUB1.expense_total * card_index.price, 2) AS Wartosc, card_index.file FROM(SELECT T1.id_inventory, DATE_FORMAT(T1.date_add_inventory, '%d.%m.%Y') AS date_add_inventory, T1.id_ci_index, SUM(T2.expenditure_inventory) AS expense_total FROM inventory T1, inventory T2 WHERE T1.id_inventory = T2.id_inventory AND t1.date_add_inventory = '" + selectedDate.Date.ToString("yyyy-MM-dd") + "' GROUP BY T1.id_ci_index) SUB1 JOIN inventory ON SUB1.id_inventory = inventory.id_inventory INNER JOIN card_index ON inventory.id_ci_index = card_index.id ORDER BY NAME DESC) SUB2;";

                        MySqlCommand command = new MySqlCommand(query, connection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        MySqlCommand command2 = new MySqlCommand(query2, connection);
                        MySqlDataReader reader = command2.ExecuteReader();
                        reader.Read();
                        string totall_price = reader[0].ToString();
                        reader.Close();

                        Document document = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                        document.Open();
                        PdfContentByte cb = writer.DirectContent;
                        cb.BeginText();

                        BaseFont f_arial = BaseFont.CreateFont(@"C:\Windows\Fonts\Arial.ttf", BaseFont.CP1250, true);
                        iTextSharp.text.Font fontArial = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        cb.SetFontAndSize(f_arial, 11);

                        PdfPTable table = new PdfPTable(dataTable.Columns.Count)
                        {
                            WidthPercentage = 100
                        };

                        for (int k = 0; k < dataTable.Columns.Count; k++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName, fontArial))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                                BackgroundColor = new iTextSharp.text.BaseColor(192, 192, 192)
                            };

                            table.AddCell(cell);
                        }

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString(), fontArial))
                                {
                                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                                    VerticalAlignment = PdfPCell.ALIGN_CENTER
                                };

                                table.AddCell(cell);
                            }
                        }

                        Paragraph p = new Paragraph("Kwota wartosci rozchodu: " + totall_price, fontArial)

                        {
                            Alignment = Element.ALIGN_RIGHT
                        };

                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "RAPORT ŻYWIENIOWY", 295, 805, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, selectedDate.Date.ToString("dd.MM.yyyy"), 495, 805, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Podpis dyr. przedszkola:", 100, 70, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Podpis intendenta:", 295, 70, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Podpis kucharki:", 405, 70, 0);
                        cb.SetFontAndSize(f_arial, 5);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Dokument wygenerowany przy pomocy programu gastroEdu", 295, 15, 0);


                        document.Add(new Paragraph("\n"));
                        document.Add(new Paragraph("\n"));
                        document.Add(new Paragraph("\n"));
                        document.Add(table);
                        document.Add(p);
                        cb.EndText();
                        document.Close();
                        MessageBox.Show("Utworzono raport.");
                    }

                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }
            }
            connection.Close();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ExportToPDF();
        }

        #endregion

        #region regex

        private void TxtNr_tel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void TxtNr_konta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void TxtWartosc_netto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[,]{0,1}[0-9]{0,2}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtWartosc_brutto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[,]{0,1}[0-9]{0,2}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtCena_CI_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[,]{0,1}[0-9]{0,2}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtStan_poczatkowy_inv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[.]{0,1}[0-9]{0,3}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtPrzychod_inv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[.]{0,1}[0-9]{0,3}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtRozchod_inv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+[.]{0,1}[0-9]{0,3}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }


        #endregion

        
    }
}
