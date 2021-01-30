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

namespace MuEditor
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class CharacterCreation : Window
    {

        private Account account;
       
        public CharacterCreation(Account accountName)
        {
            InitializeComponent();
            InitCombo();
            this.account = accountName;
        }


        private void InitCombo()
        {
            ClassCombo.Items.Add("Dark Wizard");
            ClassCombo.Items.Add("Dark Knight");
            ClassCombo.Items.Add("Fairy Elf");
            ClassCombo.Items.Add("Magic Gladiator");
            ClassCombo.Items.Add("Dark Lord");
            ClassCombo.Items.Add("Summoner");
            ClassCombo.Items.Add("Rage Fighter");
            ClassCombo.Items.Add("Grow Lancer");

        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            //TODO: Добавить проверку наличия персонажей.
            if (ClassCombo.SelectedItem == null)
                MessageBox.Show("You didn't choose class.", "Mu Editor");
            else if (NameTextBox.Text.Length < 4)
                MessageBox.Show("Check name field", "Mu editor");
            else
            {
                string selected = ClassCombo.SelectedItem.ToString();
                int value = 0;
                switch (selected)
                {
                    case "Dark Wizard":
                        value = 0;
                        break;
                    case "Dark Knight":
                        value = 16;
                        break;
                    case "Fairy Elf":
                        value = 32;
                        break;
                    case "Magic Gladiator":
                        value = 48;
                        break;
                    case "Dark Lord":
                        value = 64;
                        break;
                    case "Summoner":
                        value = 80;
                        break;
                    case "Rage Fighter":
                        value = 96;
                        break;
                    case "Grow Lancer":
                        value = 112;
                        break;
                    default:
                        MessageBox.Show("Error occured. Line 77", "Mu Editor");
                        return;
                }
                //MessageBox.Show("Выбрано: " + selected + "\n" + value);
                DbModel.AddCharacter(account, new Character(NameTextBox.Text, value));
                this.Close();
            }
        }
    }
}
