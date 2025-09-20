using App.Applications.Universities.Apis;
using App.Applications.Universities.Queries.GetUniversityInfo;
using App.Applications.Universities.Responses.GetCompanyInfo;

namespace App.Handlers.Universities.Queries.GetUniversityInfo;

public record GetUniversityInfoQueryHandler(
    ApiFactory ApiFactory ,
    ISnackbarService SnackbarService
) : IRequestHandler< GetUniversityInfoQuery, GetUniversityInfoResponse>
{
    public IUniversityApi Api  = ApiFactory.CreateApi<IUniversityApi>();

    public async Task<GetUniversityInfoResponse> Handle(GetUniversityInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Api.GetUniversityInfoAsync(cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                return result.Content!;
            }

            SnackbarService.ShowApiResult(result.StatusCode);
            return null!;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}