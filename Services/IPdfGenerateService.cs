using PdfGenerate.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PdfGenerate.Services
{
    public interface IPdfGenerateService
    {
        Task<FileDownloadDto> GeneratePdfFirstCase(DataTable dataTable);
    }
}
