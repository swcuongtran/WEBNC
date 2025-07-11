using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
namespace BuildingBlocks.Behavior
{
    public class ValidationBehavior<Trequest, TResponse>(IEnumerable<IValidator<Trequest>> validators) : IPipelineBehavior<Trequest, TResponse>
    where Trequest : ICommand<TResponse>
    where TResponse : notnull
    {
        public async Task<TResponse> Handle(Trequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<Trequest>(request);

            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
