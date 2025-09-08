using SmartAttendance.Application.Base.Companies.Requests.InitialCompany;

namespace SmartAttendance.Application.Base.Companies.Commands.InitialCompany;

public class InitialCompanyCommand : InitialCompanyRequest,
    IRequest<string>;