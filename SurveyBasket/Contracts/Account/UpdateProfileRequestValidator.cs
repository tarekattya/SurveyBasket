namespace SurveyBasket.Contracts.Account
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            When(x => x.ProfileImage != null, () =>
            {
                RuleFor(x => x.ProfileImage)
                    .Must(f => f!.Length! <= 2 * 1024 * 1024).WithMessage("أقصى حجم للصورة هو 2 ميجا")
                    .Must(f => IsValidContentType(f!.ContentType)).WithMessage("نوع الملف يجب أن يكون JPEG أو PNG");
            });

            RuleFor(x => x.FirstName)
              .NotEmpty()
              .Length(3, 100);

            RuleFor(x => x.LastName)
               .NotEmpty()
               .Length(3, 100);

        }



        private bool IsValidContentType(string contentType)
        {
            return contentType == "image/jpeg" || contentType == "image/png";
        }
    }

    }

