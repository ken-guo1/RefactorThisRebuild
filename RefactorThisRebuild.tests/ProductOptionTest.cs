using Xunit;
using RefactorThisRebuild.Models;
using RefactorThisRebuild.Controllers;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;

namespace RefactorThisRebuild.tests
{
    public class ProductOptionTest : IClassFixture<DatabaseFixture>
    {
        private string _keyName = "items";
        private Guid _productGuid = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");
        private Guid _optionGuidOne = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec4");
        private Guid _optionGuidTwo = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec5");
        private Guid _optionGuidThree = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec6");
        private Guid _newOptionGuid = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec7");

        public ProductOptionTest(DatabaseFixture fixture) => Fixture = fixture;

        public DatabaseFixture Fixture { get; }

        [Fact]
        public void GetProductOptions_ShouldReturnAllOptions()
        {

            using (var context = Fixture.CreateContext())
            {

                // Act
                var controller = new OptionsController(context);
                var result = controller.Get(_productGuid)[_keyName].Count();

                // Assert
                Assert.True(result > 1, "Expected actualCount to be greater than 1.");
            }



        }
        [Fact]
        public void GetProductOptionsWithId_ShouldReturnOption()
        {

            using (var context = Fixture.CreateContext())
            {

                // Act
                var controller = new OptionsController(context);
                var result = controller.Get(_productGuid, _optionGuidOne);

                // Assert
                Assert.Equal(_optionGuidOne, result.Id);
            }



        }
        [Fact]
        public void PostProductOptionsWithId_ShouldCreateOption()
        {

            using (var context = Fixture.CreateContext())
            {

                //Arrange
                ProductOption option = new ProductOption { Name = "128G", Description = "Biggest" };

                // Act
                var controller = new OptionsController(context);             
                var result = controller.Post(_productGuid, option) ;


                // Assert
                var createdObjectResult = result as CreatedResult;
                Assert.NotNull(createdObjectResult);

                var optionResult = createdObjectResult.Value as ProductOption;
                Assert.NotNull(optionResult);
                Assert.Equal(optionResult.Name, option.Name);
            }



        }
        [Fact]
        public void PutProductOptionsWithId_ShouldUpdateOption()
        {

            using (var context = Fixture.CreateContext())
            {

                var newOption = new ProductOption { Id = _newOptionGuid, Name = "Orange", Description = "Newest Orange" };
                // Act
                var controller = new OptionsController(context);
                var result = controller.Put( _optionGuidTwo, newOption);

                // Assert
                var createdObjectResult = result as CreatedResult;
                Assert.NotNull(createdObjectResult);

                var optionResult = createdObjectResult.Value as ProductOption;
                Assert.NotNull(optionResult);
                Assert.Equal(optionResult.Name, newOption.Name);
            }



        }
        [Fact]
        public void DeleteProductOptionsWithId_ShouldRemoveOption()
        {

            using (var context = Fixture.CreateContext())
            {

                // Act
                var controller = new OptionsController(context);
                var result = controller.Delete(_optionGuidThree);

                // Assert
                var okResult = result as OkResult;

                Assert.Equal(200, okResult.StatusCode);
            }



        }
    }
}
