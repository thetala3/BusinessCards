using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Domain.Entities
{
    public class BusinessCard
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
