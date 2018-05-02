using Epine.Infrastructure.Extensions;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Epine.Domain.Data {
	/// <summary>
	///              <c>Repository&lt;TEntity&gt;</c> implements a base repository for 
	///              <typeparamref name="TEntity"/>-based entities.
	/// </summary>
	/// <typeparam name="TEntity">
	///              The <see cref="DataEntity"/>-based object associated
	///              with <see cref="EntityRepository&lt;TEntity&gt;"/>.
	/// </typeparam>
	public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity>
		where TEntity : DataEntity {

		#region Constructors

		/// <summary>
		///             Creates a new <see cref="EntityRepository&lt;TEntity&gt;"/> instance based on the
		///              specified values.
		/// </summary>
		/// <param name="contextFactory">
		///             The <see cref="IContextFactory"/> instance associated
		///             with the created <see cref="EntityRepository&lt;TEntity&gt;"/> instance.
		/// </param>
		protected EntityRepository(
			IContextFactory contextFactory) {
			// Verify parameters.
			"contextFactory".IsNotNullArgument(contextFactory);

			this.contextFactory = contextFactory;

		}

		#endregion

		#region IEntityRepository<TEntity> Implementation

		/// <summary>
		///             Adds the specified <typeparamref name="TEntity"/> instance to the
		///             current data repositories instance's collection of
		///              <typeparamref name="TEntity"/>-based instances.
		/// </summary>
		/// <param name="entity">
		///              The <typeparamref name="TEntity"/>-based entity to be added.
		/// </param>
		/// <returns>
		///              The unique identifier for the newly added 
		///              <typeparamref name="TEntity"/>-based entity.
		/// </returns>
		public virtual long Add(TEntity entity) {
			// Verify parameters.
			"entity".IsNotNullArgument(entity);

			// Add accordingly.
			using (DbContext context = this.Connect()) {

				if (this.IsValid(entity, context)) {

					// Add accordingly.                                
					context.Set<TEntity>().Add(entity);
				}

				// Save.
				try {
				context.SaveChanges();

				}
				catch (Exception e) {
					
					throw e;
				}
			}
			// Return resulting entity id.
			return entity.Id;
		}
		/// <summary>
		///             Adds the <typeparamref name="TEntity"/> instances specified in 
		///              parameter <paramref name="entities"/> to the
		///             current data repositories instance's collection of
		///              <typeparamref name="TEntity"/>-based instances.
		/// </summary>
		/// <param name="entities">
		///              A <see cref="IEnumerable{TEntity}"/>
		/// </param>
		public void Add(IEnumerable<TEntity> entities) {
			// Verify parameters.
			"entity".IsNotNullArgument(entities);


			using (DbContext context = this.Connect()) {
				entities.ForEachElement(
					entity => {
						if (this.IsValid(entity, context)) {
							// Add accordingly.
							context.Set<TEntity>().Add(entity);
						}
					});

				// Save.
				context.SaveChanges();
			}
		}




		/// <summary>
		///              Updates the <typeparamref name="TEntity"/>-based entities 
		///             specified  in parameter <paramref name="entities"/>.
		/// </summary>
		/// <param name="entities">
		///             The collection of <typeparamref name="TEntity"/>-based entities to update.
		/// </param>
		public void Edit(IEnumerable<TEntity> entities) {
			// Connect.
			using (DbContext context = this.Connect()) {

				entities.ForEachElement(
					entity => {
						// Retrieve the existing one.
						TEntity original = this.GetEntity(context, entity.Id);
						// Forward accordingly.
						this.DataTransfer(original, entity);
					});

				// Save changes.
				context.SaveChanges();
			}
		}
		/// <summary>
		///             Updates the specified <typeparamref name="TEntity"/> instance.
		/// </summary>
		/// <param name="entity">
		///              The <typeparamref name="TEntity"/>-based entity to be updated.
		/// </param>
		public void Edit(TEntity entity) {
			"entity".IsNotNullArgument(entity);

			// Connect.
			using (DbContext context = this.Connect()) {
				// Retrieve the existing one.
				TEntity original = this.GetEntity(context, entity.Id);

				// Forward accordingly.
				this.DataTransfer(original, entity);

				// Save changes.
				context.SaveChanges();
			}
		}
        /// <summary>
        ///     Updates the specified <typeparamref name="TEntity"/> instance.
		/// </summary>
		/// <param name="entity">
        ///     The <typeparamref name="TEntity"/>-based entity to be updated.
		/// </param>
		public async Task EditAsync(TEntity entity) {
            "entity".IsNotNullArgument(entity);

            // Connect.
            using (var context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Retrieve the existing one.
                TEntity original = await this.GetEntityAsync(context, entity.Id).ConfigureAwait(false);

                // Forward accordingly.
                this.DataTransfer(original, entity);

                // Save changes.
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

	    public async Task<long> EditWithIdResultAsync(TEntity entity)
	    {
	        "entity".IsNotNullArgument(entity);

	        // Connect.
	        using (var context = await this.ConnectAsync().ConfigureAwait(false))
	        {
	            // Retrieve the existing one.
	            TEntity original = await this.GetEntityAsync(context, entity.Id).ConfigureAwait(false);

	            // Forward accordingly.
	            this.DataTransfer(original, entity);

	            // Save changes.
	            return await context.SaveChangesAsync().ConfigureAwait(false);
	        }
	    }
        public void Edit(TEntity entity, Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes) {
			"entity".IsNotNullArgument(entity);

			// Connect.
			using (DbContext context = this.Connect()) {
				// Retrieve the existing one.
				TEntity original = this.GetEntity(context, where, includes);

				// Forward accordingly.
				this.DataTransfer(original, entity);

				// Save changes.
				context.SaveChanges();
			}
		}




		/// <summary>
		///             'Deletes' the <typeparamref name="TEntity"/>-based entity with 
		///              the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id">
		///             The value of the unique identifier property for the 
		///              <typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		public void Delete(long id) {
			// Connect
			using (DbContext context = this.Connect()) {
				// Retrieve entity.
				TEntity entity = this.GetEntity(context, id);

				if (entity != null) {
					// Delete accordingly.
					entity.IsDeleted = true;
					//context.Set<TEntity>().Remove(entity);
					context.SaveChanges();
				}
			}
		}
		/// <summary>
		///             'Deletes' the <typeparamref name="TEntity"/>-based entities 
		///              the specified by <paramref name="ids"/>.
		/// </summary>
		/// <param name="ids">
		///             The collection of unique identifiers for the 
		///              <typeparamref name="TEntity"/>-based entities to find.
		/// </param>
		public void Delete(IEnumerable<long> ids) {
			// Connect
			using (DbContext context = this.Connect()) {
				ids.ForEachElement(
					id => {
						// Retrieve entity.
						TEntity entity = this.GetEntity(context, id);

						if (entity != null) {
							// Delete accordingly.
							entity.IsDeleted = true;
							//context.Set<TEntity>().Remove(entity);
						}
					});

				context.SaveChanges();
			}
		}
		/// <summary>
		///             'Deletes' the <typeparamref name="TEntity"/>-based entity with 
		///              the specified <paramref name="id"/>.
		/// </summary>
		/// <param name="id">
		///             The value of the unique identifier property for the 
		///              <typeparamref name="TEntity"/>-based entity to find.
		/// </param>
		public void HardDelete(long id) {
			// Connect
			using (DbContext context = this.Connect()) {
				// Retrieve entity.
				TEntity entity = this.GetEntity(context, id);

				if (entity != null) {
					// Delete accordingly.
					context.Set<TEntity>().Remove(entity);
					//context.Set<TEntity>().Remove(entity);
					context.SaveChanges();
				}
			}
		}



		/// <summary>
		///		Checks for the existence of  <typeparamref name="TEntity"/>-based entities.
		/// </summary>
		/// <param name="where">
		///		The Expression{Func{DataEntity, bool}} defining the filter 
		///     method to apply when checking for the existence of 
		///     <typeparamref name="TEntity"/>-based entities.
		/// </param>
		/// <returns>
		///		True if the any elements exist, else false.
		/// </returns>
		public bool Any(Expression<Func<TEntity, bool>> where) {
			var response = false;

			// Connect
			using (DbContext context = this.Connect()) {
				// Retrieve entity.
				response =
					context.Set<TEntity>()
						.Where(
							entity =>
								!entity.IsDeleted)
						.Any(where);
			}

			return response;
		}
		/// <summary>
		///		Checks for the existence of  <typeparamref name="TEntity"/>-based entities.
		/// </summary>
		/// <param name="where">
		///		The Expression{Func{DataEntity, bool}} defining the filter 
		///     method to apply when checking for the existence of 
		///     <typeparamref name="TEntity"/>-based entities.
		/// </param>
		/// <returns>
		///		True if the any elements exist, else false.
		/// </returns>
		public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where) {
			var response = false;

			// Connect
			using (DbContext context = this.Connect()) {
				// Retrieve entity.
				response =
					await context.Set<TEntity>()
						.Where(
							entity =>
								!entity.IsDeleted)
						.AnyAsync(where);
			}

			return response;
		}




        /// <summary>
        ///     Template method allowing <see cref="EntityRepository{TEntity}"/> implementations 
        ///     the ability to select a specific <typeparamref name="TEntity"/> instance
        ///     by it's unique identifier property.
        /// </summary>
        /// <param name="id">
        ///     The value of the unique identifier property for the 
        ///     <typeparamref name="TEntity"/>-based entity to find.
        /// </param>
        /// <param name="includes">
        ///     The navigational properties to be included with 
        ///     the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///     The resulting <typeparamref name="TEntity"/>-based entity.
        /// </returns>
        public TEntity GetEntityById(
			long id,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = default(TEntity);

			using (DbContext context = this.Connect()) {
				// Create relevant object set.

				var query =
					context.Set<TEntity>()
						.Where(
							obj =>
								obj.Id == id &&
								!obj.IsDeleted);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				entity =
					query
						.FirstOrDefault<TEntity>();
			}

			return entity;
		}
        /// <summary>
        ///     Template method allowing <see cref="EntityRepository{TEntity}"/> implementations 
        ///     the ability to select a specific <typeparamref name="TEntity"/> instance
        ///     by it's unique identifier property.
        /// </summary>
        /// <param name="id">
        ///     The value of the unique identifier property for the 
        ///     <typeparamref name="TEntity"/>-based entity to find.
        /// </param>
        /// <param name="includes">
        ///     The navigational properties to be included with 
        ///     the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///     The resulting <typeparamref name="TEntity"/>-based entity.
        /// </returns>
        public async Task<TEntity> GetEntityByIdAsync(
			long id,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = default(TEntity);

			using (DbContext context = this.Connect()) {
				// Create relevant object set.

				var query =
					context.Set<TEntity>()
						.Where(
							obj =>
								obj.Id == id &&
								!obj.IsDeleted);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				entity =
					await query.FirstOrDefaultAsync<TEntity>();
			}

			return entity;
		}
        /// <summary>
        ///     Template method allowing <see cref="EntityRepository{TEntity}"/> implementations 
        ///     the ability to select a specific <typeparamref name="TEntity"/> instance
        ///     by it's unique identifier property.
        /// </summary>
        /// <param name="id">
        ///     The value of the unique identifier property for the 
        ///     <typeparamref name="TEntity"/>-based entity to find.
        /// </param>
        /// <param name="includes">
        ///     The navigational properties to be included with 
        ///     the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///     The resulting <typeparamref name="TEntity"/>-based entity.
        /// </returns>
        public TEntity GetEntityById(
            long id,
            IEnumerable<string> includes) {
            // Resulting instance.
            TEntity entity = default(TEntity);

            using (DbContext context = this.Connect()) {
                // Create relevant object set.

                var query =
                    context.Set<TEntity>()
                        .Where(
                            obj =>
                                obj.Id == id &&
                                !obj.IsDeleted);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                entity =
                    query
                        .FirstOrDefault<TEntity>();
            }

            return entity;
        }




        /// <summary>
        ///     Template method allowing <see cref="EntityRepository{TEntity}"/> implementations 
        ///     the ability to select a specific <typeparamref name="TEntity"/> instance 
        ///     by utilizing the <paramref name="where"/> parameter.
        /// </summary>
        /// <param name="where">
        ///     The Expression{Func{TEntity, bool}} lambda expression defining the filtering 
        ///     strategy to apply when retrieving the 
        ///     <typeparamref name="TEntity"/>-based entity from the data store.
        /// </param>
        /// <param name="includes">
        ///		The <see cref="Expression{Func{TEntity, object}}"/> collection defining 
        ///		which navigation properties to load for the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///     The resulting <typeparamref name="TEntity"/>-based entity.
        /// </returns>
        public TEntity GetEntity(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = null;

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj =>
								!obj.IsDeleted)
						.Where(where.Expand());

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				entity =
					query
						.FirstOrDefault<TEntity>();
			}

			return entity;
		}
        /// <summary>
        ///     Template method allowing <see cref="EntityRepository{TEntity}"/> implementations 
        ///     the ability to select a specific <typeparamref name="TEntity"/> instance 
        ///     by utilizing the <paramref name="where"/> parameter.
        /// </summary>
        /// <param name="where">
        ///     The Expression{Func{TEntity, bool}} lambda expression defining the filtering 
        ///     strategy to apply when retrieving the 
        ///     <typeparamref name="TEntity"/>-based entity from the data store.
        /// </param>
        /// <param name="includes">
        ///		The <see cref="Expression{Func{TEntity, object}}"/> collection defining 
        ///		which navigation properties to load for the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///     The resulting <typeparamref name="TEntity"/>-based entity.
        /// </returns>
        public async Task<TEntity> GetEntityAsync(
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting instance.
            TEntity entity = null;

            using (DbContext context = this.Connect()) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj =>
                                !obj.IsDeleted)
                        .Where(where);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                entity =
                    await query.FirstOrDefaultAsync<TEntity>();
            }

            return entity;
        }




		public TEntity GetSingle(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = null;

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context.Set<TEntity>()
						.Where(obj => !obj.IsDeleted)
						.Where(where);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				entity =
					query
						.Single<TEntity>();
			}

			return entity;
		}




		public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes) {
			var response = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.AsQueryable();


				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Get results.
				response = query.ToList();
			}

			return response;
		}
		public IEnumerable<TEntity> GetAll(string[] includes) {
			var response = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.AsQueryable();


				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Get results.
				response = query.ToList();
			}

			return response;
		}
        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes) {
            var response = Enumerable.Empty<TEntity>();

            using (DbContext context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .AsQueryable();


                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                response = await query.ToListAsync();
            }

            return response;
        }



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
        /// <param name="includes">
        ///     The Expression{Func{TEntity, object}} collection defining 
        ///     which navigation properties to load for 
        ///     each <typeparamref name="TEntity"/> returned.
        /// </param>
        /// <returns>
        ///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
        /// </returns>
        public IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting collection.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Get results.
				entities =
					query.ToList();
			}

			// Return resulting collection.
			return entities;
		}
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
        /// <param name="includes">
        ///     The <see cref="IEnumerable{string}"/> collection defining 
        ///     which navigation properties to load for 
        ///     each <typeparamref name="TEntity"/> returned.
        /// </param>
        /// <returns>
        ///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
        /// </returns>
        public IEnumerable<TEntity> GetEntities(
            Expression<Func<TEntity, bool>> where,
            IEnumerable<string> includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (DbContext context = this.Connect()) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .Where(where);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                    query.ToList();
            }

            // Return resulting collection.
            return entities;
        }
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
        /// <param name="includes">
        ///     The Expression{Func{TEntity, object}} collection defining 
        ///     which navigation properties to load for 
        ///     each <typeparamref name="TEntity"/> returned.
        /// </param>
        /// <returns>
        ///     A filtered <see cref="IEnumerable{TEntity}"/>-based entity collection.
        /// </returns>
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync(
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (DbContext context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .Where(where.Expand());

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                    await query.ToListAsync().ConfigureAwait(false);
            }

            // Return resulting collection.
            return entities;
        }
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
        public IEnumerable<TEntity> GetEntities(
			Expression<Func<TEntity, bool>> where,
			int take,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting collection.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where)
                        .OrderBy(
                            e => e.Id)
                        .Take(take);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Get results.
				entities =
					query.ToList();
			}

			// Return resulting collection.
			return entities;
		}
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
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync(
            Expression<Func<TEntity, bool>> where,
            int take,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (DbContext context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .Where(where)
                        .Take(take);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                   await  query.ToListAsync().ConfigureAwait(false);
            }

            // Return resulting collection.
            return entities;
        }
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
        public IEnumerable<TEntity> GetEntities(
            Expression<Func<TEntity, bool>> where,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes) {
			// Resulting collection.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect())
			{
				// Create relevant object set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where)
						.Skip(skip)
						.Take(take);

				// Add includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Get results.
				entities =
					query.ToList();
			}

			// Return resulting collection.
			return entities;
		}
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
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync(
            Expression<Func<TEntity, bool>> where,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (var context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .Where(where)
                        .Skip(skip)
                        .Take(take);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                    await query.ToListAsync().ConfigureAwait(false);
            }

            // Return resulting collection.
            return entities;
        }
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
        public IEnumerable<TEntity> GetEntities(
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (DbContext context = this.Connect()) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .OrderBy(
                            e => e.Id)
                        .Skip(skip)
                        .Take(take);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                    query.ToList();
            }

            // Return resulting collection.
            return entities;
        }
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
        public async Task<IEnumerable<TEntity>> GetEntitiesAsync(
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes) {
            // Resulting collection.
            IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

            using (DbContext context = await this.ConnectAsync().ConfigureAwait(false)) {
                // Create relevant object set.
                var query =
                    context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .OrderBy(
                            e => e.Id)
                        .Skip(skip)
                        .Take(take);

                // Add includes.
                includes
                    .ForEachElement(
                        include =>
                            query = query.Include(include));

                // Get results.
                entities =
                    await query.ToListAsync().ConfigureAwait(false);
            }

            // Return resulting collection.
            return entities;
        }




        public TResult Max<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> query) {
			var response = default(TResult);
			using (var context = this.Connect()) {
				response = context.Set<TEntity>().Where(where).Max(query);
			}
			return response;
		}

		public TResult Max<TResult>( Expression<Func<TEntity, TResult>> query)
		{
			var response = default(TResult);
			using (var context = this.Connect())
			{
				response = context.Set<TEntity>().Max(query);
			}
			return response;
		}


		public long GetId( Expression<Func<TEntity, bool>> where, int skip)
		{
			var response = default(long);
			using (DbContext context = this.Connect()) {
				 response = context.Set<TEntity>()
					 .Where(where)
					.OrderBy(
							e => e.Id)
					.Skip(skip).Take(1).SingleOrDefault(where).Id;
			}
			return response;
		}
		public async Task<long> GetIdAsync(Expression<Func<TEntity, bool>> where, int skip) {
			var response = default(long);

			using (DbContext context = this.Connect()) {
				var entity =
					await
					context
						.Set<TEntity>()
						.Where(where)
						.OrderBy(
							e => e.Id)
						.Skip(skip)
						.FirstOrDefaultAsync();

				if(entity != null) {
					response = entity.Id;
				}
			}
			return response;
		}




		/// <summary>
		///             Retrieves the collection of all <typeparamref name="TEntity"/>
		///             instances associated with the current <see cref="EntityRepository&lt;TEntity&gt;"/>
		///             instance, filtered by <paramref name="where"/>, ordered by <paramref name="orderBys"/>,
		///             with the navigation properties defined by <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///             The lambda expression defining the filtering 
		///              strategy to apply when retrieving the 
		///             <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="orderBys">
		///             The <see cref="Tuple"/> collection holding the 
		///             Func{TEntity, object} <see cref="Expression"/> 
		///             defining the order element expression and the <see cref="SortOrder"/> value
		///             defining whether to order by ascending or descending.
		/// </param>
		/// <param name="includes">
		///             The <see cref="string"/> collection defining 
		///              which navigation properties to load for 
		///             each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///             A filtered and ordered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		public IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting collection of entities.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where);

				// Includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Order by.
				var count = 0;
				orderBys
					.ForEachElement(
						orderBy => {
							query =
								(orderBy.Item2 == SortOrder.Descending)
									? count == 0
										? query.OrderByDescending(orderBy.Item1)
										: query.As<IOrderedQueryable<TEntity>>().ThenByDescending(orderBy.Item1)
									: count == 0 ? query.OrderBy(orderBy.Item1) : query.As<IOrderedQueryable<TEntity>>().ThenBy(orderBy.Item1);
							count++;
						});


				entities = query.ToList();
			}

			// Return resulting set.
			return entities;
		}

		public IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			string[] includes) {
			// Resulting collection of entities.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where);

				// Includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Order by.
				var count = 0;
				orderBys
					.ForEachElement(
						orderBy => {
							query =
								(orderBy.Item2 == SortOrder.Descending)
									? count == 0
										? query.OrderByDescending(orderBy.Item1)
										: query.As<IOrderedQueryable<TEntity>>().ThenByDescending(orderBy.Item1)
									: count == 0 ? query.OrderBy(orderBy.Item1) : query.As<IOrderedQueryable<TEntity>>().ThenBy(orderBy.Item1);
							count++;
						});


				entities = query.ToList();
			}

			// Return resulting set.
			return entities;
		}

		public IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			int take,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting collection of entities.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where)
						.Take(take);

				// Includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Order by.
				var count = 0;
				orderBys
					.ForEachElement(
						orderBy => {
							query =
								(orderBy.Item2 == SortOrder.Descending)
									? count == 0
										? query.OrderByDescending(orderBy.Item1)
										: query.As<IOrderedQueryable<TEntity>>().ThenByDescending(orderBy.Item1)
									: count == 0 ? query.OrderBy(orderBy.Item1) : query.As<IOrderedQueryable<TEntity>>().ThenBy(orderBy.Item1);
							count++;
						});


				entities = query.ToList();
			}

			// Return resulting set.
			return entities;
		}

		public IEnumerable<TEntity> GetOrderedEntities(
			Expression<Func<TEntity, bool>> where,
			int take,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			string[] includes) {
			// Resulting collection of entities.
			IEnumerable<TEntity> entities = Enumerable.Empty<TEntity>();

			using (DbContext context = this.Connect()) {
				// Create relevant set.
				var query =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Where(where)
						.Take(take);

				// Includes.
				includes
					.ForEachElement(
						include =>
							query = query.Include(include));

				// Order by.
				var count = 0;
				orderBys
					.ForEachElement(
						orderBy => {
							query =
								(orderBy.Item2 == SortOrder.Descending)
									? count == 0
										? query.OrderByDescending(orderBy.Item1)
										: query.As<IOrderedQueryable<TEntity>>().ThenByDescending(orderBy.Item1)
									: count == 0 ? query.OrderBy(orderBy.Item1) : query.As<IOrderedQueryable<TEntity>>().ThenBy(orderBy.Item1);
							count++;
						});


				entities = query.ToList();
			}

			// Return resulting set.
			return entities;
		}

		/// <summary>
		///             Retrieves the first <typeparamref name="TEntity"/>-based instance associated with the current 
		///             <see cref="EntityRepository{TEntity}"/> instance, filtered by <paramref name="where"/>, 
		///             ordered by <paramref name="orderBys"/>, with the navigation properties defined by 
		///              <paramref name="includes"/> loaded.
		/// </summary>
		/// <param name="where">
		///             The lambda expression defining the filtering 
		///              strategy to apply when retrieving the 
		///             <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <param name="orderBys">
		///             The <see cref="Tuple"/> collection holding the 
		///             Expression&lt;Func&lt;TEntity, object&gt;&gt; <see cref="Expression"/> 
		///             defining the order element expression and the <see cref="SortOrder"/> value
		///             defining whether to order by ascending or descending.
		/// </param>
		/// <param name="includes">
		///             The Expression{Func{TEntity, object}} collection defining 
		///              which navigation properties to load for 
		///             each <typeparamref name="TEntity"/> returned.
		/// </param>
		/// <returns>
		///             A filtered and ordered <see cref="IEnumerable{TEntity}"/>-based entity collection.
		/// </returns>
		public TEntity GetOrderedEntity(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			params Expression<Func<TEntity, object>>[] includes) {
			return
				this.GetOrderedEntities(where, orderBys, includes).FirstOrDefault();
		}

		public TEntity GetOrderedEntity(
			Expression<Func<TEntity, bool>> where,
			IEnumerable<Tuple<Expression<Func<TEntity, object>>, SortOrder>> orderBys,
			string[] includes) {
			return
				this.GetOrderedEntities(where, orderBys, includes).FirstOrDefault();
		}

		/// <summary>
		///             Calculates the number of entities associated with 
		///             <see cref="EntityRepository&lt;TEntity&gt;"/>.
		/// </summary>
		/// <param name="where">
		///             The lambda expression defining the filtering 
		///              strategy to apply when counting the 
		///             <typeparamref name="TEntity"/> collection from the data store.
		/// </param>
		/// <returns>
		///             An <see cref="int"/> value representing the amount 
		///              of <typeparamref name="TEntity"/>-based entities counted.
		/// </returns>
		public long LongCount(Expression<Func<TEntity, bool>> where) {
			// Resulting count.
			long numberOfEntities = 0;

			using (DbContext context = this.Connect()) {
				// Execute query.
				numberOfEntities =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.LongCount(where);
			}

			return numberOfEntities;
		}
		public long LongCount() {
			// Resulting count.
			long numberOfEntities = 0;

			using (DbContext context = this.Connect()) {
				// Execute query.
				numberOfEntities =
					context
						.Set<TEntity>()
						.LongCount(obj => !obj.IsDeleted);
			}

			return numberOfEntities;
		}




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
		public int Count(Expression<Func<TEntity, bool>> where) {
			// Resulting count.
			int numberOfEntities = 0;

			using (DbContext context = this.Connect()) {

				// Execute query.
				numberOfEntities =
					context
						.Set<TEntity>()
						.Where(
							obj => !obj.IsDeleted)
						.Count(where);
			}

			return numberOfEntities;
		}
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
		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where) {
            // Resulting count.
            int numberOfEntities = 0;

            using (DbContext context = await this.ConnectAsync().ConfigureAwait(false)) {

                // Execute query.
                numberOfEntities =
                    await context
                        .Set<TEntity>()
                        .Where(
                            obj => !obj.IsDeleted)
                        .CountAsync(where);
            }

            return numberOfEntities;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///             Connects to the <see cref="DbContext"/> instance associated 
        ///             with the current <see cref="EntityRepository&lt;TEntity&gt;"/> instance.
        /// </summary>
        /// <returns>
        ///              The <see cref="DbContext"/> instance to use.
        /// </returns>
        protected DbContext Connect() {
			return this.contextFactory.Connect();
        }
        /// <summary>
		///             Connects to the <see cref="DbContext"/> instance associated 
		///             with the current <see cref="EntityRepository&lt;TEntity&gt;"/> instance.
		/// </summary>
		/// <returns>
		///              The <see cref="DbContext"/> instance to use.
		/// </returns>
		protected async Task<DbContext> ConnectAsync() {
            return await this.contextFactory.ConnectAsync();
        }

        /// <summary>
        ///             Template method allowing <see cref="EntityRepository{TEntity}"/> implementations
        ///             the ability to update a specific <typeparamref name="TEntity"/> instance
        ///             based on another <typeparamref name="TEntity"/>-based instance.
        /// </summary>
        /// <param name="original">
        ///              The <typeparamref name="TEntity"/>-based original entity to be updated
        ///              with the values of the modified instance.
        /// </param>
        /// <param name="modified">
        ///              The <typeparamref name="TEntity"/>-based entity from which the original
        ///              is to be updated.
        /// </param>
        /// <remarks>
        ///             <b>Note to inheritors: </b> update <paramref name="original"/>
        ///             with the values from <paramref name="modified"/>.
        /// </remarks>
        protected virtual void DataTransfer(TEntity original, TEntity modified) {
			// Verify parameters.
			"original".IsNotNullArgument(original);
			"modified".IsNotNullArgument(modified);
		}

        /// <summary>
        ///             Template method allowing <see cref="EntityRepository{TEntity}"/> implementations
        ///             the ability to select a specific <typeparamref name="TEntity"/> instance
        ///              by it's unique identifier property.
        /// </summary>
        /// <param name="context">
        ///             The <see cref="DbContext"/> from which to locate an 
        ///              <typeparamref name="TEntity"/>-based entity with 
        ///              the specified <paramref name="id"/>.
        /// </param>
        /// <param name="id">
        ///             The value of the unique identifier property for the 
        ///              <typeparamref name="TEntity"/>-based entity to find.
        /// </param>
        /// <param name="includes">
        ///             The navigational properties to be included with 
        ///              the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///             The resulting <typeparamref name="TEntity"/> based entity.
        /// </returns>
        /// <remarks>
        ///             <b>Note to inheritors: </b> do not dispose the specified 
        ///              <see cref="DbContext"/> instance.
        /// </remarks>
        protected TEntity GetEntity(
			DbContext context,
			long id,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = default(TEntity);

			// Create relevant object set.
			var query =
				context
					.Set<TEntity>()
					.Where(
						obj =>
							obj.Id == id &&
							!obj.IsDeleted);

			// Add includes.
			includes
				.ForEachElement(
					include =>
						query = query.Include(include));

			entity =
				query
					.FirstOrDefault<TEntity>();

			return entity;
		}
        /// <summary>
        ///             Template method allowing <see cref="EntityRepository{TEntity}"/> implementations
        ///             the ability to select a specific <typeparamref name="TEntity"/> instance
        ///              by it's unique identifier property.
        /// </summary>
        /// <param name="context">
        ///             The <see cref="DbContext"/> from which to locate an 
        ///              <typeparamref name="TEntity"/>-based entity with 
        ///              the specified <paramref name="id"/>.
        /// </param>
        /// <param name="id">
        ///             The value of the unique identifier property for the 
        ///              <typeparamref name="TEntity"/>-based entity to find.
        /// </param>
        /// <param name="includes">
        ///             The navigational properties to be included with 
        ///              the <typeparamref name="TEntity"/>-based entity.
        /// </param>
        /// <returns>
        ///             The resulting <typeparamref name="TEntity"/> based entity.
        /// </returns>
        /// <remarks>
        ///             <b>Note to inheritors: </b> do not dispose the specified 
        ///              <see cref="DbContext"/> instance.
        /// </remarks>
        protected async Task<TEntity> GetEntityAsync(
            DbContext context,
            long id,
            params Expression<Func<TEntity, object>>[] includes) {

            // Create relevant object set.
            var query =
                context
                    .Set<TEntity>()
                    .Where(
                        obj =>
                            obj.Id == id &&
                            !obj.IsDeleted);

            // Add includes.
            includes
                .ForEachElement(
                    include =>
                        query = query.Include(include));

            return await query.FirstOrDefaultAsync<TEntity>().ConfigureAwait(false);
        }

        protected TEntity GetEntityById(
			DbContext context,
			long id,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = default(TEntity);

			// Create relevant object set.

			var query =
				context.Set<TEntity>()
					.Where(
						obj =>
							obj.Id == id &&
							!obj.IsDeleted);

			// Add includes.
			includes
				.ForEachElement(
					include =>
						query = query.Include(include));

			entity =
				query
					.FirstOrDefault<TEntity>();


			return entity;
		}

		/// <summary>
		///             Template method allowing <see cref="Repository&lt;TEntity&gt;"/> implementations
		///             the ability to select a specific <typeparamref name="TEntity"/> instance
		///             by utilizing the <paramref name="where"/> parameter.
		/// </summary>
		/// <param name="context">
		///             The <see cref="ObjectContext"/> from which to locate an 
		///              <typeparamref name="TEntity"/>-based entity with 
		///             the specified <paramref name="where"/> parameter.
		/// </param>
		/// <param name="where">
		///             The <see cref="{Expression<Func<TEntity, bool>>}"/> lambda expression defining the filtering 
		///              strategy to apply when retrieving the 
		///              <typeparamref name="TEntity"/>-based entity from the data store.
		/// </param>
		/// <param name="includes">
		///             The navigational properties to be included with 
		///              the <typeparamref name="TEntity"/>-based entity.
		/// </param>
		/// <returns>
		///             The resulting <typeparamref name="TEntity"/> based entity.
		/// </returns>
		/// <remarks>
		///             <b>Note to inheritors: </b> do not dispose the specified 
		///              <see cref="ObjectContext"/> instance.
		/// </remarks>
		protected TEntity GetEntity(
			DbContext context,
			Expression<Func<TEntity, bool>> where,
			params Expression<Func<TEntity, object>>[] includes) {
			// Resulting instance.
			TEntity entity = default(TEntity);

			// Create relevant object set.
			var query =
				context
					.Set<TEntity>()
					.Where(
						obj => !obj.IsDeleted)
					.Where(where.Expand());

			// Add includes.
			includes
				.ForEachElement(
					include =>
						query = query.Include(include));

			entity =
				query
					.FirstOrDefault<TEntity>();

			return entity;
		}

		protected virtual bool IsValid(TEntity entity, DbContext context) {
			return true;
		}

		/// <summary>
		///             Adds the <typeparamref name="TEntity"/> instances specified in 
		///              parameter <paramref name="entities"/> to the
		///             current data repositories instance's collection of
		///              <typeparamref name="TEntity"/>-based instances.
		/// </summary>
		/// <param name="entities">
		///              A <see cref="IEnumerable{TEntity}"/>
		/// </param>
		public void Add(IEnumerable<TEntity> entities, int batch) {
			// Verify parameters.
			"entity".IsNotNullArgument(entities);
			//int count = 0;
			entities.GroupSplit(batch)
				.ForEachElement(
					groupEntity => {
						using (DbContext context = this.Connect()) {

							context.Set<TEntity>().Add(groupEntity);
							//groupEntity.ForEachElement(
							//    entity =>
							//    {
							//        if (this.IsValid(entity, context))
							//        {
							//            // Add accordingly.
							//            context.Set<TEntity>().Add(entity);
							//        }
							//        count++;
							//    });

							// Save.
							context.SaveChanges();
						}
					});

		}

		public async Task Add(IEnumerable<TEntity> entities, bool autoDetectChangesEnabled) {
			// Verify parameters.
			"entity".IsNotNullArgument(entities);


			using (var context = await this.ConnectAsync()) {
				context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
				context.Set<TEntity>().Add(entities);
				//groupEntity.ForEachElement(
				//    entity =>
				//    {
				//        if (this.IsValid(entity, context))
				//        {
				//            // Add accordingly.
				//            context.Set<TEntity>().Add(entity);
				//        }
				//        count++;
				//    });

				// Save.
				await context.SaveChangesAsync();
			}
		}

		#endregion

			#region Instance Fields

			/// <summary>
			///             The <see cref="IContextFactory"/> instance associated with the
			///             current <see cref="EntityRepository&lt;TEntity&gt;"/> instance.
			/// </summary>
		private readonly IContextFactory contextFactory;

		#endregion
	}
}
