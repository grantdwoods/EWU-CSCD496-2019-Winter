using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Import.Services;
using System;
using System.IO;
using System.Text;

namespace SecretSanta.Import.Tests
{
    [TestClass]
    public class GiftImportServiceTest
    {
        string dirPath;
        string tmpFilePath;

        [TestInitialize]
        public void SetUpDirectory()
        {
            string tempDirPath = Path.GetTempPath();
            dirPath = Path.Combine(tempDirPath + "SecretSantaGiftImport");

            if (Directory.Exists(dirPath))
                CleanUpDirectory();

            Directory.CreateDirectory(dirPath);
            tmpFilePath = Path.Combine(dirPath, "userGifts.tmp");
        }
        [TestCleanup]
        public void CleanUpDirectory()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            foreach(FileInfo file in dirInfo.GetFiles())
            {
                File.Delete(file.FullName);
            }
            Directory.Delete(dirPath);
        }
        [TestMethod]
        public void ImportGifts_Valid_Format()
        {
            GiftImportService giftImportService = new GiftImportService();

            string[] wishList = 
            {
                "Name: Grant Woods",
                "Gift1",
                "Gift2",
                "Gift3"
            };
            File.WriteAllLines(tmpFilePath, wishList);

            //giftImportService.ImportGifts(tmpFilePath);

            File.Delete(tmpFilePath);
        }


    }
}