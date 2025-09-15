﻿using App.Applications.Users.Requests.Registers;
using Microsoft.AspNetCore.Components;

namespace App.Handlers.Users.Requests.Registers;

public record RegisterRequestHandler(
    ApiFactory          apiFactory,
    ClientStateProvider stateProvider,
    NavigationManager   navigationManager,
    ISnackbarService    SnackbarService
) : IRequestHandler<RegisterRequest>
{
    private IUserApis Apis { get; } = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string profile = null!;


            if (request.Profile is not null)
            {
                await using var readStream = request.Profile.OpenReadStream(request.Profile.Size);
                var             ms         = new MemoryStream();
                await readStream.CopyToAsync(ms, cancellationToken);
                ms.Position = 0;

                var imageUrl = await Apis.UploadAvatar(new StreamPart(ms, request.Profile.Name, request.Profile.ContentType));

                if (imageUrl.IsSuccessStatusCode)
                    profile = imageUrl.Content;
                else
                {
                    SnackbarService.ShowError("Profile upload failed");
                    return;
                }
            }

            var apiRequest = new RegisterApiRequest
            {
                Profile     = profile,
                FirstName   = request.FirstName,
                LastName    = request.LastName,
                Address     = request.Address,
                Email       = request.Email,
                PhoneNumber = request.PhoneNumber,
                FullName    = request.FullName,
                Password    = request.Password
            };


            var result = request.Role switch
                         {
                             "Secretary" => await Apis.RegisterSecretary(apiRequest),
                             "Patient"   => stateProvider.User != null ? await Apis.RegisterPatient(apiRequest) : await Apis.Register(apiRequest),
                             _           => throw new ArgumentOutOfRangeException()
                         };

            if (!result.IsSuccessStatusCode)
                throw new Exception("wrong");
        }

        catch (Exception e)

        {
            Console.WriteLine(e);
            throw;
        }
    }
}