using Dotnet.Challenge.Application.Commands;
using Dotnet.Challenge.Domain.Aggregates.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Dotnet.Challenge.Domain.Exceptions;
using System.Linq;

namespace Dotnet.Challenge.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <returns>A newly created user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users
        ///     {
        ///        "firstName": "Fabricio",
        ///        "lastName": "Panaccia",
        ///        "email": "fake_email@gmail.com",
        ///        "dateOfBirth": "1984-10-24T22:17:32.975Z"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">>Returns the newly created user</response>
        /// <response code="400">If the creation fails for some validation</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateUser(CreateUser user)
        {
            try
            {
                var createdUser = await _userService.CreateAsync(user.ToDomain());
                return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                        .Select(failure => new
                        {
                            Property = failure.PropertyName,
                            Error = failure.ErrorMessage
                        });

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }
            catch (UserAlreadyExistException)
            {
                return BadRequest("Email already exists");
            }
        }

        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns list of users</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Returns a specific user
        /// </summary>
        /// <returns>A user</returns>
        /// <response code="200">Returns a user</response>
        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">>If user is deleted</response>
        /// <response code="404">If the user does not exists</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeletAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Updates a specific user
        /// </summary>
        /// <returns>A user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "firstName": "Fabricio",
        ///        "lastName": "Panaccia",
        ///        "email": "fake_email@gmail.com",
        ///        "dateOfBirth": "1984-10-24T22:17:32.975Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">>Returns the user updated</response>
        /// <response code="400">If the user is invalid</response>
        /// <response code="404">If the user does not exists</response>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> PutUser(Guid id, UpdateUser user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID does not correspond with id in object");
            }

            try
            {
                var updatedUser = await _userService.UpdateAsync(user.ToDomain());
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                        .Select(failure => new
                        {
                            Property = failure.PropertyName,
                            Error = failure.ErrorMessage
                        });

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }
            catch (UserAlreadyExistException)
            {
                return BadRequest("Email already exists");
            }
        }
    }
}
