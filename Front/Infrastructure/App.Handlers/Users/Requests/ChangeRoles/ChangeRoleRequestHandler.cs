﻿using App.Applications.Users.Requests.ChangeRoles;

namespace App.Handlers.Users.Requests.ChangeRoles;

public class ChangeRoleRequestHandler(
    ApiFactory apiFactory,
    ISnackbarService snackbarService
) : IRequestHandler<ChangeRoleRequest>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(ChangeRoleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiResponse = await Apis.ChangeRole(request);

            if (apiResponse.IsSuccessStatusCode)
                snackbarService.ShowApiResult(apiResponse.StatusCode);
            else
                snackbarService.ShowError();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            snackbarService.ShowError("Something went wrong");
        }
    }
}