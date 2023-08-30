using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Algebra_Calculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressionController : ControllerBase
    {
        [HttpGet("expresison")]
        [ProducesResponseType(typeof(Number), StatusCodes.Status200OK)]
        public IActionResult Factorise(Expression expression)
        {
            return Ok(Factorise(expression));
        }
    }
}
