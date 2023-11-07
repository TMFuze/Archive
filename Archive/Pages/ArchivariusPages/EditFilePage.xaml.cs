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
    /// Логика взаимодействия для EditFilePage.xaml
    /// </summary>
    public partial class EditFilePage : Page
    {
        private Document doc;

        public EditFilePage(Document selectedDoc)
        {
            InitializeComponent();

            DocumentType.SelectedValuePath = "Id";
            DocumentType.DisplayMemberPath = "Name";
            DocumentType.ItemsSource = DBConnect.entities.Type.ToList();

            // ComboBox MnemonicCode - вывод данных
            MnemoCode.SelectedValuePath = "Id";
            MnemoCode.DisplayMemberPath = "Color";
            MnemoCode.ItemsSource = DBConnect.entities.MnemonicCode.ToList();

            // ComboBox Status - вывод данных
            DocumentStatus.SelectedValuePath = "Id";
            DocumentStatus.DisplayMemberPath = "Name";
            DocumentStatus.ItemsSource = DBConnect.entities.Status.ToList();



            this.doc = selectedDoc;
            // Заполните элементы формы значениями из supplier
            if (doc != null)
            {
                DocumentNumber.Text = doc.Number.ToString();
                DocumentDate.SelectedDate = doc.DateOfDocument;
                DocumentType.SelectedItem = doc.Type;
                RelatedContract.Text = doc.RelatedDocument.ToString();
                PersonName.Text = doc.FIO;
                StorageNumber.Text = doc.IdStorage.ToString();
                Cabinet.Text = doc.Wardrobe.ToString();
                Folder.Text = doc.Folder.ToString();
                MnemoCode.SelectedItem = doc.MnemonicCode;
                DocumentStatus.SelectedItem = doc.Status;
            }


        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.frmObj.GoBack();
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

        private void CompleteChange_Click(object sender, RoutedEventArgs e)
        {
            if (doc != null)
            {
                doc.Number = int.Parse(DocumentNumber.Text);
                doc.DateOfDocument = DocumentDate.SelectedDate.Value;
                doc.Type = (DBModel.Type)DocumentType.SelectedItem;
                if (int.TryParse(RelatedContract.Text, out int result))
                {
                    doc.RelatedDocument = result;
                }
                doc.FIO = PersonName.Text;
                doc.IdStorage = int.Parse(StorageNumber.Text);
                doc.Wardrobe = int.Parse(Cabinet.Text);
                doc.Folder = int.Parse(Folder.Text);
                doc.MnemonicCode = (MnemonicCode)MnemoCode.SelectedItem;
                doc.Status = (Status)DocumentStatus.SelectedItem;


                DBConnect.entities.SaveChanges();
                FrameApp.frmObj.Navigate(new ArchFilesPage());
            }

        }
    }

}
