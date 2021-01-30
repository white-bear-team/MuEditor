using IniParser;
using IniParser.Model;
using MuEditor.Manager;
using MuEditor.Misc;
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

namespace MuEditor.MainWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool updated = false;

        public MainWindow()
        {
            InitializeComponent();
            FileWork();
            SelectLastDataBase();
        }

        private void FileWork()
        {
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not work with INI file\n[Exception]\n" + e.Message, "Mu Editor");
            }

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
            if (DatabaseComboBox.SelectedItem != null)
                UpdateUIOnDatabaseSelected();
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not work with INI file\n[Exception]\n" + e.Message, "Mu Editor");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewDatabase createNewDatabase = new CreateNewDatabase();
            createNewDatabase.Show();
        }

        private String GenerateConnectionString(string dataSource, string initialCatalog, string username, string password)
        {
            string connectionString = ("Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + initialCatalog + ";UID=" + username + ";PWD=" + password + ";");
            return connectionString;
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {
                MessageBox.Show("Could not proceed removal of DB\n[Exception]\n" + ex.Message, "Mu Editor");
            }
        }

        private void DatabaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatabaseComboBox.SelectedItem != null) //Проверка, чтобы не было Exception
            {
                ResetLastDatabase();
                WriteLastDatabase();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DbComboUpdate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void AccountManagerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AccountManager accountManager = new AccountManager();
            accountManager.Show();
        }

        private void CharacterManagerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CharacterEditor characterEditor = new CharacterEditor();
            characterEditor.Show();
        }

        private void FindMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
        }

        private void SqlLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SqlLogWindow sqlLogWindow = new SqlLogWindow();
            sqlLogWindow.Show();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ApplicationClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
