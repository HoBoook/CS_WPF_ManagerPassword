using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;

namespace CS_WPF_ManagerPassword
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ItemRecord> listRecords = new List<ItemRecord> { };
        private string nameLocalFile = "Records.json";
        private int indexEdit = -1;
        public MainWindow()
        {
            InitializeComponent();
            LoadRecords();
            RefreshItemsComboBox();
            dataRecords.ItemsSource = listRecords;
        }

        private void RefreshItemsComboBox()
        {
            foreach (ItemRecord record in listRecords)
            {
                if (!comboBoxResource.Items.Contains(record.Resource))
                {
                    comboBoxResource.Items.Add(record.Resource);
                }
            }
        }
        private void LoadRecords()
        {
            if (File.Exists(nameLocalFile) == false) return;
            using (StreamReader sr = new StreamReader(nameLocalFile))
            {
                List<ItemRecord>? temp = JsonConvert.DeserializeObject<List<ItemRecord>>(sr.ReadToEnd());
                if (temp != null)
                {
                    listRecords = temp;
                }
                else
                {
                    MessageBox.Show($"Error read {nameLocalFile}", "Error");
                }
            }
            dataRecords.Items.Refresh();
        }
        private void SaveRecords()
        {
            string json = JsonConvert.SerializeObject(listRecords);
            using (StreamWriter sw = new StreamWriter(nameLocalFile))
            {
                sw.Write(json);
            }
        }

        private string Crypt()
        {
            return "";
        }
        private string Decrypt()
        {
            return "";
        }
        private int GetIndexItem(string login, string resource)
        {
            return listRecords.FindIndex(x => x.Login == login && x.Resource == resource);
        }

        private void buttonAddSave_Click(object sender, RoutedEventArgs e)
        {
            string newLogin = textBoxLogin.Text;
            string newPassword = passBoxPassword.Password;
            string newResource = comboBoxResource.Text;

            listRecords[indexEdit].Login = newLogin;
            listRecords[indexEdit].Resource = newResource;
            listRecords[indexEdit].Password = newPassword;
            indexEdit = -1;
            buttonAdd.Content = "Add";
            buttonAdd.Click -= buttonAddSave_Click;
            buttonAdd.Click += buttonAdd_Click;

            dataRecords.Items.Refresh();
        }

    private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            string newLogin = textBoxLogin.Text;
            string newPassword = passBoxPassword.Password;
            string newResource = comboBoxResource.Text;

            

            int index = GetIndexItem(newLogin, newResource);
            if (index == -1)
            {
                listRecords.Add(new ItemRecord(newLogin, newPassword, newResource));
                if (!comboBoxResource.Items.Contains(newResource))
                {
                    comboBoxResource.Items.Add(newResource);
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want replace data \n{newResource}?",
                    "Replace Item?",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    listRecords[index].Login = newLogin;
                    listRecords[index].Resource = newResource;
                    listRecords[index].Password = newPassword;
                }
            }
            dataRecords.Items.Refresh();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveRecords();
        }


        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataRecords.SelectedIndex != -1)
            {
                listRecords.Remove(listRecords[dataRecords.SelectedIndex]);
                dataRecords.Items.Refresh();
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            int index = dataRecords.SelectedIndex;
            if (index != -1)
            {
                textBoxLogin.Text = listRecords[index].Login;
                passBoxPassword.Password = listRecords[index].Password;
                comboBoxResource.Text = listRecords[index].Resource;
                buttonAdd.Content = "Save";
                indexEdit = index;
                buttonAdd.Click -= buttonAdd_Click;
                buttonAdd.Click += buttonAddSave_Click;
            }
        }
    }
}