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

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            int num1 = DbLite.DbU.ExecWithResult("select count(*) from MEMB_INFO where memb___id = '" + this.AccountTextBox.Text + "'");
            if (this.AccountTextBox.Text.Length < 2)
            {
                int num2 = (int)MessageBox.Show("Could not create an account, check name field.", "Mu Editor");
                DbLite.DbU.Close();
            }
            else if (this.PasswordTextBox.Text.Length < 2)
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
                bool a = DbLite.DbU.Exec("insert into MEMB_INFO (memb___id,memb__pwd,memb_name,sno__numb,mail_addr,fpas_ques,fpas_answ,appl_days,modi_days,out__days,true_days,mail_chek,bloc_code,ctl1_code) values ('" + this.AccountTextBox.Text + "','" + this.PasswordTextBox.Text + "','','1','" + this.EmailTextBox.Text + "','','','20140101','20140101','20140101','20140101','1','0','0')");
                DbLite.DbU.Close();
                if (a)
                    MessageBox.Show("Account was created.", "Mu Editor");
                else
                    MessageBox.Show("Could not create an account!\n[Exception]\n" + DbLite.Db.ExError.Message, "Mu Editor");
            }
            Account_Reload();
        }

        public void Account_Reload()
        {
            this.CharacterComboBox.Text = "";
            this.CharacterComboBox.Items.Clear();
            this.AccountComboBox.Text = "";
            this.AccountComboBox.Items.Clear();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                string account = DbLite.DbU.GetAsString("memb___id");
               // MessageBox.Show(account, "Developer info");
                this.AccountComboBox.Items.Add((object)account);
            }
                
            DbLite.DbU.Close();
        }

        public void Character_Reload()
        {
 
            this.CharacterComboBox.Text = "";
            this.CharacterComboBox.Items.Clear();
            if (this.AccountComboBox.SelectedItem == null)
                return;

            string nick = AccountComboBox.SelectedItem.ToString();
            DbLite.Db.Read("select Name from Character where AccountID = '" + nick + "' order by Name");
            while (DbLite.Db.Fetch())
                this.CharacterComboBox.Items.Add((object)DbLite.Db.GetAsString("Name"));
            DbLite.Db.Close();
        }


        private void InformationReload()
        {
            if (!(AccountComboBox.SelectedItem == null))
            {
                string nick = AccountComboBox.SelectedItem.ToString();
                DbLite.DbU.Read("select memb_guid, memb__pwd,mail_addr,sno__numb from MEMB_INFO where memb___id = '" + nick + "'");
                DbLite.DbU.Fetch();
                this.InformationPasswordTextBox.Text = DbLite.DbU.GetAsString("memb__pwd");
                this.InformationEmailTextBox.Text = DbLite.DbU.GetAsString("mail_addr");
                this.InformationIdTextBox.Text = DbLite.DbU.GetAsString("sno__numb");
                //this.MemberGuid = DBLite.dbMe.GetAsInteger("memb_guid");
            }
        }
        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Character_Reload();
            InformationReload();
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.AccountComboBox.Text != "") || this.CharacterComboBox.SelectedItem == null)
                return;

            string account = AccountComboBox.SelectedItem.ToString();
            DbLite.DbU.Exec("delete from MEMB_INFO where memb___id = '" + account + "'");
            DbLite.DbU.Close();
            DbLite.Db.Exec("delete from Character where AccountID = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from AccountCharacter where Id = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from warehouse where AccountID = '" + account + "'");
            DbLite.Db.Close();
            this.AccountComboBox.Items.Remove(this.AccountComboBox.SelectedItem);
            this.AccountComboBox.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(this.CharacterComboBox.Text != "") || this.CharacterComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please choose what to delete first.");
                return;
            }
            DbLite.Db.Exec("delete from Character where Name = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from Ertel_Inventory where UserName = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from GensMainInfo where memb_char = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from GuildMatching_OfferList where Master = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from GuildMatching_RequestList where Sender = '" + this.CharacterComboBox.Text + "' OR Recipient = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from GuildMatching_RequestList where Sender = '" + this.CharacterComboBox.Text + "' OR Recipient = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            for (int index = 1; index <= 5; ++index)
            {
                DbLite.Db.Exec("update AccountCharacter set GameID" + (object)index + "=NULL where GameID" + (object)index + " = '" + this.CharacterComboBox.Text + "'");
                DbLite.Db.Close();
            }
            DbLite.Db.Exec("update AccountCharacter set GameIDC=NULL where GameIDC = '" + this.CharacterComboBox.Text + "'");
            DbLite.Db.Close();
            this.CharacterComboBox.Items.Remove(this.CharacterComboBox.SelectedItem);
            this.CharacterComboBox.Text = "";
        }

        private void CharacterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (AccountComboBox.SelectedItem == null)
                MessageBox.Show("Choose what to modify first.", "Mu Editor");
            else
            {
                AccountCreation window1 = new AccountCreation(AccountComboBox.SelectedItem.ToString());
                window1.Show();
            }
                

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
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

        /*public void AccountInformation_Load()
        {
            {
                DbLite.Db.Read("select memb_guid, memb__pwd,mail_addr,sno__numb from MEMB_INFO where memb___id = '" + this.comboBox1.Text + "'");
                DbLite.Db.Fetch();
                this.textBox7.Text = DBLite.dbMe.GetAsString("memb__pwd");
                this.textBox6.Text = DBLite.dbMe.GetAsString("mail_addr");
                this.textBox5.Text = DBLite.dbMe.GetAsString("sno__numb");
                this.MemberGuid = DBLite.dbMe.GetAsInteger("memb_guid");
            }
            DbLite.Db.Close();
            DbLite.Db.Read("select * from MEMB_STAT where memb___id = '" + this.comboBox1.Text + "'");
            DbLite.Db.Fetch();
            if (DbLite.Db.GetAsInteger("ConnectStat") == 1)
            {
                this.label18.Text = "ONLINE";
                this.label18.ForeColor = Color.Green;
            }
            else
            {
                this.label18.Text = "OFFLINE";
                this.label18.ForeColor = Color.Red;
            }
            this.textBox8.Text = DbLite.Db.GetAsString("ServerName");
            this.textBox9.Text = DbLite.Db.GetAsString("IP");
            this.textBox11.Text = DbLite.Db.GetAsString("ConnectTM");
            this.textBox10.Text = DbLite.Db.GetAsString("DisConnectTM");
            DBLite.dbMu.Close();
            DBLite.dbMu.Read("select WCoinC, WCoinP, GoblinPoint from T_InGameShop_Point where AccountID = " + (object)this.MemberGuid);
            DBLite.dbMu.Fetch();
            this.numericUpDown1.Value = (Decimal)DbLite.Db.GetAsInteger("WCoinC");
            this.numericUpDown2.Value = (Decimal)DbLite.Db.GetAsInteger("WCoinP");
            this.numericUpDown3.Value = (Decimal)DbLite.Db.GetAsInteger("GoblinPoint");
            DbLite.Db.Close();
        }*/
    }
}
