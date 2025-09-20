using App.Applications.Universities.Apis;
using App.Applications.Universities.Commands.UpdateUniversity;
using App.Applications.Universities.Requests.UpdateCompany;
using App.Common.Utilities.MediaFiles.Requests;

namespace App.Handlers.Universities.Commands.UpdateUniversity;

/// <summary>
///     Main class UpdateUniversityCommand implementing IRequest<UpdateUniversityCommandResponse>.
/// </summary>
public class UpdateUniversityCommandHandler(
    ApiFactory apiFactory ,
    ISnackbarService snackbarService
) : IRequestHandler<UpdateUniversityCommand>
{
    private  IUniversityApi Apis { get; set; } = apiFactory.CreateApi<IUniversityApi>();

    public async Task Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await Apis.UpdateUniversityAsync(request.Adapt<UpdateUniversityRequest>(), cancellationToken);
            snackbarService.ShowSuccess("Update was succsessfully done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            snackbarService.ShowError(e.Message);
        }
    }
}