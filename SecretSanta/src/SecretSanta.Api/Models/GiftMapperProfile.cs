using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Models
{
    public class GiftMapperProfile : Profile
    {
        public GiftMapperProfile()
        {
            CreateMap<Gift, GiftViewModel>();
        }
    }
}
