using BiancasBikeShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiancasBikeShop.Models;
using System.Security.Claims;

namespace BiancasBikeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private IBikeRepository _bikeRepo;

        public BikeController(IBikeRepository bikeRepo)
        {
            _bikeRepo = bikeRepo;
        }

        [HttpGet]
        public IActionResult Get()
         {
            
             return Ok(_bikeRepo.GetAllBikes());
         }

        [HttpGet("{id}")]
         public IActionResult GetById(int id)
         {
             
             return Ok(_bikeRepo.GetBikeById(id));
         }

        [HttpGet("count")]
         public IActionResult GetBikesInShopCount()
         {
             return Ok(_bikeRepo.GetBikesInShopCount());
         }
    }
}
