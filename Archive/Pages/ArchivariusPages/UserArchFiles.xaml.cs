using Archive.AppFiles;
using Archive.DBModel;
using Archive.Pages.UserPages;
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
    /// Логика взаимодействия для UserArchFiles.xaml
    /// </summary>
    public partial class UserArchFiles : Page
    {
        private Document selectedDoc;
        private List<Document> allItems;
        private int userId;

        public UserArchFiles(int userId)
        {
            InitializeComponent();

            FolderBox.SelectedValuePath = "Id";
            FolderBox.DisplayMemberPath = "Name";
            FolderBox.ItemsSource = DBConnect.entities.Type.ToList();

            this.userId = userId;

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


        private void TakeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли элемент в DataGrid
            if (DGItems.SelectedItem != null)
            {
                var selectedDocument = (Document)DGItems.SelectedItem;

                // Проверяем, есть ли у документа IdStatus
                if (selectedDocument.IdStatus.HasValue)
                {
                    MessageBox.Show("Документ отсутствует в архиве комитета.");
                    return;
                }

                // Проверяем, есть ли запись в DocHistory с датой возврата > текущей даты
                if (DBConnect.entities.DocHistory.Any(d => d.IdDocument == selectedDocument.Id && d.ReturnDate > DateTime.Now))
                {
                    MessageBox.Show("Документ занят. Пожалуйста, подождите его возврата.");
                    return;
                }

                // Создаем новый объект DocHistory и заполняем его свойства
                var docHistory = new DBModel.DocHistory
                {
                    IdDocument = selectedDocument.Id,
                    IdUser = userId, // Замените на логику получения имени текущего пользователя
                    DateOfIssue = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7)
                };

                // Открываем окно для ввода примечания
                NoteWindow noteWindow = new NoteWindow();
                
                noteWindow.ShowDialog();

                // Проверяем, было ли введено примечание
                if (noteWindow.DialogResult == true)
                {
                    docHistory.Note = noteWindow.NoteTextBox.Text;
                }

                // Добавляем объект docHistory в базу данных
                DBConnect.entities.DocHistory.Add(docHistory);

                // Сохраняем изменения в базе данных
                DBConnect.entities.SaveChanges();
                MessageBox.Show("Документ успешно зарезервирован вами, обратитесь к архивариусу для его получения");

                // По желанию, вы можете обновить DataGrid или выполнить другие необходимые действия
                // Например, обновить DataGrid, чтобы отобразить изменения
                // DGItems.ItemsSource = DBConnect.entities.YourDocumentItems.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите документ из DataGrid перед нажатием 'Take'.");
            }

        }

        private void RefreshDGBtn_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем выбранную папку
            FolderBox.SelectedItem = null;

            // Обновляем данные в DGItems
            DGItems.ItemsSource = DBConnect.entities.Document.ToList();
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateDataGrid();
            }
        }

        private void DGItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoc = (Document)DGItems.SelectedItem;
        }

        private void FolderBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            

            // Переход на страницу истории пользователя
            NavigationService.Navigate(new UserHistoryPage(userId));

        }
    }
}
