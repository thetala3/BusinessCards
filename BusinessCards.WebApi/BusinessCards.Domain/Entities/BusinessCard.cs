using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Domain.Entities
{
    public class BusinessCard
    {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public byte[]? Photo { get; set; }
            public string? Address { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
