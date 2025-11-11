using BusinessCards.Application.Dtos;
using BusinessCards.Application.Interfaces;
using BusinessCards.Domain.Entities;
using BusinessCards.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Infrastructure.Services
{
    public class BusinessCardsService : IBusinessCardsService
    {
        private readonly ILogger<BusinessCardsService> _logger;
        private readonly AppDbContext appDbContext;

        public BusinessCardsService(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }
        public async Task CreateCard(BusinessCardRequestDto BusinessCardRequest)
        {
            if (BusinessCardRequest is null)
                return;
            byte[]? photoBytes = null;
            if (!BusinessCardRequest.Photo.IsNullOrEmpty()) {
                var padding = BusinessCardRequest.Photo!.Count(c => c == '=');
                var decodedLen = (BusinessCardRequest.Photo!.Length * 3 / 4) - padding;
                if (decodedLen > 1_000_000)
                    throw new InvalidOperationException("Photo exceeds 1MB.");
                photoBytes = Convert.FromBase64String(BusinessCardRequest.Photo);
            }
            await appDbContext.BusinessCards.AddAsync(
                new BusinessCard { 
                    Name = BusinessCardRequest.Name,
                    Gender = BusinessCardRequest.Gender,
                    Address = BusinessCardRequest.Address,
                    CreatedAt = DateTime.UtcNow,
                    DateOfBirth = BusinessCardRequest.DateOfBirth,
                    Email = BusinessCardRequest.Email,
                    Phone = BusinessCardRequest.Phone,
                    Photo = photoBytes
                });

            await appDbContext.SaveChangesAsync();

        } 

        public async Task<bool> DeleteCardAsync(Guid id, CancellationToken ct = default)
        {
            var affected = await appDbContext.BusinessCards
          .Where(x => x.Id == id)
          .ExecuteDeleteAsync(ct);

            await appDbContext.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<List<BusinessCardRequestDto>> GetAllCardsAsync()
        {
            return await appDbContext.BusinessCards
                .AsNoTracking()
                .Select(card => new BusinessCardRequestDto
                {
                    Id = card.Id,
                    Name = card.Name,
                    Address = card.Address,
                    DateOfBirth = card.DateOfBirth,
                    Email = card.Email,
                    Gender = card.Gender,
                    Phone = card.Phone,
                    Photo = card.Photo != null ? Convert.ToBase64String(card.Photo) : null,
                }).ToListAsync();

        }

        public async Task<BusinessCardRequestDto> GetCard(Guid id)
        {
            var card = await appDbContext.BusinessCards
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(card => new BusinessCardRequestDto
                {
                    Id = card.Id,
                    Name = card.Name,
                    Address = card.Address,
                    DateOfBirth = card.DateOfBirth,
                    Email = card.Email,
                    Gender = card.Gender,
                    Phone = card.Phone,
                    Photo = card.Photo != null ? Convert.ToBase64String(card.Photo) : null,
                })
                .FirstOrDefaultAsync();
            return card;
        }
    }
}
