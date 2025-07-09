using System;
using Xunit;

namespace Monopoly_Test_v2.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void BoxValidation_RequireAtLeastOneDate()
        {
            var box = new Box();

            Assert.Throws<InvalidOperationException>(() =>
            {
                if (box.ProductionDate == null && box.ExpirationDate == null)
                    throw new InvalidOperationException("Требуется хотя бы одна дата");
            });
        }
    }
}
