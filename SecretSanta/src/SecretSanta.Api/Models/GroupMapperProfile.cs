using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Models
{
    public class GroupMapperProfile : Profile
    {
        public GroupMapperProfile()
        {
            CreateMap<Group, GroupViewModel>();
            CreateMap<Group, GroupInputViewModel>();
            CreateMap<GroupInputViewModel, Group>();
            CreateMap<GroupViewModel, GroupInputViewModel>();
        }
    }
}
