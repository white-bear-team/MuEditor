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
using MuEditor.Utils.Account;

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

        private List<IdWithNameModel> characterClass = new List<IdWithNameModel>
        {
            new IdWithNameModel(0, "Dark Wizard"), //0
            new IdWithNameModel(8, "Soul Master"), //1
            new IdWithNameModel(12, "Grand Master"), //2
            new IdWithNameModel(14, "Sould Wizard"), //3

            new IdWithNameModel(16, "Dark Knight"), //4
            new IdWithNameModel(24, "Blade Knight"), //5
            new IdWithNameModel(28, "Blade Master"), //6
            new IdWithNameModel(30, "Dark Knight"), //7

            new IdWithNameModel(32, "Fairy Elf"), //8
            new IdWithNameModel(40, "Muse Elf"), //9
            new IdWithNameModel(44, "Hight Elf"), //10
            new IdWithNameModel(46, "Noble Elf"), //11

            new IdWithNameModel(48, "Magic Gladiator"), //12
            new IdWithNameModel(60, "Duel Master"), //13
            new IdWithNameModel(62, "Magic Knight"), //14

            new IdWithNameModel(64, "Dark Lord"), //15
            new IdWithNameModel(76, "Lord Emperor"), //16
            new IdWithNameModel(78, "Empire Road"), //17
            
            new IdWithNameModel(80, "Summoner"), //18
            new IdWithNameModel(88, "Bloody Summoner"), //19
            new IdWithNameModel(92, "Dimension Master"), //20
            new IdWithNameModel(94, "Dimension Summoner"), //21

            new IdWithNameModel(96, "Rage Fighter"), //22
            new IdWithNameModel(108, "First Master"), //23
            new IdWithNameModel(110, "First Blazer"), //24

            new IdWithNameModel(112, "Grow Lancer"), //25
            new IdWithNameModel(124, "Mirage Lancer"), //26
            new IdWithNameModel(126, "Shining Lancer") //27
        };

        private List<IdWithNameModel> characterFamily = new List<IdWithNameModel>
        {
            new IdWithNameModel(0, "None"),
            new IdWithNameModel(1, "Duprian"),
            new IdWithNameModel(2, "Vanert")
        };

        private Account SelectedAccount => (Account) this.accountComboBox.SelectedItem;

        private Character SelectedCharacter => (Character) this.characterComboBox.SelectedItem;
        
        public CharacterEditor()
        {
            InitializeComponent();
            ReloadAccounts();
            SetupUI();
        }

        public CharacterEditor(string accountName, string characterName)
        {
            InitializeComponent();
            ReloadAccounts();
            SelectAccountByName(accountName);
            SelectCharacterByName(characterName);
            SetupUI();
            EnableAllUI();
            LoadInformation();
        }

        private void SetupUI()
        {
            DisableAllUI();

            //Class
            characterClass.ForEach(model => classComboBox.Items.Add(model));

            //Quest
            characterQuests.ForEach(model => questComboBox.Items.Add(model));

            //Access
            characterStatuses.ForEach(model => characterStatusComboBox.Items.Add(model));

            //Status
            pkStatuses.ForEach(model => pkStatusComboBox.Items.Add(model));

            //Family
            characterFamily.ForEach(model => gensComboBox.Items.Add(model));
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

        private void ReloadAccounts()
        {
            this.characterComboBox.Text = "";
            this.characterComboBox.Items.Clear();
            this.accountComboBox.Text = "";
            this.accountComboBox.Items.Clear();
            foreach(Account account in DbModel.GetAccounts())
            {
                this.accountComboBox.Items.Add((object)account);
            }
        }

        private void SelectAccountByName(string name)
        {
            foreach (var item in accountComboBox.Items)
            {
                if (((Account) item).Name.Equals(name))
                {
                    accountComboBox.SelectedItem = item;
                    return;
                }
            }
        }

        private void SelectCharacterByName(string name)
        {
            foreach (var item in characterComboBox.Items)
            {
                if (((Character) item).Name.Equals(name))
                {
                    characterComboBox.SelectedItem = item;
                    return;
                }
            }
        }

        private void AccountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.characterComboBox.Text = "";
            this.characterComboBox.Items.Clear();
            
            if (SelectedAccount == null)
                return;
            
            foreach(Character character in DbModel.GetCharacters(SelectedAccount.Name))
            {
                this.characterComboBox.Items.Add((object)character);
            }
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

        public void LoadInformation()
        {
            if (SelectedCharacter == null)
                return;

            DbLite.Db.Read($"select * from Character where Name = '{SelectedCharacter.Name }'");
            DbLite.Db.Fetch();

            int classDb = DbLite.Db.GetAsInteger("Class");
            int ctlCodeDb = DbLite.Db.GetAsInteger("CtlCode");


            // this.characterStatusComboBox.SelectedIndex = GetStatusIndex(1, clt);

            var selectedCharacterStatus = characterStatuses.FirstOrDefault(model => model.GetId() == ctlCodeDb);
            this.characterStatusComboBox.SelectedItem = selectedCharacterStatus;

            var selectedClass = characterClass.FirstOrDefault(model => model.GetId() == classDb);
            this.classComboBox.SelectedItem = selectedClass;

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


            DbLite.Db.Read("select * from Gens_Rank where Name = '" + SelectedCharacter.Name + "'");
            DbLite.Db.Fetch();
            this.gensComboBox.SelectedIndex = DbLite.Db.GetAsInteger("Family");
            this.gensContributionTextBox.Text = DbLite.Db.GetAsInteger("Contribution").ToString();
            DbLite.Db.Close();


            /*GensRankDb.Open();
            this.gensComboBox.SelectedIndex = GensRankDb.GetFamily();
            this.gensComboBox.SelectedIndex = GensRankDb.GetContribution();
            GensRankDb.Close();
            */


            DbLite.Db.Read("select * from MasterSkillTree where Name = '" + SelectedCharacter.Name + "'");
            DbLite.Db.Fetch();
            this.masterLevelTextBox.Text = DbLite.Db.GetAsInteger("MasterLevel").ToString();
            this.masterExpTextBox.Text = DbLite.Db.GetAsInteger64("MasterExperience").ToString();
            this.masterPointsTextBox.Text = DbLite.Db.GetAsInteger("MasterPoint").ToString();
            DbLite.Db.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCharacter.Name == null)
            {
                MessageBox.Show("Choose character to edit first");
                return;
            }

            ((CharacterQuestModel)questComboBox.SelectedItem).UpdateTypeIntoVariable(ref this.questData);
            string questString = "0x" + BitConverter.ToString(this.questData).Replace("-", "");

            DbLite.Db.Exec("UPDATE dbo.Character set cLevel = " + this.masterLevelTextBox.Text +
                           ", Class = " + ((IdWithNameModel)classComboBox.SelectedItem).GetId() +
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
                           ", PkTime = " + pkTimeTextBox.Text + " where Name = '" + SelectedCharacter.Name + "'");
            DbLite.Db.Close();
        }
    }
}