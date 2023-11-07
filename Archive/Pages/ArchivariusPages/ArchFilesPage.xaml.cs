using Archive.AppFiles;
using Archive.DBModel;
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
    /// Логика взаимодействия для ArchFilesPage.xaml
    /// </summary>
    public partial class ArchFilesPage : Page
    {
        private Document selectedDoc;
        private List<Document> allItems;
        public ArchFilesPage()
        {
            InitializeComponent();
            
            FolderBox.SelectedValuePath = "Id";
            FolderBox.DisplayMemberPath = "Name";
            FolderBox.ItemsSource = DBConnect.entities.Type.ToList();

            DGItems.ItemsSource = DBConnect.entities.Document.ToList();

            allItems = DBConnect.entities.Document.ToList();
        }

        private void UpdateDataGrid()
        {
            var selectedDoc = FolderBox.SelectedItem as DBModel.Type;
            string searchText = SearchBox.Text.ToLower();

            // Получение коллекции всех элементов или элементов для выбранной папки
            var items = (selectedDoc != null) ? allItems.Where(x => x.IdType == selectedDoc.Id) : allItems;

            // Выполнение фильтрации поискового запроса
            var filteredItems = items.Where(item =>
                (item.FIO != null && item.FIO.ToLower().Contains(searchText)) ||
                (item.Status != null && item.Status.Name.ToLower().Contains(searchText)) ||
                (item.Number.ToString().Contains(searchText)) ||
                item.DateOfDocument.ToString("dd.MM.yyyy").Contains(searchText) ||
                item.IdStorage.ToString().Contains(searchText) ||
                item.Wardrobe.ToString().Contains(searchText) ||
                item.Folder.ToString().Contains(searchText) ||
                item.MnemonicCode != null && item.MnemonicCode.Color.ToLower().Contains(searchText) ||
                item.Type != null && item.Type.Name.ToLower().Contains(searchText) ||
                item.Id.ToString().Contains(searchText)
                );


            // Обновление отображения в DataGrid
            DGItems.ItemsSource = filteredItems;

            // Выбор первого элемента, если есть результаты
            if (filteredItems.Any())
            {
                DGItems.SelectedIndex = 0;
            }
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                UpdateDataGrid();
            }
        }

        private void RefreshDGBtn_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем выбранную папку
            FolderBox.SelectedItem = null;

            // Обновляем данные в DGItems
            DGItems.ItemsSource = DBConnect.entities.Document.ToList();
        }

        private void FolderBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var filesForRemoving = DGItems.SelectedItems.Cast<Document>().ToList();
            try
            {
                var result = MessageBox.Show("Вы уверены?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Удаление выбранных файлов из базы данных
                    DBConnect.entities.Document.RemoveRange(filesForRemoving);
                    DBConnect.entities.SaveChanges();
                    MessageBox.Show("Данные удалены.");

                    // Получение выбранной папки перед обновлением данных
                    var selectedFolder = FolderBox.SelectedItem as DBModel.Type;

                    // Обновление значений в DataGrid
                    UpdateDataGrid();

                    // Повторный выбор папки, если она была выбрана
                    if (selectedFolder != null)
                    {
                        FolderBox.SelectedItem = selectedFolder;
                    }
                }
                else
                {
                    DGItems.ItemsSource = DBConnect.entities.Document.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.frmObj.Navigate(new Pages.ArchivariusPages.AddFile());
        }

        private void HistoryGo_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.frmObj.Navigate(new DocHistory());
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDoc != null)
            {
                EditFilePage editPage = new EditFilePage(selectedDoc);
                FrameApp.frmObj.Navigate(editPage);
            }
        }

        private void DGItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoc = (Document)DGItems.SelectedItem;
        }
    }
}
