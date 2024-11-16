using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BuyMyHouse.Models;
using BuyMyHouse.DAL;

namespace BuyMyHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Update customer's financial information
        [HttpPut("{customerId}/financial-info")]
        public async Task<IActionResult> UpdateFinancialInfo(int customerId, [FromBody] FinancialInformation model)
        {
            try
            {
                if (model == null || model.CreditScore <= 0 || model.Income < 0)
                {
                    return BadRequest(new { message = "Invalid financial information provided." });
                }

                // Update customer financial information
                await _customerRepository.UpdateCustomerFinancialInfoAsync(customerId, model.Income.Value, model.CreditScore.Value);

                return Ok(new { message = "Customer financial information updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating financial information.", details = ex.Message });
            }
        }
    }
}
