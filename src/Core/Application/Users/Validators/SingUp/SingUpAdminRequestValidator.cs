using FluentValidation;
using Shifty.Application.Users.Requests.SingUp;
using System.Text.RegularExpressions;

namespace Shifty.Application.Users.Validators.SingUp
{
    public class SingUpAdminRequestValidator : AbstractValidator<SingUpAdminRequest>
    {
        public SingUpAdminRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("نام الزامیست").Length(2, 255).WithMessage("حداقل طول نام 2 حرف و حداکثر 255 حرف می‌باشد");

            RuleFor(x => x.LastName).
                NotEmpty().
                WithMessage("نام خانوادگی الزامیست").
                Length(2, 255).
                WithMessage("حداقل طول نام خانوادگی 2 حرف و حداکثر 255 حرف می‌باشد");

            RuleFor(x => x.FatherName).NotEmpty().WithMessage("نام پدر الزامیست").Length(2, 255).WithMessage("حداقل طول نام پدر 2 حرف و حداکثر 255 حرف می‌باشد");

            RuleFor(x => x.NationalCode).
                NotEmpty().
                WithMessage("کد ملی الزامیست").
                Length(10).
                WithMessage("طول کد ملی باید 10 عدد باشد").
                Matches(@"^\d+$").
                WithMessage("کد ملی باید فقط شامل اعداد باشد");

            RuleFor(x => x.Gender).NotNull().WithMessage("جنسیت الزامیست");

            RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("شماره همراه الزامیست").Matches(@"^09\d{9}$").WithMessage("فرمت شماره همراه صحیح نمی‌باشد");

            // RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور الزامیست").MinimumLength(6).WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد");
            //
            // RuleFor(x => x.ConfirmPassword).
            //     NotEmpty().
            //     WithMessage("تایید رمز عبور الزامیست").
            //     Equal(x => x.Password).
            //     WithMessage("رمز عبور و تایید آن باید یکسان باشند");
        }

        private bool BeAValidDomain(string website)
        {
            return true;
            var forbiddenCharsPattern = @"[^a-zA-Z0-9\-\.]";
            return Regex.IsMatch(website, forbiddenCharsPattern) && website.Contains('.');
        }
    }
}