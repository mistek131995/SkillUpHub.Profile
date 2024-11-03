using System.Data;
using MediatR;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace SkillUpHub.Query.Application.Handlers.GetProfile;

public class QueryHandler(IDbConnection connection, IHttpContextAccessor httpContextAccessor) : IRequestHandler<Query, ViewModel>
{
    public async Task<ViewModel> Handle(Query request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "Id")!.Value;
        
        const string sql = """
                               SELECT
                                   p."FirstName",
                                   p."LastName",
                                   p."Description"
                               FROM "Profiles" AS p
                               WHERE p."UserId" = @UserId;
                           """;
        
        
        return (await connection.QuerySingleOrDefaultAsync<ViewModel>(sql, new { UserId = Guid.Parse(userId) }))!;
    }
}