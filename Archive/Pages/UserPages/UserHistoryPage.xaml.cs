using Archive.AppFiles;
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

namespace Archive.Pages.UserPages
{
    /// <summary>
    /// Логика взаимодействия для UserHistoryPage.xaml
    /// </summary>
    public partial class UserHistoryPage : Page
    {


        private List<DBModel.DocHistory> userHistory;

        public UserHistoryPage(int userId)
        {
            InitializeComponent();

            // Загрузка истории пользователя при создании страницы
            LoadUserHistory(userId);
        }

        private void LoadUserHistory(int userId)
        {
            // Получение истории пользователя из базы данных
            userHistory = DBConnect.entities.DocHistory.Where(d => d.IdUser == userId).ToList();

            // Привязка данных к DataGrid
            UserHistoryGrid.ItemsSource = userHistory;
        }







    }
}
