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

        [HttpGet]
        public IActionResult Get()
        {
            _valueRepository.LazyInitializer(); // Bu satırı eklemek gerekmeyebilir, bağlamıza bağlıdır
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

            // Güncelleme işlemi için model oluştur ve varolan değerin özelliklerini kopyala
            var updatedValue = new ValueUpdateModel
            {
                Id = id,
                Name = model.Name
            };

            valueRepository.UpdateValue(updatedValue); // Güncellenen değeri repository'e gönder

            return NoContent(); // 204 No Content response
        }

    }
}
