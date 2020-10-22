using IniParser;
using IniParser.Model;
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
    /// Логика взаимодействия для CreateNewDatabase.xaml
    /// </summary>
    public partial class CreateNewDatabase : Window
    {
        public CreateNewDatabase()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("db.ini");
                foreach (var section in data.Sections)
                {
                    if (section.SectionName == dbName.Text)
                    {
                        MessageBox.Show("Данная база данных уже присутствует в конфигурационном файле", "Mu Editor");
                        return;
                    }
                }
                data.Sections.AddSection(dbName.Text);
                data[dbName.Text].AddKey("mainHost", mainHost.Text);
                data[dbName.Text].AddKey("mainCatalog", mainCatalog.Text);
                data[dbName.Text].AddKey("mainUsername", mainUsername.Text);
                data[dbName.Text].AddKey("mainPassword", mainPassword.Text);
                data[dbName.Text].AddKey("userHost", usersHost.Text);
                data[dbName.Text].AddKey("userCatalog", usersCatalog.Text);
                data[dbName.Text].AddKey("userPassword", usersPassword.Text);
                data[dbName.Text].AddKey("userUsername", usersUsername.Text);
                parser.WriteFile("db.ini", data);
            }catch(Exception ex)
            {
                MessageBox.Show("Не удалось записать в файл.\n[Ошибка]\n" + ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            usersHost.Text = mainHost.Text;
            usersCatalog.Text = mainCatalog.Text;
            usersUsername.Text = mainUsername.Text;
            usersPassword.Text = mainPassword.Text;
            MessageBox.Show("Скопировано", "Mu Editor");
        }
    }
}
