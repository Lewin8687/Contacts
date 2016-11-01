using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Contacts_NoAuth_.Models
{
    public class Contacts_NoAuth_Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Contacts_NoAuth_Context() : base("name=Contacts_NoAuth_Context")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Contacts_NoAuth_Context>(null);
            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.DbSet<Contacts_NoAuth_.Models.Contacts> Contacts { get; set; }

        public System.Data.Entity.DbSet<Contacts_NoAuth_.Models.History> Histories { get; set; }
    }
}
