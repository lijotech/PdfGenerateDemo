using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfGenerate.DTO;
using PdfGenerate.Extensions;
using PdfGenerate.Services;
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
        private readonly IPdfGenerateService _pdfGenerateService;
        
        public PdfGenerateController(IPdfGenerateService pdfGenerateService)
        {
            _pdfGenerateService = pdfGenerateService;
        }

        [HttpGet("GeneratePdfFirstCase")]
        public async Task<IActionResult> GeneratePdfFirstCase()
        {
            try
            {
                var result = await _pdfGenerateService.GeneratePdfFirstCase(Utility.GenerateDatatableWithData(4,3));

                return File(result.Attachment, result.MimeType, result.FileName);               
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
