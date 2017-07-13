namespace StakHappy.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StakHappyDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientContacts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Client_Id = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 150),
                        LastName = c.String(maxLength: 150),
                        Email = c.String(maxLength: 300),
                        PrimaryPhone = c.String(maxLength: 30),
                        SecondaryPhone = c.String(maxLength: 30),
                        IsPrimary = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                        BusinessPhone = c.String(maxLength: 30),
                        AddressLine1 = c.String(maxLength: 300),
                        AddressLine2 = c.String(maxLength: 300),
                        City = c.String(maxLength: 300),
                        State = c.String(maxLength: 150),
                        PostalCode = c.String(maxLength: 20),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CompanyName = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Client_Id = c.Guid(nullable: false),
                        Number = c.String(maxLength: 100),
                        Date = c.DateTime(nullable: false),
                        PaymentRequiredDate = c.DateTime(),
                        PaymentTerm = c.Int(),
                        Voided = c.Boolean(),
                        VoidedDate = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        PaidInFull = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Client_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Invoice_Id = c.Guid(nullable: false),
                        Service_Id = c.Guid(),
                        Quantity = c.Int(nullable: false),
                        UnitCost = c.Long(nullable: false),
                        DiscountPercentage = c.Decimal(precision: 18, scale: 2),
                        DiscountAmount = c.Long(),
                        Description = c.String(),
                        DeletedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserServices", t => t.Service_Id)
                .Index(t => t.Invoice_Id)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.UserServices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                        Label = c.String(maxLength: 300),
                        Description = c.String(),
                        Price = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 150),
                        FirstName = c.String(maxLength: 150),
                        LastName = c.String(maxLength: 150),
                        CompanyName = c.String(maxLength: 100),
                        DisplayName = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 300),
                        PrimaryPhone = c.String(maxLength: 30),
                        SecondaryPhone = c.String(maxLength: 30),
                        AddressLine1 = c.String(maxLength: 300),
                        AddressLine2 = c.String(maxLength: 300),
                        City = c.String(maxLength: 300),
                        State = c.String(maxLength: 150),
                        PostalCode = c.String(maxLength: 30),
                        Country = c.String(maxLength: 150),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "IX_User_UserName")
                .Index(t => t.Email, unique: true, name: "IX_User_Email");
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 300),
                        Name = c.String(maxLength: 300),
                        ObjectFullName = c.String(maxLength: 500),
                        XmlData = c.String(),
                        JsonData = c.String(),
                        DataType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedData = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Invoice_Id = c.Guid(nullable: false),
                        Amount = c.Long(nullable: false),
                        Description = c.String(),
                        TypeId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        VoidedDate = c.DateTime(),
                        Reference = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id, cascadeDelete: true)
                .Index(t => t.Invoice_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientContacts", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Clients", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Payments", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceItems", "Service_Id", "dbo.UserServices");
            DropForeignKey("dbo.UserServices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserSettings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Invoices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.InvoiceItems", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Payments", new[] { "Invoice_Id" });
            DropIndex("dbo.UserSettings", new[] { "User_Id" });
            DropIndex("dbo.Users", "IX_User_Email");
            DropIndex("dbo.Users", "IX_User_UserName");
            DropIndex("dbo.UserServices", new[] { "User_Id" });
            DropIndex("dbo.InvoiceItems", new[] { "Service_Id" });
            DropIndex("dbo.InvoiceItems", new[] { "Invoice_Id" });
            DropIndex("dbo.Invoices", new[] { "User_Id" });
            DropIndex("dbo.Invoices", new[] { "Client_Id" });
            DropIndex("dbo.Clients", new[] { "User_Id" });
            DropIndex("dbo.ClientContacts", new[] { "Client_Id" });
            DropTable("dbo.Payments");
            DropTable("dbo.UserSettings");
            DropTable("dbo.Users");
            DropTable("dbo.UserServices");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientContacts");
        }
    }
}
