﻿//------------------------------------------------------------------------------
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
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BlockusEntities : DbContext
    {
        public BlockusEntities()
            : base("name=BlockusEntities")
        {
        }

        public BlockusEntities(DbConnection connection)
            : base(connection, true)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BlackList> BlackList { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<ProfileConfiguration> ProfileConfiguration { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Results> Results { get; set; }
    }
}
