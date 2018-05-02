using System.Data.Entity;
using System.Threading.Tasks;

namespace Epine.Domain.Data
{
	//!!!!!!!!!!!!!!!!!NBN My Sql Add-Migration InitialMigrations -IgnoreChanges
	/// <summary>
	///		Defines a contract for <c>IContextFactory</c>, providing
	///		connectivity to a <see cref="DbContext"/>.
	/// </summary>
	public interface IContextFactory {

		/// <summary>
		///		Creates a new <see cref="DbContext"/> instance.
		/// </summary>
		/// <returns>
		///		A new <see cref="DbContext"/> instance.
		/// </returns>
		DbContext Connect();

        /// <summary>
		///		Creates a new <see cref="DbContext"/> instance.
		/// </summary>
		/// <returns>
		///		A new <see cref="Task{DbContext}"/> instance.
		/// </returns>
        Task<DbContext> ConnectAsync();
    }
}
