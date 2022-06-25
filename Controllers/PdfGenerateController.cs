using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfGenerate.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfGenerate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfGenerateController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {


                return Ok();
            }
            
            catch (Exception ex)
            {

                var response = new
                {
                    Msg = "Processing request failed.",
                    Errorlst = new List<ErrorMessage>() {
                        new ErrorMessage() { Error = ex.Message } }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
