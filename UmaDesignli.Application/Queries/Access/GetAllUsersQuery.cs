using MediatR;

namespace UmaDesignli.Application.Queries.Access
{
    public record GetAllUsersQuery : IRequest<List<UserResponse>>;

    public record UserResponse(int Id, string Username, string Email, string Name, string LastName);
}
