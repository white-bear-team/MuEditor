using MuEditor.Utils.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuEditor
{
    /// <summary>
    /// Логика взаимодействия для AccountManager.xaml
    /// </summary>
    public partial class AccountManager : Window
    {
        private Account SelectedAccount => (Account)this.AccountComboBox.SelectedItem;
        private Character SelectedCharacter => (Character)this.CharacterComboBox.SelectedItem;

        private string connectionString; //temporary does nothing
        public AccountManager()
        {
            InitializeComponent();
            Account_Reload();
        }

        public AccountManager(string connectionString) //samething
        {
            InitializeComponent();
            Account_Reload();
            this.connectionString = connectionString;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            DbModel.AddAccount(new Account(AccountTextBox.Text, PasswordTextBox.Text, EmailTextBox.Text, PersonalIdTextBox.Text));
            Account_Reload();
        }

        public void Account_Reload()
        {
            this.CharacterComboBox.Text = "";
            this.CharacterComboBox.Items.Clear();
            this.AccountComboBox.Text = "";
            this.AccountComboBox.Items.Clear();
            foreach (Account account in DbModel.GetAccounts())
            {
                this.AccountComboBox.Items.Add((object)account);
            }
        }

        public void Character_Reload()
        {
            this.CharacterComboBox.Text = "";
            this.CharacterComboBox.Items.Clear();

            if (SelectedAccount == null)
                return;

            foreach (Character character in DbModel.GetCharacters(SelectedAccount.Name))
            {
                this.CharacterComboBox.Items.Add((object)character);
            }
        }


        private void InformationReload()
        {
            if (!(AccountComboBox.SelectedItem == null))
            {
                this.InformationPasswordTextBox.Text = SelectedAccount.Password;
                this.InformationEmailTextBox.Text = SelectedAccount.Email;
                this.InformationIdTextBox.Text = SelectedAccount.Id;
            }
        }

        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Character_Reload();
            InformationReload();
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedAccount != null)
            {
                DbModel.RemoveAccount(SelectedAccount.Name);
            }
            Account_Reload();
           
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedCharacter != null)
            {
                DbModel.RemoveCharacter(SelectedCharacter);
            }
            Character_Reload();
        }

        private void CharacterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewButtonClick(object sender, RoutedEventArgs e)
        {
            if (AccountComboBox.SelectedItem == null)
                MessageBox.Show("Choose what to modify first.", "Mu Editor");
            else
            {
                CharacterCreation window1 = new CharacterCreation(SelectedAccount);
                window1.Show();
            }
                

        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (AccountComboBox.SelectedItem == null || CharacterComboBox.SelectedItem == null)
                return;
            //TODO: OpenWindow
            CharacterEditor characterEditor = new CharacterEditor();
            characterEditor.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
