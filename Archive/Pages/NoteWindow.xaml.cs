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

namespace Archive.Pages
{
    /// <summary>
    /// Логика взаимодействия для NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        public string Note { get; private set; }
        public NoteWindow()
        {
         
            
            InitializeComponent();


            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Установите значение Note из TextBox
            Note = NoteTextBox.Text;

            // Установите DialogResult в true, чтобы показать, что нажата кнопка "OK"
            DialogResult = true;

            // Закройте окно
            Close();
        }
    }
}
