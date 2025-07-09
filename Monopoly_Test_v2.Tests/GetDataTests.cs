using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Monopoly_Test_v2.Tests
{
    public class GetDataTests
    {
        [Fact]
        public async Task GetPallets_ShouldReturnPallets()
        {
            var mockService = new Mock<IDataService>();
            mockService.Setup(s => s.GetPallets())
                .ReturnsAsync(new List<Pallet> {
                    new Pallet { Id = 1 },
                    new Pallet { Id = 2 }
                });

            var pallets = await mockService.Object.GetPallets();

            Assert.NotNull(pallets);
            Assert.Equal(2, pallets.Count);
        }
    }
}