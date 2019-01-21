using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Import.Services
{
    public class GiftImportService
    {
        public bool ValidateHeader(string header)
        {
            if(header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if(header.StartsWith("Name:"))
            {
                string fullName = ParseFullName(header);
                if (fullName.Length != 0)
                {
                    return true;
                }
            }
            return false;
        }

        private string ParseFullName(string header)
        {
            string fullName = header.Substring(header.IndexOf(':') + 1);
            fullName = fullName.Trim();
            return fullName;
        }
    }
}
