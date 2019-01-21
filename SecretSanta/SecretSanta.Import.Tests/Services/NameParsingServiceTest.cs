using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Import.Services;
using System;

namespace SecretSanta.Import.Tests
{
    [TestClass]
    public class NameParsingServiceTest
    {
        public NameParsingService CreateDefaultImporter()
        {
            return new NameParsingService();
        }

        [TestMethod]
        public void ValidateHeader_ValidFirstLastFormat_ReturnTrue()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Grant Woods");

            Assert.AreEqual<bool>(true, isValid);
        }

        [TestMethod]
        public void ValidateHeader_ValidLastFirstFormat_ReturnTrue()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Woods, Grant");

            Assert.AreEqual<bool>(true, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InValidLastFirstFormat_ReturnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Woods, Grant");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InvalidMissingFirstLast_ReturnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name:");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_MissingLast_ReturnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Grant");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_MissingFirst_ReturnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name: Woods,");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_InvalidMissingFirstLastTrailingSpace_ReturnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("Name:          ");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        public void ValidateHeader_EmptyString_RetrurnFalse()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader("");

            Assert.AreEqual<bool>(false, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateHeader_NullString_ThrowsNullArgument()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            bool isValid = giftImportService.ValidateHeader(null);
        }


    }
}
