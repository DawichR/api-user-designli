using MediatR;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Application.Commands.Access
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IRepository<Userapp> _userRepository;
        private readonly ITokenProvider _tokenProvider;

        public LoginCommandHandler(IRepository<Userapp> userRepository, ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _tokenProvider.Create(user);

            return new LoginResult(token, user.Username);
        }
    }
}
