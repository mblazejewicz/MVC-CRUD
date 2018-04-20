namespace WebApplicationMVC_CRUD.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PfonesModel : DbContext
    {
        public PfonesModel()
            : base("name=PfonesModel")
        {
        }

        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<Phones> Phones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
