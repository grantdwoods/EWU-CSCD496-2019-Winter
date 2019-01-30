using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.DTO
{
    public class Group
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Group()
        {

        }
        
        public Group(Domain.Models.Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }

        public static Domain.Models.Group DtoToDomain(DTO.Group group)
        {
            return new Domain.Models.Group { Name = group.Name, Id = group.Id };
        }
    }
}
