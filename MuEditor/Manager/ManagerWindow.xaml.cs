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

namespace MuEditor.Manager
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        bool isInformationShown = false;

        private Account SelectedAccount => (Account) this.AccountListView.SelectedItem;

        private Character SelectedCharacter => (Character) this.CharacterListView.SelectedItem;
        
        public ManagerWindow()
        {
            InitializeComponent();
            var accountListViewGrid = new GridView();
            this.AccountListView.View = accountListViewGrid;
            accountListViewGrid.Columns.Add(new GridViewColumn
            {
                Header = "Account Name",
                DisplayMemberBinding = new Binding("Name")
            });
            accountListViewGrid.Columns.Add(new GridViewColumn
            {
                Header = "Online status",
                DisplayMemberBinding = new Binding("Online")
            });
            var characterListViewGrid = new GridView();
            this.CharacterListView.View = characterListViewGrid;
            characterListViewGrid.Columns.Add(new GridViewColumn
            {
                Header = "Character Name",
                DisplayMemberBinding = new Binding("Name")
            });
            this.Height = 430;
            this.Width = 767;
            UpdateAccountListView();
            AddButton.IsEnabled = false;
            AddButton.Visibility = Visibility.Hidden;

        }

        public void UpdateAccountListView()
        {
            AccountListView.Items.Clear();
            CharacterListView.Items.Clear();
            foreach(Account account in DbModel.GetAccounts())
            {
                this.AccountListView.Items.Add(account);
            }
        }

        public void UpdateCharacterListView()
        {
            if (SelectedAccount == null)
                return;
            
            CharacterListView.Items.Clear();
            foreach(Character character in DbModel.GetCharacters(SelectedAccount.Name))
            {
                this.CharacterListView.Items.Add(character);
            }
        }

        public void UpdateAccountInformation()
        {
            if (SelectedAccount == null) return;
            
            var updatedAccount = DbModel.GetAccount(SelectedAccount.Name);
            this.InformationNameTextBox.Text = updatedAccount.Name;
            this.InformationPasswordTextBox.Text = updatedAccount.Password;
            this.InformationEmailTextBox.Text = updatedAccount.Email;
            this.InformationIdTextBox.Text = updatedAccount.Id;
        }

        public void HideSaveButton()
        {
            SaveButton.Visibility = Visibility.Hidden;
            SaveButton.IsEnabled = false;
        }

        public void HideInformationFrame()
        {
            this.Height = 430;
            this.Width = 767;
            isInformationShown = false;
        }
        public void ShowSaveButton()
        {
            SaveButton.Visibility = Visibility.Visible;
            SaveButton.IsEnabled = true;
        }

        public void ClearInformationTextBox()
        {
            InformationEmailTextBox.Text = "";
            InformationNameTextBox.Text = "";
            InformationPasswordTextBox.Text = "";
            InformationIdTextBox.Text = "";
        }

        public void HideAddButton()
        {
            AddButton.Visibility = Visibility.Hidden;
            AddButton.IsEnabled = false;
        }

        public void ShowAddButton()
        {
            AddButton.Visibility = Visibility.Visible;
            AddButton.IsEnabled = true;
        }

        private void AccountListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CharacterListView.Items.Clear();
            UpdateCharacterListView();
            
            if (isInformationShown)
                UpdateAccountInformation();
        }

        private void CharacterListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CharacterEditor characterEditor = new CharacterEditor(DbModel.GetAccountByCharacterName(SelectedCharacter).Name, SelectedCharacter.Name);
            characterEditor.Show();
        }

        private void AccountListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Height = 432;
            this.Width = 1090;
            isInformationShown = true;
            UpdateAccountInformation();
        }

        private void RemoveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AccountListView.SelectedItem == null)
                return;

            try
            {
                Account account = (Account) this.AccountListView.SelectedItem;
                DbModel.RemoveAccount(account.Name);
            }catch(Exception)
            {
                MessageBox.Show("Exception", "Mu Editor");
            }
            
            UpdateAccountListView();
        }

        private void AddNewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            HideSaveButton();
            ShowAddButton();
            ClearInformationTextBox();
            InformationNameTextBox.IsEnabled = true;
            MessageBox.Show("Enter account information to the fields appeared", "Mu Editor");
            if (!isInformationShown)
            {
                this.Height = 432;
                this.Width = 1090;
            }

        }

        private void RemoveCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            if(CharacterListView.SelectedItem == null)
            {
                MessageBox.Show("Choose character first");
                return;
            }
            DbModel.RemoveCharacter(((Character)CharacterListView.SelectedItem));
            UpdateCharacterListView();
        }

        private void AddCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountListView.SelectedItem == null)
                MessageBox.Show("Choose what to modify first.", "Mu Editor");
            else
            {
                AccountCreation window1 = new AccountCreation(((Account)AccountListView.SelectedItem).Name);
                window1.Show();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newAccount = new Account(InformationNameTextBox.Text, InformationPasswordTextBox.Text, InformationEmailTextBox.Text);
            MessageBox.Show(DbModel.AddAccount(newAccount));
            UpdateAccountListView();
            HideAddButton();
            ShowSaveButton();
            HideInformationFrame();
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            HideInformationFrame();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.AccountListView.Items.Clear();
            foreach (string account in DbModel.SearchAccounts(this.SearchTextBox.Text))
            {
                this.AccountListView.Items.Add(new Account { Name = account });
            }
            this.CharacterListView.Items.Clear();
            foreach(string account in DbModel.SearchCharacters(this.SearchTextBox.Text))
            {
                this.CharacterListView.Items.Add(new Character { Name = account });
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAccountListView();
        }
    }
}