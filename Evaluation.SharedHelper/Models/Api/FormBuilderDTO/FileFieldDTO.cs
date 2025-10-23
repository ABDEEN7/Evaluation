using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FileFieldDTO
    {

        public Guid? FieldId { get; set; }
        public Guid? childFieldId { get; set; }
        public Guid? Index { get; set; }
        public IFormFile File { get; set; } = null!;

        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] FileBytes { get; set; } = null!;
    }
}
