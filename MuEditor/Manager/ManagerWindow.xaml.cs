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
            CharacterListView.Items.Clear();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                string account = DbLite.DbU.GetAsString("memb___id");
                // MessageBox.Show(account, "Developer info");
                this.AccountListView.Items.Add(new ListItem { Name = account });
            }

            DbLite.DbU.Close();
        }

        public void UpdateCharacterListView()
        {
            if (this.AccountListView.SelectedItem == null)
                return;
            ListItem selected = (ListItem) AccountListView.SelectedItem;
            DbLite.Db.Read("select Name from Character where AccountID = '" + selected.Name + "' order by Name");
            while (DbLite.Db.Fetch())
                this.CharacterListView.Items.Add(new ListItem { Name = DbLite.Db.GetAsString("Name") });
            DbLite.Db.Close();
        }
            
        public void UpdateAccountInformation()
        {
            if (!(AccountListView.SelectedItem == null))
            {
                string nick = GetSelectedAccountName();
                DbLite.DbU.Read("select memb_guid, memb__pwd,mail_addr,sno__numb from MEMB_INFO where memb___id = '" + nick + "'");
                DbLite.DbU.Fetch();
                this.InformationNameTextBox.IsEnabled = false; //Чтобы не изменяли ник
                this.InformationNameTextBox.Text = GetSelectedAccountName();
                this.InformationPasswordTextBox.Text = DbLite.DbU.GetAsString("memb__pwd");
                this.InformationEmailTextBox.Text = DbLite.DbU.GetAsString("mail_addr");
                this.InformationIdTextBox.Text = DbLite.DbU.GetAsString("sno__numb");
                //this.MemberGuid = DBLite.dbMe.GetAsInteger("memb_guid");
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

            string account = ((ListItem)AccountListView.SelectedItem).Name;
            DbLite.DbU.Exec("delete from MEMB_INFO where memb___id = '" + account + "'");
            DbLite.DbU.Close();
            DbLite.Db.Exec("delete from Character where AccountID = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from AccountCharacter where Id = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from warehouse where AccountID = '" + account + "'");
            DbLite.Db.Close();
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
            int num1 = DbLite.DbU.ExecWithResult("select count(*) from MEMB_INFO where memb___id = '" + this.InformationNameTextBox.Text + "'");
            if (this.InformationNameTextBox.Text.Length < 2)
            {
                int num2 = (int)MessageBox.Show("Could not create an account, check name field.", "Mu Editor");
                DbLite.DbU.Close();
            }
            else if (this.InformationPasswordTextBox.Text.Length < 2)
            {
                int num2 = (int)MessageBox.Show("Could not create an account, check password field.", "Mu Editor");
                DbLite.DbU.Close();
            }
            else if (num1 > 0)
            {
                int num2 = (int)MessageBox.Show("Could not create account because the name is occupied.", "Mu Editor");
                DbLite.DbU.Close();
            }
            else
            {
                DbLite.DbU.Close();
                bool a = DbLite.DbU.Exec("insert into MEMB_INFO (memb___id,memb__pwd,memb_name,sno__numb,mail_addr,fpas_ques,fpas_answ,appl_days,modi_days,out__days,true_days,mail_chek,bloc_code,ctl1_code) values ('" + this.InformationNameTextBox.Text + "','" + this.InformationPasswordTextBox.Text + "','','1','" + this.InformationEmailTextBox.Text + "','','','20140101','20140101','20140101','20140101','1','0','0')");
                DbLite.DbU.Close();
                if (a)
                    MessageBox.Show("Account was created.", "Mu Editor");
                else
                    MessageBox.Show("Could not create an account!\n[Exception]\n" + DbLite.Db.ExError.Message, "Mu Editor");
            }
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
                MessageBox.Show(account);
                this.AccountListView.Items.Add(new ListItem { Name = account });
            }
            this.CharacterListView.Items.Clear();
            foreach(string account in DbModel.SearchCharacters(this.SearchTextBox.Text))
            {
                MessageBox.Show(account);
                this.CharacterListView.Items.Add(new ListItem { Name = account });
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAccountListView();
        }
    }
}