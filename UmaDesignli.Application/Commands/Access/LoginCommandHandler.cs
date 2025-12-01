using MediatR;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Application.Commands.Access
{
    /// <summary>
    /// Handler for login command - validates credentials and generates JWT token
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITokenProvider _tokenProvider;

        public LoginCommandHandler(IRepository<User> userRepository, ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Get all users and find matching credentials
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Generate JWT token for authenticated user
            var token = _tokenProvider.Create(user);

            return new LoginResult(token, user.Username);
        }
    }
}
