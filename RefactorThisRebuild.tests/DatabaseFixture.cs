using Microsoft.Data.Sqlite;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using RefactorThisRebuild.Models;

namespace RefactorThisRebuild.tests
{
    #region SharedDatabaseFixture
    public class DatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public DatabaseFixture()
        {
            Connection = new SqliteConnection(@"datasource=testProducts.db");

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public ProductContext CreateContext(DbTransaction transaction = null)
        {
            var context = new ProductContext(new DbContextOptionsBuilder<ProductContext>().UseSqlite(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var one = new Product {Id= new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"), Name = "Apple", Description = "Newest macbook pro M1 chip", DeliveryPrice = 19, Price = 1299 };
                        var two = new Product { Name = "Dell", Description = "Newest Dell Alienware", DeliveryPrice = 9, Price = 2299 };
                        var three = new Product { Name = "Kogan", Description = "Newest Kogan 32 inch", DeliveryPrice = 55, Price = 299 };
                        var four = new Product { Name = "Kogan", Description = "oldest Kogan 25 inch", DeliveryPrice = 5, Price = 129 };
                        var five = new Product { Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec9"), Name = "Kogan", Description = "Kogan Watch", DeliveryPrice = 5, Price = 499 };

                        var optionOne = new ProductOption { Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec4"), ProductId = one.Id, Name = "White", Description = "Apple white color" };
                        var optionTwo = new ProductOption { Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec5"), ProductId = one.Id, Name = "Black", Description = "Apple black color" };
                        var optionThree = new ProductOption { Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec6"), ProductId = one.Id, Name = "Amber", Description = "Apple Amber color" };
                        var optionFour = new ProductOption { ProductId = two.Id, Name = "Yellow", Description = "Dell Yellow color" };
                        var optionFive = new ProductOption {  ProductId = three.Id, Name = "Green", Description = "Kogan Green color" };
                        var optionSix = new ProductOption { ProductId = four.Id, Name = "Blue", Description = "Kogan Blue color" };

                        context.AddRange(one, two, three, four, five,optionOne, optionTwo, optionThree, optionFour, optionFive,optionSix);

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
    #endregion
}
