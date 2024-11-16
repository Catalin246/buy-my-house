using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;
using BuyMyHouse.DAL;

namespace BuyMyHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly IHouseRepository _houseRepository;

        public HouseController(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        // Get houses in certain price range
        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetHouses([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            // Fetch filtered houses from the repository
            var houses = await _houseRepository.GetHousesInPriceRangeAsync(minPrice, maxPrice);
            
            return Ok(houses);
        }  
    }
}
