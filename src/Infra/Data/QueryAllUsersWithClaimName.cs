using Dapper;
using IWantApp.Endpoint.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantDB"]);
        var query = @"SELECT email, ClaimValue as Name
                    FROM AspNetUsers u INNER JOIN AspNetUserClaims c
                    on u.id = c.UserId AND ClaimType = 'Name'
                    ORDER BY Name
                    OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY;";
        return db.Query<EmployeeResponse>(
            query,
            new { page, rows }
        );
    }
}
