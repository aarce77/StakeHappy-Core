using System.Data.Entity;

namespace StakHappy.Core.Data
{
    internal class Context : DbContext
    {
        public Context() : base("StakHappy") { }
        public Context(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<Model.Client> Clients { get; set; }
        public DbSet<Model.ClientContact> ClientContacts { get; set; }
        public DbSet<Model.Invoice> Invoices { get; set; }
        public DbSet<Model.InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Model.Payment> Payments { get; set; }
        public DbSet<Model.User> Users { get; set; }
        public DbSet<Model.UserService> UserServices { get; set; }
        public DbSet<Model.UserSetting> UserSettings { get; set; }
    }
}
