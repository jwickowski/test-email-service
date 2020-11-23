using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EmailService.Api.CustomExceptionMiddleware;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace EmailService.Api.ConventionTests
{
    public class ErrorHandlingTest
    {
        [Fact]
        public void all_custon_exception_should_be_handled_by_CustomExceptionMapper()
        {
            var customExceptionMapper = new CustomExceptionMapper();
            var exceptionTypes = GetAllExceptionTypes();

            foreach (Type exceptionType in exceptionTypes)
            {
                Exception exception = (Exception)Activator.CreateInstance(exceptionType);
                var mappedError = customExceptionMapper.MapErrorToErrorDetails(exception);
            }
        }

        private IEnumerable<Type> GetAllExceptionTypes()
        {
            return GetAllAssemblies()
                .Distinct()
                .Where(x => x.FullName.StartsWith("EmailService"))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.BaseType == typeof(Exception))
                .Distinct();
        }

        private IEnumerable<Assembly> GetAllAssemblies()
        {
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var currentAssembly in currentAssemblies)
            {
                yield return currentAssembly;
                var referencedAssembiles = currentAssemblies
                    .SelectMany(x => x.GetReferencedAssemblies())
                    .Select(x => Assembly.Load(x));
                foreach (var referencedAssembile in referencedAssembiles)
                {
                    yield return referencedAssembile;
                }
            }
        }
    }
}
