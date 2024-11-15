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
        public async Task<ActionResult<IEnumerable<House>>> GetHouses()
        {
            return Ok( await _houseRepository.GetAllAsync());
        }        

        // Out of scoupe for this project
        // [HttpGet("{id}")]
        // public async Task<ActionResult<House>> GetHouse(int id)
        // {
        //     var house = await _houseRepository.GetByIdAsync(id);
        //     if (house == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(house);
        // }

        // [HttpPost]
        // public async Task<ActionResult<House>> CreateHouse(House house)
        // {
        //     await _houseRepository.AddAsync(house);
        //     return CreatedAtAction(nameof(GetHouse), new { id = house.HouseID }, house);
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateHouse(int id, House house)
        // {
        //     if (id != house.HouseID)
        //     {
        //         return BadRequest();
        //     }
        //     await _houseRepository.UpdateAsync(house);
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteHouse(int id)
        // {
        //     var house = await _houseRepository.GetByIdAsync(id);
        //     if (house == null)
        //     {
        //         return NotFound();
        //     }
        //     await _houseRepository.DeleteAsync(id);
        //     return NoContent();
        // }
    }
}
