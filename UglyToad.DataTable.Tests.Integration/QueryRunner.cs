namespace UglyToad.DataTable.Tests.Integration
{
    using System.Data;
    using System.Data.SqlClient;

    public class QueryRunner
    {
        public static DataTable ExecuteStoredProcedure(string procedureName)
        {
            DataTable dataTable = new DataTable("Test");

            using (var conn = new SqlConnection(DatabaseBootstrapper.GetConnectionString()))
            {
                conn.Open();

                var command = new SqlCommand(procedureName, conn);

                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    dataTable.Load(dr);
                }

                command.Dispose();
                conn.Close();
            }

            return dataTable;
        }
    }
}
