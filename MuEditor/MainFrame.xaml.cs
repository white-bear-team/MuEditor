using IniParser;
using IniParser.Model;
using MuEditor.Manager;
using MuEditor.SqlLog;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {

        bool updated = false;

        public MainFrame()
        {
            InitializeComponent();
            FileWork();
            SelectLastDataBase();
        }

        private void FileWork()
        {

            /*
            var parser = new FileIniDataParser();

            IniData data = new IniData();
            data.Sections.AddSection("newSection");
            data["newSection"].AddKey("newKey1", "value1");
            data["newSection2"].AddKey("newKey1", "value1");
            parser.WriteFile("newINIfile.ini", data);
            IniData data1 = parser.ReadFile("newINIfile.ini");
            foreach (var section in data1.Sections)
            {
                MessageBox.Show(section.SectionName);
            }
            */

            if (!File.Exists("db.ini"))
            {
                MessageBox.Show("Файл не был обнаружен, начинаем процедуру");
                File.Create("db.ini");
            }

            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("db.ini");
                foreach (var section in data.Sections)
                {
                    DatabaseComboBox.Items.Add(section.SectionName);
                }
            }catch(Exception e)
            {
                MessageBox.Show("Could not work with INI file\n[Exception]\n" + e.Message, "Mu Editor");
            }
        
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (updated)
            {
                AccountManager accountManager = new AccountManager();
                accountManager.Show();
            }
            else
            {
                MessageBox.Show("First click on update database to set a connection", "Mu Editor");
            }
        }

        private String GenerateConnectionString(string dataSource, string initialCatalog, string username, string password)
        {
            string connectionString = ("Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + initialCatalog + ";UID=" + username + ";PWD=" + password + ";");
            return connectionString;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Add new database button
            CreateNewDatabase createNewDatabase = new CreateNewDatabase();
            createNewDatabase.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DbComboUpdate();
        }

        private void DbComboUpdate()
        {
            try
            {
                DatabaseComboBox.Items.Clear();
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("db.ini");
                foreach (var section in data.Sections)
                {
                    DatabaseComboBox.Items.Add(section.SectionName);
                }
            }catch(Exception e)
            {
                MessageBox.Show("Could not work with INI file\n[Exception]\n" + e.Message, "Mu Editor");
            }
        }

        private void UpdateLabels_Click(object sender, RoutedEventArgs e)
        {
            UpdateUIOnDatabaseSelected();
        }

        private void UpdateUIOnDatabaseSelected()
        {
            try
            {
                if (DatabaseComboBox.SelectedItem == null)
                {
                    MessageBox.Show("You have to select database to use first", "Mu Editor");
                    return;
                }

                var parser = new FileIniDataParser();
                IniData data = new IniData();
                data = parser.ReadFile("db.ini");
                KeyDataCollection keyCol = data[DatabaseComboBox.SelectedItem.ToString()];
                DbLite.Db.connect(GenerateConnectionString(keyCol["mainHost"], keyCol["mainCatalog"], keyCol["mainUsername"],
                    keyCol["mainPassword"]));
                DbLite.DbU.connect(GenerateConnectionString(keyCol["userHost"], keyCol["userCatalog"], keyCol["userUsername"],
                    keyCol["userPassword"]));
                AccountCountLabel.Content = DbLite.DbU.ExecWithResult("select count(*) from MEMB_INFO").ToString();
                CharacterCountLabel.Content = DbLite.Db.ExecWithResult("select count(*) from Character").ToString();
                DatabaseNameLabel.Content = DatabaseComboBox.SelectedItem.ToString();
                updated = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not connect to database OR open ini file (Look in exception)\n[Exception]\n" + ex.Message,
                    "Mu Editor");
                updated = false;
            }
        }

        private void DeleteDbButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("db.ini");
                data.Sections.RemoveSection(DatabaseComboBox.SelectedItem.ToString());
                parser.WriteFile("db.ini", data);
                DbComboUpdate();
                updated = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not proceed removal of DB\n[Exception]\n" + ex.Message, "Mu Editor");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Character click manager
            if (DatabaseComboBox.SelectedItem == null)
            {
                MessageBox.Show("You have to select database to use first", "Mu Editor");
                return;
            }
            CharacterEditor characterEditor = new CharacterEditor();
            characterEditor.Show();
        }

        private void dbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUIOnDatabaseSelected();
            ResetLastDatabase();
            WriteLastDatabase();
        }

        private void SelectLastDataBase()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("db.ini");
            foreach (var section in data.Sections)
            {
                if (data[section.SectionName]["last"] == "true") 
                    DatabaseComboBox.SelectedItem = (section.SectionName);
            }
        }

        private void ResetLastDatabase()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("db.ini");
            foreach (var section in data.Sections)
            {
                data[section.SectionName]["last"] = "false";
            }
            
            parser.WriteFile("db.ini", data);
        }

        private void WriteLastDatabase()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("db.ini");
            data[DatabaseComboBox.SelectedItem.ToString()]["last"] = "true";
            parser.WriteFile("db.ini", data);
        }

        private void SqlButton_Click(object sender, RoutedEventArgs e)
        {
            SqlLogWindow sqlLogWindow = new SqlLogWindow();
            sqlLogWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
        }
    }
}
