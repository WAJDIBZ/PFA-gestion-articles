using StockManagement.Domain.Entities;

namespace StockManagement.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenererToken(ApplicationUser utilisateur, IList<string> roles);
}
