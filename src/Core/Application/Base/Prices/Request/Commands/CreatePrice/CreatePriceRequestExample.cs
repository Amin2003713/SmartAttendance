namespace Shifty.Application.Base.Prices.Request.Commands.CreatePrice;

public class CreatePriceRequestExample : IExamplesProvider<CreatePriceRequest>
{
    public CreatePriceRequest GetExamples()
    {
        return new CreatePriceRequest
        {
            Amount = 270000
        };
    }
}