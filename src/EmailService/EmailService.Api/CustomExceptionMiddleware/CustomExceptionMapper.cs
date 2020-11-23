using System;
using System.Collections.Generic;
using System.Net;
using EmailService.Api.Models;
using EmailService.Logic.Saving;

namespace EmailService.Api.CustomExceptionMiddleware
{
    public class CustomExceptionMapper
    {
        private readonly Dictionary<string, ErrorDetails> _errorDetails;
        public CustomExceptionMapper()
        {
            _errorDetails = new Dictionary<string, ErrorDetails>();
            AddException<ToFieldIsNotValidException>(HttpStatusCode.BadRequest, "'To' field is not valid");
            AddException<NoRecipientsException>(HttpStatusCode.BadRequest, "At least one recipient is required");
            AddException<FromFieldIsNotValidException>(HttpStatusCode.BadRequest, "'From' field is not valid");
        }

        private void AddException<T>(HttpStatusCode statusCode, string message) where T : Exception
        {
            _errorDetails.Add(typeof(T).FullName, new ErrorDetails { Message = message, StatusCode = (int)statusCode });
        }

        public ErrorDetails MapErrorToErrorDetails(Exception exception)
        {
            if (_errorDetails.TryGetValue(exception.GetType().FullName, out ErrorDetails errorDetails))
            {
                return errorDetails;
            }

            throw exception;
        }


    }
}