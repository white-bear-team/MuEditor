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
                    if (section.SectionName == DatabaseNameTextBox.Text)
                    {
                        MessageBox.Show("Данная база данных уже присутствует в конфигурационном файле", "Mu Editor");
                        return;
                    }
                }
                data.Sections.AddSection(DatabaseNameTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("mainHost", MainHostTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("mainCatalog", MainCatalogTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("mainUsername", MainUsernameTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("mainPassword", MainPasswordTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("userHost", UsersHostTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("userCatalog", UsersCatalogTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("userPassword", UsersPasswordTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("userUsername", UsersUsernameTextBox.Text);
                data[DatabaseNameTextBox.Text].AddKey("last", "true");
                parser.WriteFile("db.ini", data);
            }catch(Exception ex)
            {
                MessageBox.Show("Не удалось записать в файл.\n[Ошибка]\n" + ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UsersHostTextBox.Text = MainHostTextBox.Text;
            UsersCatalogTextBox.Text = MainCatalogTextBox.Text;
            UsersUsernameTextBox.Text = MainUsernameTextBox.Text;
            UsersPasswordTextBox.Text = MainPasswordTextBox.Text;
            MessageBox.Show("Скопировано", "Mu Editor");
        }
    }
}
