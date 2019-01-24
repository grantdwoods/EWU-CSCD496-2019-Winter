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
        public void ParseHeader_ValidFirstLastFormat_ReturnFirstLast()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader("Name: Grant Woods");

            Assert.AreEqual<string>("Grant", names[0]);
            Assert.AreEqual<string>("Woods", names[1]);
        }

        [TestMethod]
        public void ParseHeader_ValidLastFirstFormat_ReturnFirstLast()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader("Name: Woods, Grant");

            Assert.AreEqual<string>("Grant", names[0]);
            Assert.AreEqual<string>("Woods", names[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseHeader_InValidLastFirstMissingName_ThrowsArgumentException()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader("Woods, Grant");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseHeader_InValidLastFirstMistypedLabel_ThrowsArgumentException()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader("Nam: Woods, Grant");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseHeader_InValidLastFirstOnlyColonLabel_ThrowsArgumentException()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader(": Woods, Grant");
        }
        [TestMethod]
        [DataRow("Name:")]
        [DataRow("Name:           ")]
        [DataRow("Name: Grant")]
        [DataRow("Name: Woods,")]
        [DataRow("Name:  , Grant")]
        public void ParseHeader_InvalidMissingFirstLast_ThrowsArgumentException(string header)
        {
            NameParsingService giftImportService = CreateDefaultImporter();
            try
            {
                string[] names = giftImportService.ParseHeader(header);
            }
            catch (Exception exception) 
            {
                Assert.AreEqual<Type>(typeof(ArgumentException), exception.GetType() );
            }
           
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseHeader_EmptyString_ThrowsArgumentException()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseHeader_NullString_ThrowsArgumentException()
        {
            NameParsingService giftImportService = CreateDefaultImporter();

            string[] names = giftImportService.ParseHeader(null);
        }
    }
}
