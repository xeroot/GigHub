namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 100));
        }
    }
}
