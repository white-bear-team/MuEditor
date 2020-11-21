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
        public ManagerWindow()
        {
            InitializeComponent();
            var accoutListViewGrid = new GridView();
            this.AccountListView.View = accoutListViewGrid;
            accoutListViewGrid.Columns.Add(new GridViewColumn
            {
                Header = "Account Name",
                DisplayMemberBinding = new Binding("Name")
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
                this.AccountListView.Items.Add(new ListItem { Name = account.AccountName });
            }
        }

        public void UpdateCharacterListView()
        {
            if (this.AccountListView.SelectedItem == null)
                return;
            CharacterListView.Items.Clear();
            foreach(Character character in DbModel.GetCharacters(GetSelectedAccountName()))
            {
                this.CharacterListView.Items.Add(new ListItem { Name = character.CharacterName });
            }
        }
            
        public void UpdateAccountInformation()
        {
            if (!(AccountListView.SelectedItem == null))
            {
                Account account = DbModel.GetAccount(GetSelectedAccountName());
                this.InformationNameTextBox.IsEnabled = false; //Чтобы не изменяли ник
                this.InformationNameTextBox.Text = account.AccountName;
                this.InformationPasswordTextBox.Text = account.AccountPassword;
                this.InformationEmailTextBox.Text = account.Email;
                this.InformationIdTextBox.Text = account.Id;
            }
        }

        public string GetSelectedAccountName()
        {
            return ((ListItem)AccountListView.SelectedItem).Name;
        }

        public string GetSelectedCharacterName()
        {
            return ((ListItem)CharacterListView.SelectedItem).Name;
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
            CharacterEditor characterEditor = new CharacterEditor(GetSelectedAccountName(), GetSelectedCharacterName());
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
                DbModel.RemoveAccount(GetSelectedAccountName());
            }catch(Exception ex)
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

        }

        private void AddCharacterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(DbModel.AddAccount(new Account(InformationNameTextBox.Text, InformationPasswordTextBox.Text, InformationEmailTextBox.Text)));
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
                this.AccountListView.Items.Add(new ListItem { Name = account });
            }
            this.CharacterListView.Items.Clear();
            foreach(string account in DbModel.SearchCharacters(this.SearchTextBox.Text))
            {
                this.CharacterListView.Items.Add(new ListItem { Name = account });
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAccountListView();
        }
    }
}