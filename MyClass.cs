using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OpenRepo
{
    public class MyClass
    {
        public async Task ExecuteNonQueryStoredProcedureAsync(DbContext context, string procedureName, int? commandTimeOut = null, params SqlParameter[] parameters)
        {
            DbConnection con = context.Database.GetDbConnection();

            if (con.State == ConnectionState.Closed)
            {
                string s = "wharever";
                await con.OpenAsync();
            }

            using (SqlCommand command = (SqlCommand)con.CreateCommand())
            {
                var parametersForLog = parameters.Select(x => new { x.ParameterName, x.Value }).ToArray();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.Parameters.AddRange(parameters);
                if (commandTimeOut.HasValue)
                {
                    command.CommandTimeout = commandTimeOut.Value;
                }
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}