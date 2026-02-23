using FluentValidation;
using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Validators
{
    public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationDtoValidator()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0);

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Rezervasyon geçmiş tarihli olamaz.");
        }
    }
}
