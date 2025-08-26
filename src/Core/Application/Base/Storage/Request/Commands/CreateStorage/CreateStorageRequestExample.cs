using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.Storage.Request.Commands.CreateStorage;

public class CreateStorageRequestExample : IExamplesProvider<CreateStorageRequest>
{
    public CreateStorageRequest GetExamples()
    {
        return new CreateStorageRequest
        {
            RowId = Guid.Empty,

            FileType = FileType.Pdf,
            StorageUsedByItemMb = 5.25m
        };
    }
}