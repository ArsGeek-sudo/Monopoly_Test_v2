using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly_Test_v2.Tests
{
    public class PalletGroupingTests
    {
        [Fact]
        public void GroupPallets_ShouldSortGroupsByExpirationAscending()
        {
            var pallets = new List<Pallet>
            {
                CreatePallet(1, new DateTime(2023, 12, 31)),   // Группа 3
                CreatePallet(2, new DateTime(2023, 01, 01)),  // Группа 1
                CreatePallet(3, null),                       // Группа 4 (null)
                CreatePallet(4, new DateTime(2023, 06, 01)) // Группа 2
            };

            var grouped = pallets
                .GroupBy(p => p.ExpirationDate)
                .OrderBy(g => g.Key ?? DateTime.MaxValue)
                .ToList();

            Assert.Equal(4, grouped.Count);
            Assert.Equal(new DateTime(2023, 01, 01), grouped[0].Key);
            Assert.Equal(new DateTime(2023, 06, 01), grouped[1].Key);
            Assert.Equal(new DateTime(2023, 12, 31), grouped[2].Key);
            Assert.Null(grouped[3].Key);
        }

        [Fact]
        public void GroupPallets_ShouldSortPalletsByWeightInGroup()
        {
            var pallets = new List<Pallet>
            {
                CreatePallet(1, new DateTime(2023, 01, 01), 50), // Вес 50
                CreatePallet(2, new DateTime(2023, 01, 01), 30), // Вес 30
                CreatePallet(3, new DateTime(2023, 01, 01), 40)  // Вес 40
            };

            var group = pallets
                .GroupBy(p => p.ExpirationDate)
                .OrderBy(g => g.Key ?? DateTime.MaxValue)
                .First()
                .OrderBy(p => p.TotalWeight)
                .ToList();

            Assert.Equal(3, group.Count);
            Assert.Equal(2, group[0].Id); // Самый легкий (30)
            Assert.Equal(3, group[1].Id); // Средний (40)
            Assert.Equal(1, group[2].Id); // Самый тяжелый (50)
        }

        private Pallet CreatePallet(long id, DateTime? expirationDate, double weight = 30)
        {
            var boxes = new List<Box>();

            if (expirationDate.HasValue)
            {
                boxes.Add(new Box
                {
                    Weight = weight,
                    ExpirationDate = expirationDate
                });
            }

            return new Pallet
            {
                Id = id,
                Boxes = boxes
            };
        }
    }
}