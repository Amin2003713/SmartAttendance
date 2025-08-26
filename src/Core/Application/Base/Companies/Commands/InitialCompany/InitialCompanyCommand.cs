using Shifty.Application.Base.Companies.Requests.InitialCompany;

namespace Shifty.Application.Base.Companies.Commands.InitialCompany;

public class InitialCompanyCommand : InitialCompanyRequest, IRequest<string>;