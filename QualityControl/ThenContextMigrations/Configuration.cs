namespace QualityControl.ThenContextMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class TheContextDbConfig : DbMigrationsConfiguration<QualityControl.Db.TheContext>
    {
        public TheContextDbConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"ThenContextMigrations";
            ContextKey = "QualityControl.Db.TheContext";
        }

        protected override void Seed(QualityControl.Db.TheContext context)
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
