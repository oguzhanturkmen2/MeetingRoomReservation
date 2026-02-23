using FluentValidation;
using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<CreateUpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }

}
