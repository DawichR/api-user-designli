using MediatR;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Application.Queries.Access
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IRepository<Userapp> _userRepository;

        public GetAllUsersQueryHandler(IRepository<Userapp> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserResponse(
                u.Id,
                u.Username,
                u.Email,
                u.Name,
                u.LastName
            )).ToList();
        }
    }
}
