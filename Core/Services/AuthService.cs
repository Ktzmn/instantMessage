using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Jw;
using Core.Jwt;
using Core.Repository.Contracts;
using Core.Security;
using Core.Services.Contracts;
using Data.DataTransfer;
using Data.Entities;
using Data.Utilities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services
{
    public class AuthService : IAuthService
    {   
        private readonly IAuthRepository _repository;
        private readonly JwtService _jwtService;
        private readonly EncryptionService _encryption;

        public AuthService(IAuthRepository repository, JwtService jwtService, EncryptionService encryption) {
            _repository = repository;
            _jwtService = jwtService;
            _encryption = encryption;   
        }

        public async Task<RequestResult> RegisterUserAsync(CreateUserRequest request)
        {
            if (request == null) 
            {
                return RequestResult.Failure(new("Invalid credentials"));
            }

            if (await _repository.IsPresent(request.Username)) 
            {
                return RequestResult.Failure(new($"Account with username {request.Username} already exists"));
            }

            var hashResult = _encryption.GenerateHashWithSalt(request.Password,KeyDerivationPrf.HMACSHA256);

            var user = new User {
                Username = request.Username,
                Password = hashResult["hashedPassword"],
                Salt = hashResult["salt"]
            };

            await _repository.AddUserAsync(user);

            return RequestResult<string>.Success(user.Username);
        }

        public async Task<RequestResult> AuthenticateAsync(AuthUserRequest request)
        {
            var user = await _repository.FindUserByNameAsync(request.Username);

            if (user == null)
            {
                return RequestResult.Failure(new ("Could not find account with credentials provided"));
            }

            if (!_encryption.VerifyHashMatch(user, request.Password))
            {   
                return RequestResult.Failure(new ("Invalid credentials provided"));
            }

            var token = _jwtService.GenerateTBearerToken(user);

            return RequestResult<string>.Success(token);
        }

      



    }
}