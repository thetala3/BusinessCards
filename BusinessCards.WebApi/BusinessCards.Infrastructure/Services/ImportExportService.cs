using BusinessCards.Application.Dtos;
using BusinessCards.Application.Interfaces;
using BusinessCards.Domain.Entities;
using BusinessCards.Infrastructure.Persistence;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessCards.Infrastructure.Services
{
    public class ImportExportService : IImportExportService
    {
        private readonly AppDbContext appContext;
        public ImportExportService(AppDbContext _appContext)
        {
            appContext = _appContext;
        }
        public async Task<(byte[] Content, string ContentType, string FileName)> ExportCsvFilesAsync()
        {
            var all = await appContext.BusinessCards.AsNoTracking().ToListAsync();
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms, Encoding.UTF8, leaveOpen: true);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(all.Select(ToCsv));
            await writer.FlushAsync();
            return (ms.ToArray(), "text/csv", $"business-cards-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");

            static BusinessCardRequestDto ToCsv(BusinessCard e) => new()
            {
                Name = e.Name,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                Phone = e.Phone,
                Photo = e.Photo != null ? Convert.ToBase64String(e.Photo) : null,
                Address = e.Address
            };
        }

        public async Task<(byte[] Content, string ContentType, string FileName)> ExportXmlFilesAsync()
        {
            var all = await appContext.BusinessCards.AsNoTracking().ToListAsync();
            var env = all.Select(e => new BusinessCardRequestDto
            {
                Name = e.Name,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                Phone = e.Phone,
                Photo = e.Photo != null ? Convert.ToBase64String(e.Photo) : null,
                Address = e.Address
            }).ToList();

            var ser = new XmlSerializer(typeof(List<BusinessCardRequestDto>));
            using var ms = new MemoryStream();
            ser.Serialize(ms, env);
            return (ms.ToArray(), "application/xml", $"business-cards-{DateTime.UtcNow:yyyyMMddHHmmss}.xml");
        }

        public async Task<List<BusinessCardRequestDto>> ParseCsvFileAsync(Stream csv)
        {
            using var reader = new StreamReader(csv, Encoding.UTF8, leaveOpen: true);

            var CsvConfig = CsvConfiguration.FromAttributes<BusinessCardRequestDto>();
            using var csvr = new CsvReader(reader, CsvConfig);

            var rows = new List<BusinessCardRequestDto>();
            await foreach (var row in csvr.GetRecordsAsync<BusinessCardRequestDto>()) 
            {
                rows.Add(row);
            }

            return rows;
        }

        public async Task<List<BusinessCardRequestDto>> ParseXmlFilesAsync(Stream xml)
        {
            var serializer = new XmlSerializer(typeof(List<BusinessCardRequestDto>));
            var rows = serializer.Deserialize(xml) as List<BusinessCardRequestDto>;
            return await Task.FromResult(rows ?? new List<BusinessCardRequestDto>());
        }
    }
}
