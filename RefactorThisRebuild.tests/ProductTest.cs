using Xunit;
using RefactorThisRebuild.Models;
using RefactorThisRebuild.Controllers;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;

namespace RefactorThisRebuild.tests
{
    public class ProductTest : IClassFixture<DatabaseFixture>
    {
        private string _keyName = "items";
        private Guid _testGuid = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec8");
        private Guid _existGuid = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec9");

        public ProductTest(DatabaseFixture fixture) => Fixture = fixture;

        public DatabaseFixture Fixture { get; }



        [Fact]
        public void GetProducts_ShouldReturnAllProducts()
        {

            using (var context = Fixture.CreateContext())
            {

                // Act
                var controller = new ProductsController(context);
                var result = controller.Get()[_keyName].Count();

                // Assert
                Assert.True(result > 2, "Expected actualCount to be greater than 2.");
            }



        }
        [Fact]
        public void GetProductWithName_ShouldReturnAllSpecifiedName()
        {
            using (var context = Fixture.CreateContext())
            {
                //Arrange
                string name = "Kogan";

                // Act
                var controller = new ProductsController(context);
                var result = controller.Get(name)[_keyName].Count();

                // Assert
                Assert.True(result > 2, "Expected actualCount to be greater than 2.");


            }

        }
        [Fact]
        public void GetProductWithId_ShouldReturnProduct()
        {
            using (var context = Fixture.CreateContext()) 
            {
                // Act
                var controller = new ProductsController(context);
                var result = controller.Get(_existGuid) ;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(_existGuid, result.Id);
            }

        }

        [Fact]
        public void PostProduct_ShouldCreateProduct()
        {

            using (var context = Fixture.CreateContext())
            {
                //Arrange
                Product newProduct = new Product { Id = _testGuid, Name = "Window", Description = "Newest windows", DeliveryPrice = 129, Price = 1339 };
                // Act
                var controller = new ProductsController(context);
                var result = controller.Post(newProduct);

                // Assert
                var createdObjectResult = result as CreatedResult;
                Assert.NotNull(createdObjectResult);

                var productResult = createdObjectResult.Value as Product;
                Assert.NotNull(productResult);
                Assert.Equal(productResult.Name, newProduct.Name);
            }



        }
        [Fact]
        public void PutProduct_ShouldUpdateProduct()
        {

            using (var context = Fixture.CreateContext())
            {
                //Arrange
                var newProduct = new Product { Name = "after", Description = "After Newest windows", DeliveryPrice = 129, Price = 1339 };

                // Act
                var controller = new ProductsController(context);
                var result = controller.Put(_existGuid, newProduct);

                // Assert
                var createdObjectResult = result as CreatedResult;
                Assert.NotNull(createdObjectResult);

                var productResult = createdObjectResult.Value as Product;
                Assert.NotNull(productResult);
                Assert.Equal(productResult.Name, newProduct.Name);
            }



        }
        [Fact]
        public void DeleteProduct_ShouldDeleteProduct()
        {

            using (var context = Fixture.CreateContext())
            {
                // Act
                var controller = new ProductsController(context);
                var result = controller.Delete(_existGuid);

                ProductOption option = context.ProductOptions.Where(o => o.Id == _existGuid).FirstOrDefault();

                // Assert
                var okResult = result as OkResult;
                Assert.Equal(200, okResult.StatusCode);
                Assert.Null(option);
            }



        }
    }
}
