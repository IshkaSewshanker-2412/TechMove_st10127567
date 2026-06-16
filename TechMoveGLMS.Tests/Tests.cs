using Xunit;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Tests
{
    // Currency Conversion
    public class CurrencyConversionTests
    {
        [Fact]
        public void ConvertUsdToZar_GivenRate_ReturnsCorrectValue()
        {
            decimal usd = 100m;
            decimal rate = 16.63m;
            decimal zar = usd * rate;
            Assert.Equal(1663m, zar);
        }
    }

    // File Validation
    public class FileValidationTests
    {
        [Theory]
        [InlineData("agreement.pdf", true)]
        [InlineData("document.exe", false)]
        [InlineData("notes.txt", false)]
        public void ValidateFileExtension(string fileName, bool expected)
        {
            bool isPdf = fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
            Assert.Equal(expected, isPdf);
        }
    }

    // Contract Status Validation
    public class ContractValidationTests
    {
        [Theory]
        [InlineData("Active", true)]
        [InlineData("Expired", false)]
        [InlineData("On Hold", false)]
        public void CanCreateServiceRequest_BasedOnContractStatus(string status, bool expected)
        {
            var contract = new Contract { Status = status };
            bool canCreate = !(contract.Status == "Expired" || contract.Status == "On Hold");
            Assert.Equal(expected, canCreate);
        }
    }
}
