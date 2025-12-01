using MediatR;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Application.Queries.Access
{
    /// <summary>
    /// Handler for getting all users - returns list of users for protected endpoint
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IRepository<User> _userRepository;

        public GetAllUsersQueryHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // Get all users from repository
            var users = await _userRepository.GetAllAsync();

            // Map to response DTOs (excluding sensitive password data)
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
