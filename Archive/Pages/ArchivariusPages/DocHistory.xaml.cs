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

namespace Archive.Pages.ArchivariusPages
{
    /// <summary>
    /// Логика взаимодействия для DocHistory.xaml
    /// </summary>
    public partial class DocHistory : Page
    {

        private DocHistory selectedDoc;
        public DocHistory()
        {
            InitializeComponent();


            HistoryDG.ItemsSource = DBConnect.entities.DocHistory.ToList();
        }

        private void HistoryDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoc = (DocHistory)HistoryDG.SelectedItem;
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = SearchBox.Text.ToLower();

                // Получение истории документов из базы данных
                var allHistory = DBConnect.entities.DocHistory.ToList();

                // Выполнение фильтрации поискового запроса
                var filteredHistory = allHistory.Where(item =>
                    item.IdDocument.ToString().Contains(searchText) ||
                    item.DateOfIssue.ToString("dd.MM.yyyy").Contains(searchText) ||
                    item.ReturnDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                    item.Note != null && item.Note.ToLower().Contains(searchText) ||
                    item.IdUser.ToString().Contains(searchText)
                );

                // Обновление отображения в DataGrid
                HistoryDG.ItemsSource = filteredHistory;

                // Выбор первого элемента, если есть результаты
                if (filteredHistory.Any())
                {
                    HistoryDG.SelectedIndex = 0;
                }
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var filesForRemoving = HistoryDG.SelectedItems.Cast<DBModel.DocHistory>().ToList();
            try
            {
                var result = MessageBox.Show("Вы уверены?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Удаление выбранных файлов из базы данных
                    DBConnect.entities.DocHistory.RemoveRange(filesForRemoving);
                    DBConnect.entities.SaveChanges();
                    MessageBox.Show("Данные удалены.");

                    HistoryDG.ItemsSource = DBConnect.entities.DocHistory.ToList();
                }
                else
                {
                    HistoryDG.ItemsSource = DBConnect.entities.DocHistory.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
