using System;
using Xunit;

namespace Monopoly_Test_v2.Tests
{
    public class BoxTests
    {
        [Fact]
        public void Volume_ShouldCalculateCorrectly()
        {
            var box = new Box { Width = 100, Height = 100, Depth = 100 };

            Assert.Equal(1, box.Volume); // 100*100*100/1000000 = 1
        }

        [Fact]
        public void CalculatedExpirationDate_ShouldUseProductionDateWhenMissing()
        {
            var productionDate = new DateTime(2023, 01, 01);
            var box = new Box { ProductionDate = productionDate };

            Assert.Equal(productionDate.AddDays(100), box.CalculatedExpirationDate);
        }
    }
}
