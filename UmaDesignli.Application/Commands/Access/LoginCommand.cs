using MediatR;

namespace UmaDesignli.Application.Commands.Access
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;

    public record LoginResult(string Token, string Username);
}
