using System.Text;
using Spire.Doc.Documents;
using SpireDocument = Spire.Doc.Document;
using SpireFileFormat = Spire.Doc.FileFormat;
using AsposeDocument = Aspose.Words.Document;
using AsposeSaveFormat = Aspose.Words.SaveFormat;
using Evaluation.DAL.Models.Template;

namespace Evaluation.Services.BusinessLayer.API.Template
{
    public class DocumentConversionService : ApiServiceBase
    {
        public byte[]? ConvertDocxToPdfSpire(byte[] docxByteArray)
        {
            if (docxByteArray is not { Length: > 0 }) return null;

            using var inputStream = new MemoryStream(docxByteArray);
            var document = new SpireDocument();
            document.LoadFromStream(inputStream, SpireFileFormat.Docx);

            using var outputStream = new MemoryStream();
            document.SaveToStream(outputStream, SpireFileFormat.PDF);
            return outputStream.ToArray();
        }
        
        public byte[]? ConvertDocxToPdfAspose(byte[] docxByteArray)
        {
            if (docxByteArray is not { Length: > 0 }) return null;

            using var inputStream = new MemoryStream(docxByteArray);
            var document = new AsposeDocument(inputStream);

            using var outputStream = new MemoryStream();
            document.Save(outputStream, AsposeSaveFormat.Pdf);

            return outputStream.ToArray();
        }
        
        public Task<byte[]> HandleNonAttachmentSpire(List<PlaceholderDto> placeholders, TemplateDocument template, string lang)
        {
            var html = BuildHtmlWithPlaceholders(placeholders, template, lang);

            using var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            var document = new SpireDocument();
            document.LoadFromStream(htmlStream, SpireFileFormat.Html, XHTMLValidationType.None);

            using var pdfStream = new MemoryStream();
            document.SaveToStream(pdfStream, SpireFileFormat.PDF);

            return Task.FromResult(pdfStream.ToArray());
        }
        
        public Task<byte[]> HandleNonAttachmentAspose(List<PlaceholderDto> placeholders, TemplateDocument template, string lang)
        {
            var html = BuildHtmlWithPlaceholders(placeholders, template, lang);

            using var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            var document = new AsposeDocument(htmlStream);

            using var pdfStream = new MemoryStream();
            document.Save(pdfStream, AsposeSaveFormat.Pdf);

            return Task.FromResult(pdfStream.ToArray());
        }
        
        private static string BuildHtmlWithPlaceholders(List<PlaceholderDto> placeholders, TemplateDocument template, string lang)
        {
            var html = (lang == "ar" ? template.TemplateAr : template.TemplateEn) ?? string.Empty;

            return placeholders.Aggregate(html, (current, placeholder) =>
                string.IsNullOrEmpty(placeholder.Key)
                    ? current
                    : current.Replace(placeholder.Key, placeholder.Value ?? string.Empty));
        }
    }
}