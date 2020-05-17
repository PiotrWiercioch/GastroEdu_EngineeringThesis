using System;

namespace Praca_InzynierskaV2
{
    class CheckIsEmpty
    {
        public bool CheckIsEmptyInProvider(MainWindow window)
        {
            #region zmienne do sprawdzania czy pusty textbox

            bool isEmptyNazwa = String.IsNullOrWhiteSpace(window.txtNazwa.Text);
            bool isEmptyNip = String.IsNullOrWhiteSpace(window.txtNip.Text);
            bool isEmptyMiejscowosc = String.IsNullOrWhiteSpace(window.txtMiejscowosc.Text);
            bool isEmptyUlica = String.IsNullOrWhiteSpace(window.txtUlica.Text);
            bool isEmptyNr_domu = String.IsNullOrWhiteSpace(window.txtNr_domu.Text);
            bool isEmptyNr_tel = String.IsNullOrWhiteSpace(window.txtNr_tel.Text);
            bool isEmptyNr_konta = String.IsNullOrWhiteSpace(window.txtNr_konta.Text);

            #endregion

            bool checkIsEmpty = (isEmptyNazwa || isEmptyNip || isEmptyMiejscowosc || isEmptyUlica || isEmptyNr_domu || isEmptyNr_tel || isEmptyNr_konta) ? true : false;
            return checkIsEmpty;
        }

        public bool CheckIsEmptyInCardIndex(MainWindow window)
        {
            #region zmienne do sprawdzania czy pusty textbox

            bool isEmptyIKartoteka = String.IsNullOrWhiteSpace(window.txtIKartoteki_CI.Text);
            bool isEmptyNazwa = String.IsNullOrWhiteSpace(window.txtNazwa_CI.Text);
            bool isEmptyCena = String.IsNullOrWhiteSpace(window.txtCena_CI.Text);
            bool isEmptyJMiary = String.IsNullOrWhiteSpace(window.cbbJMiary_CI.Text);
            bool isEmptyProvider = String.IsNullOrWhiteSpace(window.txt_id_cbbSupp.Text);
            bool isEmpty_Start_quantity = String.IsNullOrWhiteSpace(window.txtStan_poczatkowy_inv.Text);

            #endregion

            bool checkIsEmpty = (isEmptyIKartoteka || isEmptyNazwa || isEmptyCena || isEmptyJMiary || isEmptyProvider || isEmpty_Start_quantity) ? true : false;
            return checkIsEmpty;
        }

        public bool CheckIsEmptyInInventory(MainWindow window)
        {
            #region zmienne do sprawdzania czy pusty textbox

            bool isEmptyProdukt = String.IsNullOrWhiteSpace(window.cbbFile.Text);
            bool isEmptySkrotFaktury = String.IsNullOrWhiteSpace(window.cbb_shortcut_name_invoice.Text);

            bool isEmptyPrzychod = String.IsNullOrWhiteSpace(window.txtPrzychod_inv.Text);
            bool isEmptyRozchod = String.IsNullOrWhiteSpace(window.txtRozchod_inv.Text);

            #endregion

            if(isEmptyPrzychod)
            {
                window.txtPrzychod_inv.Text = "0";
            }
            if (isEmptyRozchod)
            {
                window.txtRozchod_inv.Text = "0";
            }

            bool checkIsEmpty = (isEmptyProdukt || isEmptySkrotFaktury) ? true : false;
            return checkIsEmpty;
        }

        public bool CheckIsEmptyInInvoice(MainWindow window)
        {
            #region zmienne do sprawdzania czy pusty textbox

            bool isEmptyNumerFaktury = String.IsNullOrWhiteSpace(window.txtNumer_faktury.Text);
            bool isEmptyWartoscNetto = String.IsNullOrWhiteSpace(window.txtWartosc_netto.Text);
            bool isEmptyWartoscBrutto = String.IsNullOrWhiteSpace(window.txtWartosc_brutto.Text);
            bool isEmptySkrotFaktury = String.IsNullOrWhiteSpace(window.txtSkrot_faktury.Text);

            #endregion

            bool checkIsEmpty = (isEmptyNumerFaktury || isEmptyWartoscNetto || isEmptyWartoscBrutto || isEmptySkrotFaktury) ? true : false;
            return checkIsEmpty;
        }

        public bool CheckIsEmptyInInfoScreen(InfoScreen window)
        {
            #region zmienne do sprawdzania czy pusty textbox

            bool isEmptyUsername = String.IsNullOrWhiteSpace(window.txtUsername.Text);
            bool isEmptyPassword = String.IsNullOrWhiteSpace(window.txtPassword.Password);
            bool isEmptyRepeatPassword = String.IsNullOrWhiteSpace(window.txtRepeatPassword.Password);
            bool isEmptyUser_type = String.IsNullOrWhiteSpace(window.cbb_user_type.Text);

            #endregion

            bool checkIsEmpty = (isEmptyUsername || isEmptyPassword || isEmptyUser_type || isEmptyRepeatPassword) ? true : false;
            return checkIsEmpty;
        }
    }
}
