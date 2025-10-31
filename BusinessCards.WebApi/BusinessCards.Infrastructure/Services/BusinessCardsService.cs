using BusinessCards.Application.Dtos;
using BusinessCards.Application.Interfaces;
using BusinessCards.Domain.Entities;
using BusinessCards.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
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

            await appDbContext.BusinessCards.AddAsync(
                new BusinessCard { 
                    Name = BusinessCardRequest.Name,
                    Gender = BusinessCardRequest.Gender,
                    Address = BusinessCardRequest.Address,
                    CreatedAt = DateTime.UtcNow,
                    DateOfBirth = BusinessCardRequest.DateOfBirth,
                    Email = BusinessCardRequest.Email,
                    Phone = BusinessCardRequest.Phone,
                    Photo = BusinessCardRequest.Photo
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
                    Photo = card.Photo,
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
                    Photo = card.Photo,
                })
                .FirstOrDefaultAsync();
            return card;
        }
    }
}
