using BusinessCards.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Application.Validation
{
    public class UpsertBusinessCardValidator : AbstractValidator<BusinessCardRequestDto>
    {
        //This is for validating the input data when creating or updating a business card.
        public UpsertBusinessCardValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(200);
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required").MaximumLength(50);
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of Birth is required").LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format").MaximumLength(200);
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required").MaximumLength(20);
            RuleFor(x => x.Address).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Address));
            RuleFor(x => x.Photo)
                .Must(photo =>
                {
                    if (photo == null) return true;
                    return photo.Length <= 1_000_000;
                })
                .WithMessage("Photo must be ≤ 1MB (decoded).");
        }
    }
}
