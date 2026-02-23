using FluentValidation;
using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Validators
{
    public class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
    {
        public CreateRoomDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Capacity)
                .GreaterThan(0);
        }
    }
}
