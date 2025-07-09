using System;
using Xunit;

namespace Monopoly_Test_v2.Tests
{
    public class PalletTests
    {
        [Fact]
        public void TotalWeight_ShouldIncludeBoxesWeight()
        {
            var pallet = new Pallet();
            var box1 = new Box { Weight = 10 };
            var box2 = new Box { Weight = 20 };

            pallet.Boxes.Add(box1);
            pallet.Boxes.Add(box2);

            Assert.Equal(60, pallet.TotalWeight); // 30 (own) + 10 + 20
        }

        [Fact]
        public void ExpirationDate_ShouldBeMinBoxExpiration()
        {
            var pallet = new Pallet();
            var box1 = new Box { ProductionDate = new DateTime(2023, 01, 01) };
            var box2 = new Box { ExpirationDate = new DateTime(2023, 06, 01) };

            pallet.Boxes.Add(box1);
            pallet.Boxes.Add(box2);

            Assert.Equal(new DateTime(2023, 04, 11), pallet.ExpirationDate);
        }

        [Fact]
        public void CanContain_ShouldValidateDimensions()
        {
            var pallet = new Pallet { Width = 100, Depth = 100 };
            var validBox = new Box { Width = 100, Height = 100 };
            var invalidBox = new Box { Width = 101, Height = 100 };

            Assert.True(pallet.CanContain(validBox));
            Assert.False(pallet.CanContain(invalidBox));
        }
    }
}
