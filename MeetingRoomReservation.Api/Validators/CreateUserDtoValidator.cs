using FluentValidation;
using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Validators
{ 
    public class CreateUserDtoValidator : AbstractValidator<CreateUpdateUserDto>
    {
        public CreateUserDtoValidator()
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
