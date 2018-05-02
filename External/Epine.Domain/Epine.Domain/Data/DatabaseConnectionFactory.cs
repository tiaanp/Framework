namespace Epine.Domain.Data {
    using Epine.Infrastructure.Extensions;
	using MySql.Data.MySqlClient;
	using System;
	using System.Data.Common;
	using System.Data.SqlClient;

	public static class DatabaseConnectionFactory {

		#region Constants

		private const string NO_DATABASE_CONNECTION_FOUND = "{0} :: [ No DbConnection Found for DatabaseType {1}. ]";

		#endregion

		#region Class Methods

		public static Func<IDatabaseConnectionInfo, DbConnection> GetConnection =
			info => {
				var response = default(DbConnection);


				switch (info.DatabaseType) {
					case DatabaseType.SqlServer:	response = GetSqlConnection(info);		break;
					case DatabaseType.MySql:		response = GetMySqlConnection(info);	break;
					default: {
							throw new InvalidOperationException(
								DatabaseConnectionFactory.NO_DATABASE_CONNECTION_FOUND
									.FormatString(
										typeof(DatabaseConnectionFactory).FullName,
										info.DatabaseType.Name()));
						}
				}
               
                return response;
			};

		private static Func<IDatabaseConnectionInfo, SqlConnection> GetSqlConnection =
			info => {
				var builder							= new SqlConnectionStringBuilder();
				builder.UserID						= info.UserName;
				builder.Password					= info.Password;
				builder.InitialCatalog				= info.Catalog;
				builder.DataSource					= info.Server;
				builder.ConnectTimeout				= 600;
				builder.MultipleActiveResultSets	= true;

				return new SqlConnection(builder.ConnectionString);
			};

		private static Func<IDatabaseConnectionInfo, DbConnection> GetMySqlConnection =
			info => {
				var builder				= new DbConnectionStringBuilder();
				builder["server"]		= info.Server;
				builder["user id"]		= info.UserName;
				builder["password"]		= info.Password;
				builder["database"]		= info.Catalog;

				return new MySqlConnection(builder.ConnectionString);
			};

		#endregion
	}
}
