using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.AppFiles
{
    public class EditDataObject
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime DocumentDate { get; set; } // Пример для даты
        public int DocumentType { get; set; } // Пример для типа документа
        public string RelatedContract { get; set; } // Пример для связанного договора
        public string PersonName { get; set; } // Пример для ФИО
        public string StorageNumber { get; set; } // Пример для номера хранилища
        public string Cabinet { get; set; } // Пример для шкафа
        public string Folder { get; set; } // Пример для папки
        public int MnemonicCode { get; set; } // Пример для мнемокода
        public int? DocumentStatus { get; set; }

    }

}
