//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Archive.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocHistory
    {
        public int id { get; set; }
        public int IdDocument { get; set; }
        public int IdUser { get; set; }
        public System.DateTime DateOfIssue { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string Note { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual User User { get; set; }
    }
}