using App.Applications.Universities.Apis;
using App.Applications.Universities.Queries.CheckDomain;
using Microsoft.AspNetCore.Components;

namespace App.Handlers.Universities.Queries.CheckDomain;

public record CheckDomainQueryHandler(
    ApiFactory ApiFactory ,
    NavigationManager  NavigationManager,
    ISnackbarService SnackbarService
) : IRequestHandler<CheckDomainQuery , CheckDomainResponse>
{
    public IPanelApi Api  = ApiFactory.CreateApi<IPanelApi>();

    public async Task<CheckDomainResponse> Handle(CheckDomainQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Api.CheckDomain(request.Domain , cancellationToken);

            if (result.Content!.Exist)
                return result.Content;


            SnackbarService.ShowError("wrong university or not existed one .");
            NavigationManager.NavigateTo("/not-found" , true , true);
            return result.Content;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            SnackbarService.ShowError(e.Message);

            return null!;
        }
    }
}