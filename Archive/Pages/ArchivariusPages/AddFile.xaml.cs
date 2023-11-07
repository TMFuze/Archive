using Archive.AppFiles;
using Archive.DBModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Archive.Pages.ArchivariusPages
{
    public partial class AddFile : Page
    {
        public AddFile()
        {
            InitializeComponent();

            DocumentType.SelectionChanged += DocumentType_SelectionChanged;

            RelatedContract.IsEnabled = false;

            // ComboBox Type - вывод данных
            DocumentType.SelectedValuePath = "Id";
            DocumentType.DisplayMemberPath = "Name";
            DocumentType.ItemsSource = DBConnect.entities.Type.ToList();

            // ComboBox MnemonicCode - вывод данных
            MnemonicCode.SelectedValuePath = "Id";
            MnemonicCode.DisplayMemberPath = "Color";
            MnemonicCode.ItemsSource = DBConnect.entities.MnemonicCode.ToList();

            // ComboBox Status - вывод данных
            DocumentStatus.SelectedValuePath = "Id";
            DocumentStatus.DisplayMemberPath = "Name";
            DocumentStatus.ItemsSource = DBConnect.entities.Status.ToList();
        }

        private void AddDoc_Click(object sender, RoutedEventArgs e)
        {
            Document docObj = new Document()
            {
                DateOfDocument = DocumentDate.DisplayDate,
                FIO = PersonName.Text
            };

            if (int.TryParse(DocumentNumber.Text, out int documentNumber))
            {
                docObj.Number = documentNumber;
            }

            if (int.TryParse(StorageNumber.Text, out int storageNumber))
            {
                docObj.IdStorage = storageNumber;
            }

            if (int.TryParse(RelatedContract.Text, out int relatedDocument))
            {
                docObj.RelatedDocument = relatedDocument;
            }


            if (int.TryParse(Cabinet.Text, out int cabinetNumber))
            {
                docObj.Wardrobe = cabinetNumber;
            }

            if (int.TryParse(Folder.Text, out int folderNumber))
            {
                docObj.Folder = folderNumber;
            }

            if (DocumentType.SelectedValue != null)
            {
                docObj.IdType = (int)DocumentType.SelectedValue;
            }

            if (MnemonicCode.SelectedValue != null)
            {
                docObj.IdMnemonicCode = (int)MnemonicCode.SelectedValue;
            }

            if (DocumentStatus.SelectedValue != null)
            {
                docObj.IdStatus = (int)DocumentStatus.SelectedValue;
            }

            try
            {
                // Проверка пустых значений
                if (string.IsNullOrWhiteSpace(DocumentNumber.Text) ||
                    DocumentDate.SelectedDate == null ||
                    DocumentType.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(PersonName.Text) ||
                    string.IsNullOrWhiteSpace(StorageNumber.Text) ||
                    string.IsNullOrWhiteSpace(Cabinet.Text) ||
                    string.IsNullOrWhiteSpace(Folder.Text) ||
                    MnemonicCode.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все строки!",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                else
                {
                    // Проверка на обязательное заполнение связанного договора для типа с Id равным 3
                    if (docObj.IdType == 3)
                    {
                        if (string.IsNullOrWhiteSpace(RelatedContract.Text))
                        {
                            MessageBox.Show("Введите номер связанного договора.",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            // Проверка на повторение данных
                            if (DBConnect.entities.Document.Count(x => x.Number == docObj.Number && x.IdType == docObj.IdType) > 0)
                            {
                                MessageBox.Show("Такой номер документа с таким типом уже есть",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                            else
                            {
                                DBConnect.entities.Document.Add(docObj);
                                DBConnect.entities.SaveChanges();

                                MessageBox.Show("Документ добавлен",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                FrameApp.frmObj.Navigate(new ArchFilesPage());
                            }
                        }
                    }
                    else
                    {
                        // Для остальных типов, связанный договор не обязателен
                        // Проверка на повторение данных
                        if (DBConnect.entities.Document.Count(x => x.Number == docObj.Number && x.IdType == docObj.IdType) > 0)
                        {
                            MessageBox.Show("Такой номер документа с таким типом уже есть",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            DBConnect.entities.Document.Add(docObj);
                            DBConnect.entities.SaveChanges();

                            MessageBox.Show("Документ добавлен",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            FrameApp.frmObj.Navigate(new ArchFilesPage());
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Введены некорректные данные.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DocumentType.SelectedItem != null)
            {
                // Получаем выбранный тип документа
                DBModel.Type selectedType = (DBModel.Type)DocumentType.SelectedItem;

                // Проверяем IdType выбранного типа
                if (selectedType.Id == 3)
                {
                    // Разрешаем ввод связанного договора только для типа с Id равным 3
                    RelatedContract.IsEnabled = true;

                    // Проверяем, что поле связанного договора не пусто
                    if (string.IsNullOrWhiteSpace(RelatedContract.Text))
                    {
                        // Очищаем поле связанного договора
                        RelatedContract.Text = string.Empty;
                    }
                }
                else
                {
                    // Запрещаем ввод связанного договора для остальных типов
                    RelatedContract.IsEnabled = false;
                    // Очищаем поле связанного договора
                    RelatedContract.Text = string.Empty;
                }
            }
        }

        

        private void ClearAllItems_Click(object sender, RoutedEventArgs e)
        {
            // Очистка текстовых полей
            DocumentNumber.Clear();
            StorageNumber.Clear();
            Cabinet.Clear();
            Folder.Clear();
            PersonName.Clear();
            RelatedContract.Clear();

            // Сброс значения DatePicker
            DocumentDate.SelectedDate = null;

            // Сброс значения ComboBox
            DocumentType.SelectedIndex = -1;
            MnemonicCode.SelectedIndex = -1;
            DocumentStatus.SelectedIndex = -1;
        }
    }
}
