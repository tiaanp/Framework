using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Epine.Domain.Data {
	/// <summary>
	///		Provides contract access to common data management members for all 
	///		<see cref="DataEntity"/>-based objects.
	/// </summary>
	/// <typeparam name="TEntity">
	///		A <see cref="DataEntity"/>-based object.
	/// </typeparam>
	public interface IEntityRepository<TEntity>
	where TEntity : DataEntity {

		TResult Max<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> query);
		TResult Max<TResult>( Expression<Func<TEntity, TResult>> query);




		/// <summary>
		///		Retrieves the unique identity for the specified <typeparamref name="TEntity"/>.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> from the data store.
		/// </param>
		/// <param name="skip">
		///		How many entries to ignore before retrieving the unique identifier.
		/// </param>
		/// <returns></returns>
		long GetId(Expression<Func<TEntity, bool>> where, int skip);
		/// <summary>
		///		Retrieves the unique identity for the specified <typeparamref name="TEntity"/>.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> from the data store.
		/// </param>
		/// <param name="skip">
		///		How many entries to ignore before retrieving the unique identifier.
		/// </param>
		/// <returns>
		///		<see cref="Task{long}"/>
		/// </returns>
		Task<long> GetIdAsync(Expression<Func<TEntity, bool>> where, int skip);




		/// <summary>
		///		Adds the specified <typeparamref name="TEntity"/> instance to the
		///		current data repositories instance's collection of
		///		<typeparamref name="TEntity"/>-based instances.
		/// </summary>
		/// <param name="entity">
		///		The <typeparamref name="TEntity"/>-based entity to be added.
		/// </param>
		/// <returns>
		///		The unique identifier for the newly added 
		///		<typeparamref name="TEntity"/>-based entity.
		/// </returns>
		long Add(TEntity entity);
		/// <summary>
		///		Adds the <typeparamref name="TEntity"/> instances specified in 
		///		parameter <paramref name="entities"/> to the
		///		current data repositories instance's collection of
		///		<typeparamref name="TEntity"/>-based instances.
		/// </summary>
		/// <param name="entities">
		///		A <see cref="{IEnumerable<TEntity>}"/>
		/// </param>
		void Add(IEnumerable<TEntity> entities);

		void Add(IEnumerable<TEntity> entities, int batch);
		Task Add(IEnumerable<TEntity> entities, bool autoDetectChangesEnabled);




		/// <summary>
		///		Updates the specified <typeparamref name="TEntity"/> instance.
		/// </summary>
		/// <param name="entity">
		///		The <typeparamref name="TEntity"/>-based entity to be updated.
		/// </param>
		Task EditAsync(TEntity entity);

        /// <summary>
        ///		Updates the specified <typeparamref name="TEntity"/> instance.
        /// </summary>
        /// <param name="entity">
        ///		The <typeparamref name="TEntity"/>-based entity to be updated.
        /// </param>
        /// <returns>
        ///     The Id of the updated entity
        /// </returns>
	    Task<long> EditWithIdResultAsync(TEntity entity);
        /// <summary>
        ///		Updates the specified <typeparamref name="TEntity"/> instance.
        /// </summary>
        /// <param name="entity">
        ///		The <typeparamref name="TEntity"/>-based entity to be updated.
        /// </param>
        void Edit(TEntity entity);
		/// <summary>
		///		Updates the <typeparamref name="TEntity"/>-based entities 
		///		specified  in parameter <paramref name="entities"/>.
		/// </summary>
		/// <param name="entities">
		///		The collection of <typeparamref name="TEntity"/>-based entities to update.
		/// </param>
		void Edit(IEnumerable<TEntity> entities);
		void Edit(TEntity entity, Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		'Deletes' the <typeparamref name="TEntity"/>-based entity with 
		///		the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id">
		///		The value of the unique identifier property for the 
		///		<typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		void Delete(long id);
		/// <summary>
		///		'Deletes' the <typeparamref name="TEntity"/>-based entities 
		///		the specified by <paramref name="ids"/>.
		/// </summary>
		/// <param name="ids">
		///		The collection of unique identifiers for the 
		///		<typeparamref name="TEntity"/>-based entities to find.
		/// </param>
		void Delete(IEnumerable<long> ids);
		/// <summary>
		///		'Deletes' the <typeparamref name="TEntity"/>-based entity with 
		///		the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id">
		///		The value of the unique identifier property for the 
		///		<typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		void HardDelete(long id);




		/// <summary>
		///		Checks for the existence of <typeparamref name="TEntity"/>-based entities.
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> defining the filter 
		///		method to apply when checking for the existence of 
		///		<typeparamref name="TEntity"/>-based entities.
		/// </param>
		/// <returns>
		///		True if the any elements exist, else false.
		/// </returns>
		bool Any(Expression<Func<TEntity, bool>> where);
		/// <summary>
		///		Checks for the existence of <typeparamref name="TEntity"/>-based entities.
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> defining the filter 
		///		method to apply when checking for the existence of 
		///		<typeparamref name="TEntity"/>-based entities.
		/// </param>
		/// <returns>
		///		True if the any elements exist, else false.
		/// </returns>
		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);




		/// <summary>
		///		Template method allowing <see cref="IEntityRepository{TEntity}"/> implementations
		///		the ability to select a specific <typeparamref name="TEntity"/> instance
		///		by it's unique identifier property.
		/// </summary>
		/// <param name="id">
		///		The value of the unique identifier property for the 
		///		<typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		/// <param name="includes">
		///		The navigational properties to be included with 
		///		the <typeparamref name="TEntity"/>-based entity.
		/// </param>
		/// <returns>
		///		The resulting <typeparamref name="TEntity"/>-based entity.
		/// </returns>
		TEntity GetEntityById(
			long id,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///		Template method allowing <see cref="IEntityRepository{TEntity}"/> implementations
		///		the ability to select a specific <typeparamref name="TEntity"/> instance
		///		by it's unique identifier property.
		/// </summary>
		/// <param name="id">
		///		The value of the unique identifier property for the 
		///		<typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		/// <param name="includes">
		///		The navigational properties to be included with 
		///		the <typeparamref name="TEntity"/>-based entity.
		/// </param>
		/// <returns>
		///		The resulting <typeparamref name="TEntity"/>-based entity.
		/// </returns>
		Task<TEntity> GetEntityByIdAsync(
			long id,
			params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		Template method allowing <see cref="IEntityRepository{TEntity}"/> implementations
		///		the ability to select a specific <typeparamref name="TEntity"/> instance
		///		by utilizing the <paramref name="where"/> parameter.
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/>-based entity from the data store.
		/// </param>
		/// <param name="includes">
		///		The <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///		which navigation properties to load for 
		///		the <typeparamref name="TEntity"/>-based entity.
		/// </param>
		/// <returns>
		///		The resulting <typeparamref name="TEntity"/>-based entity.
		/// </returns>
		TEntity GetEntity(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///		Template method allowing <see cref="IEntityRepository{TEntity}"/> implementations
		///		the ability to select a specific <typeparamref name="TEntity"/> instance
		///		by utilizing the <paramref name="where"/> parameter.
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/>-based entity from the data store.
		/// </param>
		/// <param name="includes">
		///		The <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///		which navigation properties to load for 
		///		the <typeparamref name="TEntity"/>-based entity.
		/// </param>
		/// <returns>
		///		The resulting <typeparamref name="TEntity"/>-based entity.
		/// </returns>
		Task<TEntity> GetEntityAsync(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes);




		TEntity GetSingle(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes);

		

		IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
		IEnumerable<TEntity> GetAll(string[] includes);
		Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///		collection filtered by <paramref name="where"/>, with
		///		the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="includes">
		///		The <see cref="{Expression<Func<TEntity, object>>}"/> collection defining 
		///		which navigation properties to load for 
		///		each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///		A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///		Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///		collection filtered by <paramref name="where"/>, with
		///		the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="includes">
		///		The <see cref="{Expression<Func<TEntity, object>>}"/> collection defining 
		///		which navigation properties to load for 
		///		each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///		A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<string> includes);
		/// <summary>
		///		Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///		collection filtered by <paramref name="where"/>, with
		///		the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="includes">
		///		The <see cref="{Expression<Func<TEntity, object>>}"/> collection defining 
		///		which navigation properties to load for 
		///		each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///		A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		Task<IEnumerable<TEntity>> GetEntitiesAsync(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///     The lambda expression defining the filtering 
		///     strategy to apply when retrieving the 
		///     <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			int take,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///     The lambda expression defining the filtering 
		///     strategy to apply when retrieving the 
		///     <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		Task<IEnumerable<TEntity>> GetEntitiesAsync(
			Expression<Func<TEntity, bool>> where,
			int take,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///     The lambda expression defining the filtering 
		///     strategy to apply when retrieving the 
		///     <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="skip">
		///     The amount of <typeparamref name="TEntity"/> to skip before 
		///     including them in the results set.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			int skip,
			int take,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///     The lambda expression defining the filtering 
		///     strategy to apply when retrieving the 
		///     <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="skip">
		///     The amount of <typeparamref name="TEntity"/> to skip before 
		///     including them in the results set.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		Task<IEnumerable<TEntity>> GetEntitiesAsync(
			Expression<Func<TEntity, bool>> where,
			int skip,
			int take,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="skip">
		///     The amount of <typeparamref name="TEntity"/> to skip before 
		///     including them in the results set.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetEntities(
			int skip,
			int take,
			params Expression<Func<TEntity, object>>[] includes);
		/// <summary>
		///     Retrieves an <see cref="IEnumerable{TEntity}"/> 
		///     collection filtered by <paramref name="where"/>, with 
		///     the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="skip">
		///     The amount of <typeparamref name="TEntity"/> to skip before 
		///     including them in the results set.
		/// </param>
		/// <param name="take">
		///     The amount of <typeparamref name="TEntity"/> to return.
		/// </param>
		/// <param name="includes">
		///     A <see cref="Expression{Func{TEntity, object}}"/> collection defining 
		///     which navigation properties to load for 
		///     each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		Task<IEnumerable<TEntity>> GetEntitiesAsync(
			int skip,
			int take,
			params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		Retrieves the collection of all <typeparamref name="TEntity"/>
		///		instances associated with the current <see cref="IEntityRepository{TEntity}"/>
		///		instance, filtered by <paramref name="where"/>, ordered by <paramref name="orderBys"/>,
		///		with the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="orderBys">
		///		The <see cref="Tuple"/> collection holding the 
		///		Expression{Func{TEntity, object}} <see cref="Expression"/> 
		///		defining the order element expression and the <see cref="SortOrder"/> value
		///		defining whether to order by ascending or descending.
		/// </param>
		/// <param name="includes">
		///		The <see cref="{Expression<Func<TEntity, object>>}"/> collection defining 
		///		which navigation properties to load for 
		///		each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///		A filtered and ordered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes);
		IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			int take,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		Retrieves the first <typeparamref name="TEntity"/>-based instance associated with the current 
		///		<see cref="IEntityRepository{TEntity}"/> instance, filtered by <paramref name="where"/>, 
		///		ordered by <paramref name="orderBys"/>, with the navigation properties defined by 
		///		<paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///		The lambda expression defining the filtering 
		///		strategy to apply when retrieving the 
		///		<typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="orderBys">
		///		The <see cref="Tuple"/> collection holding the 
		///		Expression{Func{TEntity, object}} <see cref="Expression"/> 
		///		defining the order element expression and the <see cref="SortOrder"/> value
		///		defining whether to order by ascending or descending.
		/// </param>
		/// <param name="includes">
		///		The Expression{Func{TEntity, object}} collection defining 
		///		which navigation properties to load for 
		///		each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///		A filtered and ordered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		TEntity GetOrderedEntity(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes);




		/// <summary>
		///		Counts the amount of <typeparamref name="TEntity"/>-based entities 
		///		from the data store, filtered by the <paramref name="where"/> parameter,
		/// </summary>
		/// <param name="where">
		///		The Expression{Func{TEntity, bool}} instance containing 
		///		the lambda expression to filter a <typeparamref name="TEntity"/>-based 
		///		collection from the data store.
		/// </param>
		/// <returns>
		///		An <see cref="long"/> value representing the amount 
		///		of <typeparamref name="TEntity"/>-based counted.
		/// </returns>
		long LongCount(Expression<Func<TEntity, bool>> where);
		long LongCount();




		/// <summary>
		///		Counts the amount of <typeparamref name="TEntity"/>-based entities 
		///		from the data store, filtered by the <paramref name="where"/> parameter,
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> instance containing 
		///		the lambda expression to filter a <typeparamref name="TEntity"/>-based 
		///		collection from the data store.
		/// </param>
		/// <returns>
		///		An <see cref="int"/> value representing the amount 
		///		of <typeparamref name="TEntity"/>-based counted.
		/// </returns>
		int Count(Expression<Func<TEntity, bool>> where);
		/// <summary>
		///		Counts the amount of <typeparamref name="TEntity"/>-based entities 
		///		from the data store, filtered by the <paramref name="where"/> parameter,
		/// </summary>
		/// <param name="where">
		///		The <see cref="Expression{Func{TEntity, bool}}"/> instance containing 
		///		the lambda expression to filter a <typeparamref name="TEntity"/>-based 
		///		collection from the data store.
		/// </param>
		/// <returns>
		///		An <see cref="int"/> value representing the amount 
		///		of <typeparamref name="TEntity"/>-based counted.
		/// </returns>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> where);
	}
}
