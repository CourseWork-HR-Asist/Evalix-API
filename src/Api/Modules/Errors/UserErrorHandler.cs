﻿using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class UserErrorHandler
{
    public static ObjectResult ToObjectResult(this UserException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                UserNotFoundException
                    or UserWithUsernameNotFoundException
                    or UserRoleNotFoundException
                    or UserRefreshTokenNotFoundException => StatusCodes.Status404NotFound,
                UserAlreadyExistsException
                    or UserWithEmailAlreadyExistsException
                    or UserWithUsernameAlreadyExistsException 
                    or UserLoginWithGoogleExistsException => StatusCodes.Status409Conflict,
                UserUnknownException => StatusCodes.Status500InternalServerError,
                UserIncorrectPasswordException 
                    or UserLoginWithGoogleNotAcceptedException => StatusCodes.Status401Unauthorized,
                _ => throw new NotImplementedException("User error handler does not implemented")
            }

        };
    }
}