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
using MuEditor.Utils;

namespace MuEditor
{
    public partial class CharacterEditor : Window
    {
        private byte[] questData;

        private const int MAX_COUNT = 10;

        private List<IdWithNameModel> characterStatuses = new List<IdWithNameModel>
        {
            new IdWithNameModel(0, "Normal"),
            new IdWithNameModel(1, "Blocked"),
            // new IdWithNameModel(8, "Item Blocked"),
            new IdWithNameModel(32, "Game Master")
        };

        private List<CharacterQuestModel> characterQuests = new List<CharacterQuestModel>
        {
            new CharacterQuestModel(CharacterQuestModel.CharacterQuestType.NO_QUEST),
            new CharacterQuestModel(CharacterQuestModel.CharacterQuestType.COMPLETE_2),
            new CharacterQuestModel(CharacterQuestModel.CharacterQuestType.COMPLETE_3),
            new CharacterQuestModel(CharacterQuestModel.CharacterQuestType.COMPLETE_4)
        };

        private List<IdWithNameModel> pkStatuses = new List<IdWithNameModel>
        {
            new IdWithNameModel(1, "Hero"),
            new IdWithNameModel(2, "Commoner"),
            new IdWithNameModel(3, "Normal"),
            new IdWithNameModel(4, "Outlaw Warning"),
            new IdWithNameModel(5, "1 Outlaw"),
            new IdWithNameModel(6, "2 Outlaw")
        };

        public CharacterEditor()
        {
            InitializeComponent();
            UpdateAccount();
            SetupUI();
        }

        public CharacterEditor(string accountName, string characterName)
        {
            InitializeComponent();
            UpdateAccount();
            accountComboBox.SelectedItem = (object)accountName;
            characterComboBox.SelectedItem = (object)characterName;
            LoadInformation();
        }

        private void SetupUI()
        {
            DisableAllUI();

            //Class

            classComboBox.Items.Add("Dark Wizard"); //0
            classComboBox.Items.Add("Soul Master"); //1
            classComboBox.Items.Add("Grand Master"); //2
            classComboBox.Items.Add("Soul Wizard"); //3

            classComboBox.Items.Add("Dark Knight"); //4
            classComboBox.Items.Add("Blade Knight"); //5
            classComboBox.Items.Add("Blade Master"); //6
            classComboBox.Items.Add("Dragon Knight"); //7

            classComboBox.Items.Add("Fairy Elf"); //8
            classComboBox.Items.Add("Muse Elf"); //9
            classComboBox.Items.Add("Hight Elf"); //10
            classComboBox.Items.Add("Noble Elf"); //11

            classComboBox.Items.Add("Magic Gladiator"); //12
            classComboBox.Items.Add("Duel Master"); //13
            classComboBox.Items.Add("Magic Knight"); //14

            classComboBox.Items.Add("Dark Lord"); //15
            classComboBox.Items.Add("Lord Emperor"); //16
            classComboBox.Items.Add("Empire Road"); //17

            classComboBox.Items.Add("Summoner"); //18
            classComboBox.Items.Add("Bloody Summoner"); //19
            classComboBox.Items.Add("Dimension Master"); //20
            classComboBox.Items.Add("Dimension Summoner"); //21

            classComboBox.Items.Add("Rage Fighter"); //22
            classComboBox.Items.Add("First Master"); //23
            classComboBox.Items.Add("First Blazer"); //24

            classComboBox.Items.Add("Grow Lancer"); //25
            classComboBox.Items.Add("Mirage Lancer"); //26
            classComboBox.Items.Add("Shining Lancer"); //27


            //Quest
            characterQuests.ForEach(model => questComboBox.Items.Add(model));

            //Access
            characterStatuses.ForEach(model => characterStatusComboBox.Items.Add(model));

            //Status
            pkStatuses.ForEach(model => pkStatusComboBox.Items.Add(model));

            //Family

            gensComboBox.Items.Add("None");
            gensComboBox.Items.Add("Duprian");
            gensComboBox.Items.Add("Vanert");
        }
        private void DisableAllUI()
        {
            classComboBox.IsEnabled = false;
            masterLevelTextBox.IsEnabled = false;
            moneyTextBox.IsEnabled = false;
            pointsTextBox.IsEnabled = false;
            expTextBox.IsEnabled = false;
            strTextBox.IsEnabled = false;
            dexTextBox.IsEnabled = false;
            vitTextBox.IsEnabled = false;
            levelTextBox.IsEnabled = false;
            eneTextBox.IsEnabled = false;
            comTextBox.IsEnabled = false;
            questComboBox.IsEnabled = false;
            characterStatusComboBox.IsEnabled = false;
            extInventoryTextBox.IsEnabled = false;
            chatLimitTimeTextBox.IsEnabled = false;
            pkStatusComboBox.IsEnabled = false;
            pkTimeTextBox.IsEnabled = false;
            pkKillsCountTextBox.IsEnabled = false;
            masterLevelTextBox.IsEnabled = false;
            masterExpTextBox.IsEnabled = false;
            masterPointsTextBox.IsEnabled = false;
            gensComboBox.IsEnabled = false;
            rankBox.IsEnabled = false;
            gensContributionTextBox.IsEnabled = false;
        }

        private void EnableAllUI()
        {
            classComboBox.IsEnabled = true;
            masterLevelTextBox.IsEnabled = true;
            moneyTextBox.IsEnabled = true;
            pointsTextBox.IsEnabled = true;
            levelTextBox.IsEnabled = true;
            expTextBox.IsEnabled = true;
            strTextBox.IsEnabled = true;
            dexTextBox.IsEnabled = true;
            vitTextBox.IsEnabled = true;
            eneTextBox.IsEnabled = true;
            comTextBox.IsEnabled = true;
            questComboBox.IsEnabled = true;
            characterStatusComboBox.IsEnabled = true;
            extInventoryTextBox.IsEnabled = true;
            chatLimitTimeTextBox.IsEnabled = true;
            pkStatusComboBox.IsEnabled = true;
            pkTimeTextBox.IsEnabled = true;
            pkKillsCountTextBox.IsEnabled = true;
            masterLevelTextBox.IsEnabled = true;
            masterExpTextBox.IsEnabled = true;
            masterPointsTextBox.IsEnabled = true;
            gensComboBox.IsEnabled = true;
            rankBox.IsEnabled = true;
            gensContributionTextBox.IsEnabled = true;
        }

        private void UpdateAccount()
        {
            this.characterComboBox.Text = "";
            this.characterComboBox.Items.Clear();
            this.accountComboBox.Text = "";
            this.accountComboBox.Items.Clear();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                string account = DbLite.DbU.GetAsString("memb___id");
                // MessageBox.Show(account, "Developer info");
                this.accountComboBox.Items.Add((object) account);
            }

            DbLite.DbU.Close();
        }

        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.characterComboBox.Text = "";
            this.characterComboBox.Items.Clear();
            if (this.accountComboBox.SelectedItem == null)
                return;

            string nick = accountComboBox.SelectedItem.ToString();
            DbLite.Db.Read("select Name from Character where AccountID = '" + nick + "' order by Name");
            while (DbLite.Db.Fetch())
                this.characterComboBox.Items.Add((object) DbLite.Db.GetAsString("Name"));
            DbLite.Db.Close();
            DisableAllUI();
        }

        private void CharacterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableAllUI();
            LoadInformation();
        }

        public string GetDataFromColumn(string columnName)
        {
            return DbLite.Db.GetAsInteger(columnName).ToString();
        }

        public string InformationChecker64(string columnName)
        {
            return DbLite.Db.GetAsInteger64(columnName).ToString();
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

        public void LoadInformation()
        {
            if (characterComboBox.SelectedItem == null) // Проверка на "выбор аккаунта"
                return;

            DbLite.Db.Read($"select * from Character where Name = '{this.characterComboBox.SelectedItem }'");
            DbLite.Db.Fetch();

            int classDb = DbLite.Db.GetAsInteger("Class");
            int ctlCodeDb = DbLite.Db.GetAsInteger("CtlCode");


            // this.characterStatusComboBox.SelectedIndex = GetStatusIndex(1, clt);

            var selectedCharacterStatus = characterStatuses.FirstOrDefault(model => model.GetId() == ctlCodeDb);
            this.characterStatusComboBox.SelectedItem = selectedCharacterStatus;


            this.classComboBox.SelectedIndex = GetComboIndex(1, classDb);

            this.levelTextBox.Text = GetDataFromColumn("cLevel");
            //     GetDataFromColumn(this.numericUpDown2, "Resets");

            this.pointsTextBox.Text = GetDataFromColumn("LevelUpPoint");
            this.expTextBox.Text = InformationChecker64("Experience");
            this.strTextBox.Text = GetDataFromColumn("Strength");
            this.dexTextBox.Text = GetDataFromColumn("Dexterity");
            this.vitTextBox.Text = GetDataFromColumn("Vitality");
            this.eneTextBox.Text = GetDataFromColumn("Energy");
            this.comTextBox.Text = GetDataFromColumn("Leadership");
            this.extInventoryTextBox.Text = GetDataFromColumn("ExtInventory");
            this.chatLimitTimeTextBox.Text = GetDataFromColumn("ChatLimitTime");
            this.moneyTextBox.Text = GetDataFromColumn("Money");
            //    GetDataFromColumn(numericUpDown20, "Ruud");

            var pkStatusDb = DbLite.Db.GetAsInteger("PkLevel");
            var selectedPkStatus = pkStatuses.FirstOrDefault(model => model.GetId() == pkStatusDb);
            this.pkStatusComboBox.SelectedItem = selectedPkStatus;


            this.pkKillsCountTextBox.Text = GetDataFromColumn("PkCount");
            this.pkTimeTextBox.Text = GetDataFromColumn("PkTime");


            // 

            // TODO: Закончить квесты
            this.questData = DbLite.Db.GetAsBinary("Quest");
            DbLite.Db.Close();

            var questType = CharacterQuestModel.GetCharacterQuestType(questData);
            var selectedQuest = characterQuests.FirstOrDefault(model => model.questType == questType);
            this.questComboBox.SelectedItem = selectedQuest;


            DbLite.Db.Read("select * from Gens_Rank where Name = '" + this.characterComboBox.SelectedItem + "'");
            DbLite.Db.Fetch();
            this.gensComboBox.SelectedIndex = DbLite.Db.GetAsInteger("Family");
            this.gensContributionTextBox.Text = DbLite.Db.GetAsInteger("Contribution").ToString();
            DbLite.Db.Close();


            /*GensRankDb.Open();
            this.gensComboBox.SelectedIndex = GensRankDb.GetFamily();
            this.gensComboBox.SelectedIndex = GensRankDb.GetContribution();
            GensRankDb.Close();
            */


            DbLite.Db.Read("select * from MasterSkillTree where Name = '" + this.characterComboBox.SelectedItem + "'");
            DbLite.Db.Fetch();
            this.masterLevelTextBox.Text = DbLite.Db.GetAsInteger("MasterLevel").ToString();
            this.masterExpTextBox.Text = DbLite.Db.GetAsInteger64("MasterExperience").ToString();
            this.masterPointsTextBox.Text = DbLite.Db.GetAsInteger("MasterPoint").ToString();
            DbLite.Db.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.characterComboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose character to edit first");
                return;
            }

            ((CharacterQuestModel)questComboBox.SelectedItem).UpdateTypeIntoVariable(ref this.questData);
            string questString = "0x" + BitConverter.ToString(this.questData).Replace("-", "");

            DbLite.Db.Exec("UPDATE dbo.Character set cLevel = " + this.masterLevelTextBox.Text +
                           ", Class = " + GetComboIndex(2, classComboBox.SelectedIndex) +
                           ", CtlCode = " + ((IdWithNameModel) characterStatusComboBox.SelectionBoxItem).GetId() +
                           //", Resets = " + (object)this.numericUpDown2.Value + 
                           ", LevelUpPoint = " + pointsTextBox.Text +
                           ", Experience = " + expTextBox.Text + ", Strength = " + strTextBox.Text +
                           ", Dexterity = " + dexTextBox.Text + ", Vitality = " + vitTextBox.Text +
                           ", Energy = " + eneTextBox.Text + ", Leadership = " + comTextBox.Text +
                           ", ExtInventory = " + extInventoryTextBox.Text +
                           ", ChatLimitTime = " + chatLimitTimeTextBox.Text +
                           ", Money = " + moneyTextBox.Text + //", Ruud = " + (object)this.numericUpDown20.Value + 
                           ", PkLevel = " + ((IdWithNameModel)pkStatusComboBox.SelectedItem).GetId() + ", PkCount = " + pkKillsCountTextBox.Text +
                           ", Quest = " + questString +
                           ", PkTime = " + pkTimeTextBox.Text + " where Name = '" + this.characterComboBox.SelectedItem + "'");
            DbLite.Db.Close();
        }
    }
}