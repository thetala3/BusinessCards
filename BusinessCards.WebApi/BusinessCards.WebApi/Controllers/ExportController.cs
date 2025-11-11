using BusinessCards.Application.Dtos;
using BusinessCards.Application.Interfaces;
using BusinessCards.Domain.Entities;
using BusinessCards.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCards.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _svc;

        public ExportController(IExportService svc)
        {
            _svc = svc;
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportCsv()
        {
            var (content, contentType, fileName) = await _svc.ExportCsvFilesAsync();
            return File(content, contentType, fileName);
        }

        [HttpGet("export/xml")]
        public async Task<IActionResult> ExportXml()
        {
            var (content, contentType, fileName) = await _svc.ExportXmlFilesAsync();
            return File(content, contentType, fileName);
        }

    }
}
