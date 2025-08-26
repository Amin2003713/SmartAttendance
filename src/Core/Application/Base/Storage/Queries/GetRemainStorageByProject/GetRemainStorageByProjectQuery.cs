using Shifty.Application.Base.Storage.Request.Queries.GetRemainStorageByProject;

namespace Shifty.Application.Base.Storage.Queries.GetRemainStorageByProject;

public class GetRemainStorageByProjectQuery : IRequest<GetRemainStorageByProjectResponse>
{
    public GetRemainStorageByProjectQuery(Guid? projectId)
    {

    }

    public Guid? UserId { get; set; }
    // public Guid? ProjectId { get; set; }
}