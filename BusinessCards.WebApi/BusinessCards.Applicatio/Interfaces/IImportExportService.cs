using BusinessCards.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Application.Interfaces
{
    public interface IImportExportService
    {
        Task<List<BusinessCardRequestDto>> ParseCsvFileAsync(Stream csv);
        Task<List<BusinessCardRequestDto>> ParseXmlFilesAsync(Stream xml);

        Task<(byte[] Content, string ContentType, string FileName)> ExportCsvFilesAsync();
        Task<(byte[] Content, string ContentType, string FileName)> ExportXmlFilesAsync();
    }
}
