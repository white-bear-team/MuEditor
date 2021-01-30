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

        private string connectionString; //temporary does nothing

        private bool isCreating = true;

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
            if (isCreating)
            {
                MessageBox.Show(DbModel.AddAccount(new Account(AccountTextBox.Text, PasswordTextBox.Text, EmailTextBox.Text, PersonalIdTextBox.Text)));
                Account_Reload();
                MessageBox.Show("Creating...");
                isCreating = false;
                return;
            }
            else
            {
                DbModel.SaveAccountEdit(AccountTextBox.Text, PersonalIdTextBox.Text, EmailTextBox.Text, PasswordTextBox.Text);
                MessageBox.Show("Saved!");
                Account_Reload();
                return;
            }
        }

        public void Account_Reload()
        {
            this.AccountComboBox.Text = "";
            this.AccountComboBox.Items.Clear();
            foreach (Account account in DbModel.GetAccounts())
            {
                this.AccountComboBox.Items.Add((object)account);
            }
        }

        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowInformation();
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedAccount != null)
            {
                DbModel.RemoveAccount(SelectedAccount.Name);
            }
            Account_Reload();
           
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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            isCreating = true;
            AccountTextBox.IsEnabled = true;
            GroupNameLabel.Content = "Create account";
            ActionButton.Content = "Create account";
            AccountTextBox.Text = "";
            PersonalIdTextBox.Text = "";
            EmailTextBox.Text = "";
            PasswordTextBox.Text = "";
        }

        private void ShowInformation()
        {
            if(SelectedAccount != null)
            {
                isCreating = false;
                GroupNameLabel.Content = "Account Information";
                ActionButton.Content = "Apply edit";
                AccountTextBox.Text = SelectedAccount.Name;
                PersonalIdTextBox.Text = SelectedAccount.Id;
                EmailTextBox.Text = SelectedAccount.Email;
                PasswordTextBox.Text = SelectedAccount.Password;
                AccountTextBox.IsEnabled = false;
                IsOnlineLabel.Content = SelectedAccount.Online;
                if (SelectedAccount.Online == "OFFLINE")
                    IsOnlineLabel.Foreground = Brushes.Red;
                else if ((SelectedAccount.Online == "ONLINE"))
                    IsOnlineLabel.Foreground = Brushes.Green;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Account_Reload();
        }

        private void DissconnectButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Why is this not working omg");
        }
    }
}
