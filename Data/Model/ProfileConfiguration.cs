//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProfileConfiguration
    {
        public int Id_Configuration { get; set; }
        public Nullable<int> BoardStyle { get; set; }
        public Nullable<int> TilesStyle { get; set; }
        public Nullable<int> Id_Account { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
