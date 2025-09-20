using App.Applications.Universities.Apis;
using App.Applications.Universities.Queries.ListAvailableUniversities;
using App.Applications.Universities.Responses.GetCompanyInfo;

namespace App.Handlers.Universities.Queries.ListAvailableUniversities;

public record ListAvailableUniversitiesQueryHandler(
    ApiFactory ApiFactory ,
    ILocalStorage LocalStorage,
    ISnackbarService SnackbarService
) :
    IRequestHandler<ListAvailableUniversitiesQuery , List<GetUniversityInfoResponse>>
{
    public IPanelApi Api  = ApiFactory.CreateApi<IPanelApi>();


    public async Task<List<GetUniversityInfoResponse>>
        Handle(ListAvailableUniversitiesQuery request, CancellationToken cancellationToken)
    {
        var result = await Api.ListAvailableUniversities(cancellationToken);
        return result.IsSuccessStatusCode ? result.Content! : [];
    }
}