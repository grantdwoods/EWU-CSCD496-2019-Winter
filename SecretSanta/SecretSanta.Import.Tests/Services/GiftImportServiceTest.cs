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

        [TestInitialize]
        public void SetUpDirectory()
        {
            string tempDirPath = Path.GetTempPath();
            dirPath = Path.Combine(tempDirPath + "/SecretSantaGiftImport");

            if (Directory.Exists(dirPath))
                CleanUpDirectory();

            Directory.CreateDirectory(dirPath);
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
        public void Test()
        {
            GiftImportService giftImportService = new GiftImportService();

            string tmpFile = dirPath + "/userGifts.tmp";
            string[] wishList = 
            {
                "Name: Grant Woods",
                "",
                "Gift1",
                "Gift2",
                "Gift3"
            };

            File.WriteAllLines(tmpFile, wishList);

            foreach(string line in File.ReadAllLines(tmpFile))
            {
                string testing = line;
            }
            //Directory.Delete(path);
            File.Delete(tmpFile);
        }


    }
}