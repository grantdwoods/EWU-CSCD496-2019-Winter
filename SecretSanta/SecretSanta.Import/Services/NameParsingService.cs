using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Import.Services
{
    public class NameParsingService
    {
        public string[] ParseHeader(string header)
        {
            if(header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if(!header.StartsWith("Name: "))
            {
                throw new ArgumentException(nameof(header), 
                    "Header must start with \"Name: /"");
            }
        
            string fullName = ParseFullName(header);
            string[] names = separateFirstAndLast(fullName);

            if (!CheckNamesArray(names))
            {
                throw new ArgumentException(nameof(header), 
                    "first or last name missing.");
            }

            return names;
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
                SwapToFirstLastFormat(names);
            }
            else
            {
                names = fullName.Split(" ");
            }
            TrimNameArray(names);
            return names;
        }

        private void TrimNameArray(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = names[i].Trim();
            }
        }

        private void SwapToFirstLastFormat(string[] names)
        {
            string temp = names[0];
            names[0] = names[1];
            names[1] = temp;
        }
    }
}
