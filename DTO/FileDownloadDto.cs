using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfGenerate.DTO
{    
    public class FileDownloadDto
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Attachment { get; set; }
    }
}
