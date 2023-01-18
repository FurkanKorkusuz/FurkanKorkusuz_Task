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

            RuleFor(n => n.Summary).NotEmpty().WithMessage("Haber ad� bo� ge�ilemez.");
            RuleFor(n => n.Summary).Length(3, 200).WithMessage("Haber �zeti 3 ile 500 karakter aras�nda olmal�d�r.");

            RuleFor(n => n.Content).NotEmpty().WithMessage("Haber i�eri�i bo� ge�ilemez.");
            RuleFor(n => n.UrlLink).NotEmpty().WithMessage("Haber linki bo� ge�ilemez.");
            RuleFor(n => n.Summary).MaximumLength(200).WithMessage("Haber linki 200 karakter veya daha k�sa olmal�d�r.");
            //�zel kurallar� b�yle �a��r�r�m.
            //RuleFor(p => p.BrandName).Must(MyRule);
        }

        // �zel kurallar yazabilirim
        private bool MyRule(string arg)
        {
            return arg.StartsWith("a");
        }
    }
}
        