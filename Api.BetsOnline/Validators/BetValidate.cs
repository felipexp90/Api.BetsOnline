using Entities;
using FluentValidation;

namespace Api.BetsOnline.Validators
{
    public class BetValidate : AbstractValidator<Bet>
    {
        public BetValidate()
        {
            RuleFor(x => x.Number)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(36)
                .When(x => x.Color.Contains(""));

            RuleFor(x => x.MoneyBet)
                .GreaterThan(0)
                .LessThanOrEqualTo(10000);
        }
    }
}
