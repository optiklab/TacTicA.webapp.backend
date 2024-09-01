using System;
using System.Threading.Tasks;
using TacTicA.Services.Items.Domain.Models;
using TacTicA.Services.Items.Domain.Repositories;
using TacTicA.Services.Items.Services;
using Moq;
using Xunit;

namespace TacTicA.Services.Items.Tests.Unit.Services
{
    public class ItemServiceTests
    {
        [Fact]
        public async Task item_service_add_async_should_succeed()
        {
            var category = "test";
            var itemRepositoryMock = new Mock<IItemRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(x => x.GetAsync(category))
                .ReturnsAsync(new Category(category));
            var itemService = new ItemService(itemRepositoryMock.Object,
                categoryRepositoryMock.Object);

            var id = Guid.NewGuid();
            await itemService.AddAsync(id, Guid.NewGuid(), category, "item",
                "description", DateTime.UtcNow);

            categoryRepositoryMock.Verify(x => x.GetAsync(category), Times.Once);
            itemRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Item>()), Times.Once);
        }
    }
}