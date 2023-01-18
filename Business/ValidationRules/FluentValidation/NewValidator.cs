using Core.Entities.Concrete;
using Core.Entities.DTOs;
using FluentValidation;
using System;

namespace Business.ValidationRules.FluentValidation
{
    public class NewAddDtoValidator : AbstractValidator<NewAddDto>
    {
        public NewAddDtoValidator()
        {

            RuleFor(n => n.Summary).NotEmpty().WithMessage("Haber adý boþ geçilemez.");
            RuleFor(n => n.Summary).Length(3, 200).WithMessage("Haber özeti 3 ile 500 karakter arasýnda olmalýdýr.");

            RuleFor(n => n.Content).NotEmpty().WithMessage("Haber içeriði boþ geçilemez.");
            RuleFor(n => n.UrlLink).NotEmpty().WithMessage("Haber linki boþ geçilemez.");
            RuleFor(n => n.Summary).MaximumLength(200).WithMessage("Haber linki 200 karakter veya daha kýsa olmalýdýr.");
            //Özel kurallarý böyle çaðýrýrým.
            //RuleFor(p => p.BrandName).Must(MyRule);
        }

        // Özel kurallar yazabilirim
        private bool MyRule(string arg)
        {
            return arg.StartsWith("a");
        }
    }
}
        