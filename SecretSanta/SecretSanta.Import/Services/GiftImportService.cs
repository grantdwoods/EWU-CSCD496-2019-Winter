using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Import.Services
{
    public class GiftImportService
    {
        public NameParsingService NameParsingService { get; }
        public GiftImportService()
        {
            this.NameParsingService = new NameParsingService();
        }
        
        public void ImportGifts(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
