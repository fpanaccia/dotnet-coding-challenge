using Dotnet.Challenge.Data.Cache;
using Dotnet.Challenge.Domain.Aggregates.User;
using Dotnet.Challenge.Domain.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dotnet.Challenge.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;

        public UserService(IUserRepository userRepository, IValidator<User> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<User> CreateAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userOnDb = await _userRepository.FindByEmailAsync(user.Email);
            if (userOnDb != null)
            {
                throw new UserAlreadyExistException();
            }

            return await _userRepository.AddAsync(user);
        }

        public async Task DeletAsync(Guid userId)
        {
            var userOnDb = await _userRepository.FindByIdAsync(userId);
            if (userOnDb != null)
            {
                await _userRepository.DeleteAsync(userOnDb);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var userOnDb = await _userRepository.FindByIdAsync(userId);
            if (userOnDb is null)
            {
                return null;
            }

            return userOnDb;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userOnDb = await _userRepository.FindByIdAsync(user.Id);
            if (userOnDb is null)
            {
                return null;
            }

            var userOnDbByEmail = await _userRepository.FindByEmailAsync(user.Email);
            if (userOnDbByEmail != null && userOnDbByEmail.Id != user.Id)
            {
                throw new UserAlreadyExistException();
            }

            return await _userRepository.UpdateAsync(user);
        }
    }
}
