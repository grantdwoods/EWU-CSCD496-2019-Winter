using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Models
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<User, UserInputViewModel>()
                .ForSourceMember(x => x.Id, opt => opt.DoNotValidate());

            CreateMap<UserInputViewModel, User>();

            CreateMap<UserViewModel, UserInputViewModel>();
        }
    }
}
