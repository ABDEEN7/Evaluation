using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Helper
{
    public class FileVm
    {
        [JsonProperty("fileId", NullValueHandling = NullValueHandling.Ignore)]
        public long FileId { get; set; }

        [JsonProperty("fileName", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; } = null!;
        [JsonProperty("filePath", NullValueHandling = NullValueHandling.Ignore)]
        public string FilePath { get; set; } = null!;
        [JsonProperty("customFileName", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomFileName { get; set; } = null!;
        [JsonProperty("fileType", NullValueHandling = NullValueHandling.Ignore)]
        public string FileType { get; set; } = null!;

        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; } = null!;

        [JsonProperty("fileLength", NullValueHandling = NullValueHandling.Ignore)]
        public long? FileLength { get; set; }
        public IFormFile File { get; set; } = null!;
        public string FolderPath { get; set; } = null!;
        [JsonProperty("titlrAr", NullValueHandling = NullValueHandling.Ignore)]
        public string TitlrAr { get; set; } = null!;
        [JsonProperty("titlrEn", NullValueHandling = NullValueHandling.Ignore)]
        public string TitlrEn { get; set; } = null!;
        [JsonProperty("instanceFileName", NullValueHandling = NullValueHandling.Ignore)]
        public string InstanceFileName { get; set; } = null!;
        [JsonProperty("storageContainerType", NullValueHandling = NullValueHandling.Ignore)]
        public StorageContainerType StorageContainerType { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        [JsonProperty("fieldId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? FieldId { get; set; }
        public Guid? ChildFieldId { get; set; }
        public string? Index { get; set; }


    }
}
