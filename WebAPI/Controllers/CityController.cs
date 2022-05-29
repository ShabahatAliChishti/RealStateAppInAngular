using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //to protect all endpoints
    [Authorize]
    public class CityController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(IUnitOfWork uow,IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            throw new UnauthorizedAccessException();
            //return new string[] { "atlanta", "ocean","Boston" };
            var cities = await uow.CityRepository.GetCitiesAsync();
            //var citiesDto = from c in cities
            //                select new CityDto()
            //                {
            //                    Id = c.Id,
            //                    Name = c.Name
            //                };
            //we do mapping through automapper
            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);
            return Ok(citiesDto);
        }
        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            //mapping CityDto to city because city repo expected city repository
            //var city = new City
            //{
            //    Name = cityDto.Name,
            //    LastUpdatedBy = 1,
            //    LastUpdatedOn=DateTime.Now

            //};
            var city = mapper.Map<City>(cityDto);
            city.LastUpdatedBy = 1;
            city.LastUpdatedOn = DateTime.Now;
            uow.CityRepository.AddCity(city);
            await uow.SaveAsync();
            return StatusCode(201);
                
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id,CityDto cityDto)
        {
            if (id != cityDto.Id)
                return BadRequest("Update not allowed");

            var cityFromDb = await uow.CityRepository.FindCity(id);
            if (cityFromDb == null)
                return BadRequest("Update not allowed");
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LastUpdatedBy = 1;
            mapper.Map(cityDto, cityFromDb);
            throw new Exception("Some unknown error occured");
            await uow.SaveAsync();
            return StatusCode(200);

        }
        //partial update dto for cityname
        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateCityName(int id, CityUpdateDto cityNameDto)
        {
            var cityFromDb = await uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LastUpdatedBy = 1;
            mapper.Map(cityNameDto, cityFromDb);
            await uow.SaveAsync();
            return StatusCode(200);

        }
        //it is not recommended approach
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> cityToPatch)
        {
            var cityFromDb = await uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdatedOn = DateTime.Now;
            cityFromDb.LastUpdatedBy = 1;
            cityToPatch.ApplyTo(cityFromDb,ModelState);
            await uow.SaveAsync();
            return StatusCode(200);

        }
        [HttpDelete("delete/{id}")]
       public async Task<IActionResult> DeleteCity(int id)
        {
            uow.CityRepository.DeleteCity(id);
            await uow.SaveAsync();
            return Ok(id);
        }
    }
}
