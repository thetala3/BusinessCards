using BusinessCards.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Application.Interfaces
{
    public interface IBusinessCardsService
    {
        Task CreateCard(BusinessCardRequestDto upsertBusinessCardRequest);
        Task <List<BusinessCardRequestDto>> GetAllCardsAsync();
        Task <bool>DeleteCardAsync(Guid id, CancellationToken ct = default);
        Task<BusinessCardRequestDto> GetCard(Guid id);
    }
}
