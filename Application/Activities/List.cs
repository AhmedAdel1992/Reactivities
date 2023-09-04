using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        public class Hanlder : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Hanlder(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                ////Eager Loading Method for loading related data
                //var activities = await _context.Activities
                //    .Include(a => a.Attendees)
                //    .ThenInclude(u => u.AppUser)
                //    .ToListAsync(cancellationToken);

                //Projection Method for loading related data
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                ////we need this next line if we are using eager loading method
                //var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

                //if we are using eager loading method we return activitiesToReturn in Success below
                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }

}
