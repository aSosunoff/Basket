﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AGRO_BASKET> AGRO_BASKET { get; set; }
        public virtual DbSet<AGRO_CONTRACT> AGRO_CONTRACT { get; set; }
        public virtual DbSet<AGRO_ORDER> AGRO_ORDER { get; set; }
        public virtual DbSet<AGRO_PRODUCT> AGRO_PRODUCT { get; set; }
    }
}
