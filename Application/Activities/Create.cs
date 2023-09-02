using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }    
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Hanlder : IRequestHandler<Command, Result<Unit>>
        {
            private DataContext _context;

            public Hanlder(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                await _context.Activities.AddAsync(request.Activity);
                var reult = await _context.SaveChangesAsync() > 0;
                if (!reult) return Result<Unit>.Failure("Failed to create Activity");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
