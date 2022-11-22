using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.AuoProfiler
{
    public class PlatformsProfiles : Profile
    {
        public PlatformsProfiles()
        {
            CreateMap<Platform,PlatformReadDTO>();
            CreateMap<PlatformCreateDTO,Platform>();
        }
    }
}