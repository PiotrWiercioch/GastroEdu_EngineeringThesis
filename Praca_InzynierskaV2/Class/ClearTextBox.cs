namespace Praca_InzynierskaV2
{
    class ClearTextBox
    {
        public void ClearTextBoxInProvider(MainWindow window)
        {
            window.txtId.Clear();
            window.txtNazwa.Clear();
            window.txtNip.Clear();
            window.txtMiejscowosc.Clear();
            window.txtUlica.Clear();
            window.txtNr_domu.Clear();
            window.txtNr_tel.Clear();
            window.txtNr_konta.Clear();
            window.DataGridProvider.UnselectAll();
        }

        public void ClearTextBoxInCardIndex(MainWindow window)
        {
            window.txtId_CI.Clear();
            window.txtIKartoteki_CI.Clear();
            window.txtNazwa_CI.Clear();
            window.txtCena_CI.Clear();
            window.cbbJMiary_CI.SelectedIndex = -1;
            window.cbbProvider.SelectedIndex = -1;
            window.txtStan_poczatkowy_inv.Clear();
            window.DataGridCardIndex.UnselectAll();
        }

        public void ClearTextBoxInInventory(MainWindow window)
        {
            window.cbbFile.SelectedIndex = -1;
            window.txtPrzychod_inv.Clear();
            window.txtRozchod_inv.Clear();
            window.txtTotall_quantity.Text = "0";
            window.AutomaticDateRadioButton.IsChecked = true;
            window.cbb_shortcut_name_invoice.SelectedIndex = -1;
            window.DataGridInventory.UnselectAll();
        }

        public void ClearTextBoxInInvoice(MainWindow window)
        {
            window.txtId_invoice.Clear();
            window.txtNumer_faktury.Clear();
            window.txtWartosc_netto.Clear();
            window.txtWartosc_brutto.Clear();
            window.DatePickerInvoice.SelectedDate = null;
            window.txtSkrot_faktury.Clear();
            window.DataGridInvoice.UnselectAll();
        }

        public void ClearTextBoxInInfoScreen(InfoScreen window)
        {
            window.txtUsername.Clear();
            window.txtPassword.Clear();
            window.txtRepeatPassword.Clear();
            window.cbb_user_type.SelectedIndex = -1;
        }
    }
}
