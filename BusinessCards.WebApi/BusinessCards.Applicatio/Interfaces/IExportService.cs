using BusinessCards.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Application.Interfaces
{
    public interface IExportService
    {
        Task<(byte[] Content, string ContentType, string FileName)> ExportCsvFilesAsync();
        Task<(byte[] Content, string ContentType, string FileName)> ExportXmlFilesAsync();
    }
}
