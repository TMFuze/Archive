using Archive.AppFiles;
using Archive.Pages.ArchivariusPages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Archive.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxbLog.Text == null ||
                    TxbLog.Text == ""||
                    PsbPass.Password == null ||
                    PsbPass.Password == "")
                {
                    MessageBox.Show("Заполните все строки!",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                }
                else
                {
                    var userObj = DBConnect.entities.User.FirstOrDefault(x => x.Name == TxbLog.Text && x.Password == PsbPass.Password);
                    if (userObj == null)
                    {
                        MessageBox.Show("Такой пользователь не найден",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    }
                    else
                    {
                        switch (userObj.IdRole)
                        {
                            case 1:
                                MessageBox.Show("Здравствуйте пользователь " + userObj.FIO + "!",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                UserArchFiles welcomePage = new UserArchFiles(userObj.Id);
                                FrameApp.frmObj.Navigate(welcomePage);
                                

                                break;
                            case 2:
                                MessageBox.Show("Здравствуйте архивариус " + userObj.FIO + "!",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                FrameApp.frmObj.Navigate(new Pages.ArchivariusPages.ArchFilesPage());

                                break;
                            case 3:
                                MessageBox.Show("Здравствуйте администратор " + userObj.FIO + "!",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                /*FrameApp.frmObj.Navigate(new Pages.ArchiveData());*/
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критический сбой в работе приложения: " + ex.Message.ToString(),
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

            }
        }
    }
}
