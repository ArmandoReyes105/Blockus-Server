//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int Id_Report { get; set; }
        public string ReportDescription { get; set; }
        public Nullable<System.DateTime> ReportDate { get; set; }
        public Nullable<int> Id_Complainant { get; set; }
        public Nullable<int> Id_Reported { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
    }
}
