namespace QualityControl.ApplicationContextMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class LocalDbConfig : DbMigrationsConfiguration<QualityControl.Models.ApplicationDbContext>
    {
        public LocalDbConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"ApplicationContextMigrations";
            ContextKey = "QualityControl.Models.ApplicationDbContext";
        }

        protected override void Seed(QualityControl.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
