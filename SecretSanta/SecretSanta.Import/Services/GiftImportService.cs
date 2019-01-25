using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        
        public ICollection<Gift> ImportGifts(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] firstLast = NameParsingService.ParseHeader(lines[0]);
            User user = new User { FirstName = firstLast[0], LastName = firstLast[1] };

            List<Gift> wishList = new List<Gift>();

            for(int i = 1; i < lines.Length; i++)
            {
                if(!string.IsNullOrWhiteSpace(lines[i]))
                {
                    wishList.Add(new Gift { User = user, Title = lines[i] });
                }
            }
            if(wishList.Count == 0)
            {
                throw new ArgumentException("No gifts were found.");
            }
            return wishList;
        }
    }
}
