using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Application.Dtos
{
    public record BusinessCardDto(
    Guid Id,
    string Name,
    string Gender ,
    DateTime DateOfBirth ,
    string Email ,
    string Phone ,
    byte[]? Photo ,
    string Address);
}
