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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class CharacterEditor : Window
    {

        private int m_QuestData;

        public CharacterEditor()
        {
            InitializeComponent();
            AccountUpdate();
            DisableAll();

            //Class

            classCombo.Items.Add("Dark Wizard"); //0
            classCombo.Items.Add("Soul Master"); //1
            classCombo.Items.Add("Grand Master"); //2
            classCombo.Items.Add("Soul Wizard"); //3
            classCombo.Items.Add("Dark Knight"); //4
            classCombo.Items.Add("Blade Knight"); //5
            classCombo.Items.Add("Blade Master"); //6
            classCombo.Items.Add("Dragon Knight"); //7
            classCombo.Items.Add("Fairy Elf"); //8
            classCombo.Items.Add("Muse Elf"); //9
            classCombo.Items.Add("Hight Elf"); //10
            classCombo.Items.Add("Noble Elf"); //11
            classCombo.Items.Add("Magic Gladiator"); //12
            classCombo.Items.Add("Duel Master"); //13
            classCombo.Items.Add("Magic Knight"); //14
            classCombo.Items.Add("Dark Lord"); //15
            classCombo.Items.Add("Lord Emperor"); //16
            classCombo.Items.Add("Empire Road"); //17
            classCombo.Items.Add("Summoner"); //18
            classCombo.Items.Add("Bloody Summoner"); //19
            classCombo.Items.Add("Dimension Master"); //20
            classCombo.Items.Add("Dimension Summoner"); //21
            classCombo.Items.Add("Rage Fighter"); //22
            classCombo.Items.Add("First Master"); //23
            classCombo.Items.Add("First Blazer"); //24
            classCombo.Items.Add("Grow Lancer"); //25
            classCombo.Items.Add("Mirage Lancer"); //26
            classCombo.Items.Add("Shining Lancer"); //27


            //Quest

            questCombo.Items.Add("None");
            questCombo.Items.Add("Find the 'Scrool of Emperor'");
            questCombo.Items.Add("Three Treasures of Mu");
            questCombo.Items.Add("Gain 'Hero Status");
            questCombo.Items.Add("Secret of 'Dark Stone' (BK)");
            questCombo.Items.Add("Certificate of Strength! (I");
            questCombo.Items.Add("Certificate of Strength! (II)");
            questCombo.Items.Add("Certificate of Strength! (III)");
            questCombo.Items.Add("Infiltration of Barracks of Ballgass!(I)");
            questCombo.Items.Add("Infiltration of Barracks of Ballgass! (II)");
            questCombo.Items.Add("Infiltration of Refuge! (I)");
            questCombo.Items.Add("Infiltration of Refuge!(II)");
            questCombo.Items.Add("Completed 3rd class");

            //Access

            accBox.Items.Add("Normal");
            accBox.Items.Add("Blocked");
            accBox.Items.Add("Items Blocked");
            accBox.Items.Add("Game Master");

            //Status

            statusBox.Items.Add("Hero");
            statusBox.Items.Add("Commoner");
            statusBox.Items.Add("Normal");
            statusBox.Items.Add("Outlaw Warning");
            statusBox.Items.Add("1 Outlaw");
            statusBox.Items.Add("2 Outlaw");

            //Family

            familyCombo.Items.Add("None");
            familyCombo.Items.Add("Duprian");
            familyCombo.Items.Add("Vanert");
        }

        private void DisableAll()
        {
            classCombo.IsEnabled = false;
            lvlBox.IsEnabled = false;
            moneyBox.IsEnabled = false;
            pointsBox.IsEnabled = false;
            expBox.IsEnabled = false;
            strBox.IsEnabled = false;
            dexBox.IsEnabled = false;
            vitBox.IsEnabled = false;
            levelBox.IsEnabled = false;
            enBox.IsEnabled = false;
            comBox.IsEnabled = false;
            questCombo.IsEnabled = false;
            accBox.IsEnabled = false;
            exInvBox.IsEnabled = false;
            chatBox.IsEnabled = false;
            statusBox.IsEnabled = false;
            timeBox.IsEnabled = false;
            killsBox.IsEnabled = false;
            lvlBox.IsEnabled = false;
            expBox1.IsEnabled = false;
            pointsBox1.IsEnabled = false;
            familyCombo.IsEnabled = false;
            rankBox.IsEnabled = false;
            pointsBox3.IsEnabled = false;
        }
        private void EnableAll()
        {
            classCombo.IsEnabled = true;
            lvlBox.IsEnabled = true;
            moneyBox.IsEnabled = true;
            pointsBox.IsEnabled = true;
            levelBox.IsEnabled = true;
            expBox.IsEnabled = true;
            strBox.IsEnabled = true;
            dexBox.IsEnabled = true;
            vitBox.IsEnabled = true;
            enBox.IsEnabled = true;
            comBox.IsEnabled = true;
            questCombo.IsEnabled = true;
            accBox.IsEnabled = true;
            exInvBox.IsEnabled = true;
            chatBox.IsEnabled = true;
            statusBox.IsEnabled = true;
            timeBox.IsEnabled = true;
            killsBox.IsEnabled = true;
            lvlBox.IsEnabled = true;
            expBox1.IsEnabled = true;
            pointsBox1.IsEnabled = true;
            familyCombo.IsEnabled = true;
            rankBox.IsEnabled = true;
            pointsBox3.IsEnabled = true;
        }
        private void AccountUpdate()
        {
            this.characterCombo.Text = "";
            this.characterCombo.Items.Clear();
            this.accountCombo.Text = "";
            this.accountCombo.Items.Clear();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                string account = DbLite.DbU.GetAsString("memb___id");
                // MessageBox.Show(account, "Developer info");
                this.accountCombo.Items.Add((object)account);
            }

            DbLite.DbU.Close();
            
        }

        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.characterCombo.Text = "";
            this.characterCombo.Items.Clear();
            if (this.accountCombo.SelectedItem == null)
                return;

            string nick = accountCombo.SelectedItem.ToString();
            DbLite.Db.Read("select Name from Character where AccountID = '" + nick + "' order by Name");
            while (DbLite.Db.Fetch())
                this.characterCombo.Items.Add((object)DbLite.Db.GetAsString("Name"));
            DbLite.Db.Close();
            DisableAll();
        }

        private void CharacterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableAll();
            InformationLoad();
        }

        public void InformationChecker(ref TextBox a, string line)
        {
            try
            {
                Decimal temp = (Decimal)DbLite.Db.GetAsInteger(line);
                a.Text = temp.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Error in column " + line + " \nValue is: " + (Decimal)DbLite.Db.GetAsInteger(line));
                a.Text = "0";
            }
        }

        public void InformationChecker64(ref TextBox a, string line)
        {
            try
            {
                Decimal temp = (Decimal)DbLite.Db.GetAsInteger64(line);
                a.Text = temp.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Error in column " + line + " \nValue is: " + (Decimal)DbLite.Db.GetAsInteger64(line));
                a.Text = "0";
            }
        }

        private int GetComboIndex(int type, int value)
        {
            switch (type)
            {
                case 1:
                    switch (value)
                    {
                        case 0:
                            return 0;
                            break;
                        case 8:
                            return 1;
                            break;
                        case 12:
                            return 2;
                            break;
                        case 14:
                            return 3;
                            break;
                        case 16:
                            return 4;
                            break;
                        case 24:
                            return 5;
                            break;
                        case 28:
                            return 6;
                            break;
                        case 30:
                            return 7;
                            break;
                        case 32:
                            return 8;
                            break;
                        case 40:
                            return 9;
                            break;
                        case 44:
                            return 10;
                            break;
                        case 46:
                            return 11;
                            break;
                        case 48:
                            return 12;
                            break;
                        case 60:
                            return 13;
                            break;
                        case 62:
                            return 14;
                            break;
                        case 64:
                            return 15;
                            break;
                        case 76:
                            return 16;
                            break;
                        case 78:
                            return 17;
                            break;
                        case 80:
                            return 18;
                            break;
                        case 88:
                            return 19;
                            break;
                        case 92:
                            return 20;
                            break;
                        case 94:
                            return 21;
                            break;
                        case 96:
                            return 22;
                            break;
                        case 108:
                            return 23;
                            break;
                        case 110:
                            return 24;
                            break;
                        case 112:
                            return 25;
                            break;
                        case 124:
                            return 26;
                            break;
                        case 126:
                            return 27;
                            break;
                    }
                    break;
                case 2:
                    switch (value)
                    {
                        case 0:
                            return 0;
                            break;
                        case 1:
                            return 8;
                            break;
                        case 2:
                            return 12;
                            break;
                        case 3:
                            return 14;
                            break;
                        case 4:
                            return 16;
                            break;
                        case 5:
                            return 24;
                            break;
                        case 6:
                            return 28;
                            break;
                        case 7:
                            return 30;
                            break;
                        case 8:
                            return 32;
                            break;
                        case 9:
                            return 40;
                            break;
                        case 10:
                            return 44;
                            break;
                        case 11:
                            return 46;
                            break;
                        case 12:
                            return 48;
                            break;
                        case 13:
                            return 60;
                            break;
                        case 14:
                            return 62;
                            break;
                        case 15:
                            return 64;
                            break;
                        case 16:
                            return 76;
                            break;
                        case 17:
                            return 78;
                            break;
                        case 18:
                            return 80;
                            break;
                        case 19:
                            return 88;
                            break;
                        case 20:
                            return 92;
                            break;
                        case 21:
                            return 94;
                            break;
                        case 22:
                            return 96;
                            break;
                        case 23:
                            return 108;
                            break;
                        case 24:
                            return 110;
                            break;
                        case 25:
                            return 112;
                            break;
                        case 26:
                            return 124;
                            break;
                        case 27:
                            return 126;
                            break;
                    }
                    break;
                case 3:
                    return int.MaxValue;
                    break;
            }
            return int.MaxValue;
        }



        private int GetQuestIndex(int par)
        {
            switch (par)
            {
                case int.MaxValue:
                    return 0;
                case 254:
                    return 1;
                case 250:
                    return 2;
                case 234:
                    return 3;
                case 170:
                    return 4;
                case 176:
                    return 5;
                case 174:
                    return 6;
                case 178:
                    return 7;
                case 179:
                    return 8;
                case 180:
                    return 9;
                case 181:
                    return 10;
                case 182:
                    return 11;
                case 186:
                    return 12;
                default:
                    return int.MaxValue;
            }
        }

        public int getStatusIndex(int type, int value)
        {
            switch (type)
            {
                case 1:
                    switch (value)
                    {
                        case 0:
                            return 0;
                        case 1:
                            return 1;
                        case 42:
                            return 3;
                        default:
                            return int.MaxValue;
                    }
                case 2:
                    switch (value)
                    {
                        case 0:
                            return 0;
                        case 1:
                            return 1;
                        case 2:
                            return 1;
                        case 3:
                            return 42;
                        default:
                            return int.MaxValue;
                    }
                default:
                    return int.MaxValue;
            }
        }
        public void InformationLoad()
        {
            if (characterCombo.SelectedItem == null)  //Зачем оно тут нужно -  хЗ
                return;
            DbLite.Db.Read("select * from Character where Name = '" + this.characterCombo.Text + "'");
            DbLite.Db.Fetch();

            int ingameClass = DbLite.Db.GetAsInteger("Class");
            int clt = DbLite.Db.GetAsInteger("CtlCode");
            accBox.SelectedIndex = getStatusIndex(1, clt);
            classCombo.SelectedIndex = GetComboIndex(1, ingameClass);

            InformationChecker(ref this.levelBox, "cLevel");
            //     InformationChecker(ref this.numericUpDown2, "Resets");
            InformationChecker(ref this.pointsBox3, "LevelUpPoint");
            InformationChecker64(ref this.expBox, "Experience");
            InformationChecker(ref this.strBox, "Strength");
            InformationChecker(ref this.dexBox, "Dexterity");
            InformationChecker(ref this.vitBox, "Vitality");
            InformationChecker(ref this.enBox, "Energy");
            InformationChecker(ref this.comBox, "Leadership");
            InformationChecker(ref this.exInvBox, "ExtInventory");
            InformationChecker(ref this.chatBox, "ChatLimitTime");
            InformationChecker(ref this.moneyBox, "Money");
            //    InformationChecker(ref this.numericUpDown20, "Ruud");

           this.statusBox.SelectedIndex = DbLite.Db.GetAsInteger("PkLevel") - 1;
            if (this.statusBox.SelectedIndex < 0)
                this.statusBox.SelectedIndex = 0;
            if (this.statusBox.SelectedIndex > 5)
                this.statusBox.SelectedIndex = 5;
            InformationChecker(ref this.timeBox, "PkCount");
            InformationChecker(ref this.killsBox, "PkTime");
            this.m_QuestData = DbLite.Db.GetAsInteger("Quest");
            DbLite.Db.Close();
            this.questCombo.SelectedIndex = m_QuestData;
            DbLite.Db.Read("select * from Gens_Rank where Name = '" + this.characterCombo.Text + "'");
            DbLite.Db.Fetch();
            this.familyCombo.SelectedIndex = DbLite.Db.GetAsInteger("Family");
            this.pointsBox3.Text = DbLite.Db.GetAsInteger("Contribution").ToString();
            DbLite.Db.Close();
            DbLite.Db.Read("select * from MasterSkillTree where Name = '" + this.characterCombo.Text + "'");
            DbLite.Db.Fetch();
            this.lvlBox.Text = DbLite.Db.GetAsInteger("MasterLevel").ToString();
            this.expBox1.Text = DbLite.Db.GetAsInteger64("MasterExperience").ToString();
            this.pointsBox1.Text = DbLite.Db.GetAsInteger("MasterPoint").ToString();
            DbLite.Db.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.characterCombo.Text == "")
                MessageBox.Show("Choose character to edit first");
            DbLite.Db.Exec("UPDATE dbo.Character set cLevel = " + this.lvlBox.Text +
                ", Class = " + GetComboIndex(2, classCombo.SelectedIndex) +
                ", CtlCode = " + getStatusIndex(2, accBox.SelectedIndex) +
                //", Resets = " + (object)this.numericUpDown2.Value + 
                ", LevelUpPoint = " + pointsBox.Text +
                ", Experience = " + expBox.Text + ", Strength = " + strBox.Text +
                ", Dexterity = " + dexBox.Text + ", Vitality = " + vitBox.Text +
                ", Energy = " + enBox.Text + ", Leadership = " + comBox.Text +
                ", ExtInventory = " + exInvBox.Text +
                ", ChatLimitTime = " + chatBox.Text +
                ", Money = " + moneyBox.Text + //", Ruud = " + (object)this.numericUpDown20.Value + 
                ", PkLevel = " + (statusBox.Text + 1) + ", PkCount = " + killsBox.Text +
                ", PkTime = " + timeBox.Text);
            DbLite.Db.Close();

        }
    }
}
