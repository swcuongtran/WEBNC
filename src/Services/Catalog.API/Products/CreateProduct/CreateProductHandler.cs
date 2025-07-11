namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price):
       ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
    public class CreateProductHandler(IDocumentSession session,IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(command, cancellationToken);
            var errors = result.Errors;
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}
