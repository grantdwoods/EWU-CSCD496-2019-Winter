using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Import.Services;
using System;

namespace SecretSanta.Import.Tests
{
    [TestClass]
    public class GiftImportServiceTest
    {
        public GiftImportService CreateDefaultImporter()
        {
            return new GiftImportService();
        }

        [TestMethod]
        public void ValidateHeader_ValidFirstLastFormat_ReturnTrue()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Grant Woods");

            Assert.AreEqual<bool>(true, isValid);
        }

        [TestMethod]
        public void ValidateHeader_ValidLastFirstFormat_ReturnTrue()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Woods, Grant");

            Assert.AreEqual<bool>(true, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InValidLastFirstFormat_ReturnFalse()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Woods, Grant");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InvalidMissingFirstLast_ReturnFalse()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name:");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InvalidMissingFirstLastTrailingSpace_ReturnFalse()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name:          ");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_EmptyString_RetrurnFalse()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateHeader_NullString_ThrowsNullArgument()
        {
            GiftImportService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader(null);
        }
    }
}
