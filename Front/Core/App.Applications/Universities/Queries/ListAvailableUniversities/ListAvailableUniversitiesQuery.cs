using App.Applications.Universities.Responses.GetCompanyInfo;
using MediatR;

namespace App.Applications.Universities.Queries.ListAvailableUniversities;

public record ListAvailableUniversitiesQuery : IRequest<List<GetUniversityInfoResponse>>;