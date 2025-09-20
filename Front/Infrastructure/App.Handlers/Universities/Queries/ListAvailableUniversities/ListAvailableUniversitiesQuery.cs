using App.Applications.Universities.Responses.GetCompanyInfo;

namespace App.Handlers.Universities.Queries.ListAvailableUniversities;

public record ListAvailableUniversitiesQuery : IRequest<List<GetUniversityInfoResponse>>;