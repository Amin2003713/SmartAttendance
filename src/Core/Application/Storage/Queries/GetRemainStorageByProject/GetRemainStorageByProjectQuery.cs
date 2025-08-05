using Shifty.Application.Storage.Request.Queries.GetRemainStorageByProject;

namespace Shifty.Application.Storage.Queries.GetRemainStorageByProject;

public class GetRemainStorageByProjectQuery : IRequest<GetRemainStorageByProjectResponse>
{
    public GetRemainStorageByProjectQuery(Guid? projectId)
    {

    }

    public Guid? UserId { get; set; }
    // public Guid? ProjectId { get; set; }
}