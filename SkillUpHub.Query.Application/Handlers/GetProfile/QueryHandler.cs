using System.Data;
using MediatR;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace SkillUpHub.Query.Application.Handlers.GetProfile;

public class QueryHandler(IDbConnection connection) : IRequestHandler<Query, ViewModel>
{
    public async Task<ViewModel> Handle(Query request, CancellationToken cancellationToken)
    {
        const string sql = """
                               SELECT
                                   p."FirstName",
                                   p."LastName",
                                   p."Description"
                               FROM "Profiles" AS p
                               WHERE p."UserId" = @UserId;
                           """;
        
        
        return (await connection.QuerySingleOrDefaultAsync<ViewModel>(sql, new { UserId = request.UserId }))!;
    }
}