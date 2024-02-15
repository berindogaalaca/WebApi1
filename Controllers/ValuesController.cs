using Microsoft.AspNetCore.Mvc;
using WebApi1.Models;
using WebApi1.Services;

namespace WebApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValueRepository _valueRepository;

        public ValuesController(ValueRepository valueRepository)
        {
            _valueRepository = valueRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Value model, ValueRepository valueRepository)
        {
            if (model == null)
            {
                return BadRequest("Invalid model");
            }

            var createValue = new Value
            {
       
                Name = model.Name
            };

            valueRepository.CreateValue(createValue);

            return NoContent();
        }
        [HttpGet]
        public IActionResult Get()
        { 
            var values = _valueRepository.GetAllValues();
            return Ok(values);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Value>> DeleteValue(int id, ValueRepository valueRepository)
        {
            try
            {
                var valueToDelete = await valueRepository.GetValue(id);

                if (valueToDelete == null)
                {
                    return NotFound($"Value with Id = {id} not found");
                }

                await valueRepository.DeleteValue(id);
                return valueToDelete;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ValueUpdateModel model, ValueRepository valueRepository)
        {
            if (model == null)
            {
                return BadRequest("Invalid model");
            }

            var existingValue = await valueRepository.GetValue(id);

            if (existingValue == null)
            {
                return NotFound();
            }

            var updatedValue = new ValueUpdateModel
            {
                Id = id,
                Name = model.Name
            };

            valueRepository.UpdateValue(updatedValue); 

            return NoContent(); 
        }

    }
}
