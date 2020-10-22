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
        public CharacterEditor()
        {
            InitializeComponent();
            AccountUpdate();
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
        }
    }
}
