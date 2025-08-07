using Shifty.Application.Companies.Requests.InitialCompany;

namespace Shifty.Application.Companies.Commands.InitialCompany;

public class InitialCompanyCommand : InitialCompanyRequest, IRequest<string>;