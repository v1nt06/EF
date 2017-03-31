using System.Data.Entity.Validation;
using EF.Model;

namespace EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EF.Model.Model>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EF.Model.Model context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            try
            {
                context.Categories.AddOrUpdate(
                    c => c.Id,
                    new Category { Id = 1, CategoryName = "Beverages" },
                    new Category { Id = 2, CategoryName = "Condiments" },
                    new Category { Id = 3, CategoryName = "Confections" }
                    );

                context.Regions.AddOrUpdate(
                    r => r.RegionID,
                    new Region { RegionID = 1, RegionDescription = "Eastern" },
                    new Region { RegionID = 2, RegionDescription = "Western" },
                    new Region { RegionID = 3, RegionDescription = "Northern" },
                    new Region { RegionID = 4, RegionDescription = "Southern" }
                    );

                context.Territories.AddOrUpdate(
                    t => t.Id,
                    new Territory { Id = "1", TerritoryDescription = "Westboro", RegionID = 1 },
                    new Territory { Id = "2", TerritoryDescription = "Bedford", RegionID = 2 },
                    new Territory { Id = "3", TerritoryDescription = "Georgetow", RegionID = 3 },
                    new Territory { Id = "4", TerritoryDescription = "Boston", RegionID = 4 }
                    );
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }
        }
    }
}
