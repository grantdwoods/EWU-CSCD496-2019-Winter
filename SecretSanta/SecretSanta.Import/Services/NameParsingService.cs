using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Import.Services
{
    public class NameParsingService
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
                string[] names = separateFirstAndLast(fullName);
                if (CheckNamesArray(names))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckNamesArray(string[] names)
        {
            bool isValid = true;
            if(names.Length !=2 )
            {
                isValid = false;
            }

            foreach(string name in names)
            {
                if (string.IsNullOrEmpty(name))
                {
                    isValid = false;
                }
            }
            return isValid;
        }
        private string ParseFullName(string header)
        {
            string fullName = header.Substring(header.IndexOf(':') + 1);
            fullName = fullName.Trim();
            return fullName;
        }
        private string[] separateFirstAndLast(string fullName)
        {
            string[] names;

            if (fullName.Contains(','))
            {
                names = fullName.Split(',');
            }
            else
            {
                names = fullName.Split(" ");
            }
            return names;
        }
    }
}
