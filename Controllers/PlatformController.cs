using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepo repository,
        IMapper mapper,
        ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms(){
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }

        [HttpPost("{ID}", Name = "GetPlatformByID")]
        public  ActionResult<IEnumerable<PlatformReadDTO>> GetPlatformByID(int ID){
            var platformItem = _repository.GetPlatformById(ID);
            if(platformItem!=null){
               return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            }

           return NotFound(); 
        }
        
        [HttpPost]
        [Route("CreatePlatform")]
        public async Task<ActionResult<IEnumerable<PlatformReadDTO>>> CreatePlatformAsync(PlatformCreateDTO platform){
            if(platform != null){
                var newPlatform = _mapper.Map<Platform>(platform);
                _repository.CreatePlatform(newPlatform);
                _repository.SaveChanges();
                var platformReadDTO = _mapper.Map<PlatformReadDTO>(newPlatform);
                try{
                    await _commandDataClient.SendPlatformToCommand(platformReadDTO);
                }
                catch(Exception ex){
                    Console.WriteLine($"Error Message -->{ex.Message}");
                }
                return CreatedAtRoute(nameof(GetPlatformByID),new {ID=platformReadDTO.ID},platformReadDTO);
            }

            return NotFound();
        }

        
    }
}