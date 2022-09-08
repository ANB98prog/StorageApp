using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Common.Behaviors
{
    /// <summary>
    /// Defines models validation behaviour
    /// </summary>
    /// <typeparam name="TRequest">Currnet validation model type</typeparam>
    /// <typeparam name="TResponse">Next validation model type</typeparam>
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// List of validators
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Creates instance of <see cref="ValidationBehavior"/>
        /// </summary>
        /// <param name="validators">List of model validators</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;

        /// <summary>
        /// Handles model validation
        /// </summary>
        /// <param name="request">Validation request model</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="next">Next validation model request</param>
        /// <returns>Next validation request</returns>
        /// <exception cref="ValidationException">If validation has errors</exception>
        public Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if(failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
