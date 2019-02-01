using System;
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
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Id = group.Id;
            Name = group.Name;
        }

        public static Domain.Models.Group DtoToDomain(DTO.Group group)
        {
            return new Domain.Models.Group { Name = group.Name, Id = group.Id };
        }
    }
}
