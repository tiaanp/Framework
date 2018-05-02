using Epine.Infrastructure.Contracts;
using Epine.Infrastructure.Extensions;
using Epine.Infrastructure.Resolvers;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Binding = System.ServiceModel.Channels.Binding;

namespace Epine.Infrastructure.Wcf {

	/// <summary>
	/// 
	/// </summary>
	public class ProxyFacade
	{
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceConfig"></param>
		public ProxyFacade(IServiceConfig serviceConfig)
		{
			this._ServiceConfig = serviceConfig;

			this._Binding =
				(Binding)Activator
					.CreateInstance(
						typeof(Binding)
							.Assembly
							.GetType(
								serviceConfig
									.BindingType
									.Description()));
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object CallMethod(params object[] parameters)
		{
			// Set the binding timeouts in minutes
			this._Binding.ReceiveTimeout = (new TimeSpan(0, _ServiceConfig.Timeout, 0));
			this._Binding.SendTimeout = (new TimeSpan(0, _ServiceConfig.Timeout, 0));
			this._Binding.OpenTimeout = (new TimeSpan(0, _ServiceConfig.Timeout, 0));

	
			var t = (HttpBindingBase) this._Binding;
			t.MaxReceivedMessageSize = 655360000;
			ServiceEndpoint endpoint =
				new ServiceEndpoint(
					new ContractDescription(this._ServiceConfig.Contract),
					t,
					new EndpointAddress(this._ServiceConfig.EndPoint));
		 

			DynamicProxyFactory factory = new DynamicProxyFactory(this._ServiceConfig.WsdlUri);
			foreach (ContractDescription description in factory.Contracts)
			{
				foreach (OperationDescription operation in description.Operations)
				{
					DataContractSerializerOperationBehavior dataContractBehavior =
						 operation.Behaviors.Find<DataContractSerializerOperationBehavior>()
						 as DataContractSerializerOperationBehavior;
					if (dataContractBehavior != null)
					{
						dataContractBehavior.MaxItemsInObjectGraph = 655360000;
					}
					
				}
			}
		   
			
			DynamicProxy proxy = factory.CreateProxy(endpoint);
			// Left In for future use if necessary
			//SetProperties(proxy);
		    if (parameters == null)
		    {
                parameters = new object[0];
            }
            else if (parameters.Length > 0)
			{
				if (string.IsNullOrEmpty(parameters[0].ToString()))
				{
					parameters = new object[0];
				}
			}
		

			return
				proxy
					.CallMethod(
						this._ServiceConfig.Method,
						parameters);
			//return
			//	this._DynamicProxy
			//		.CallMethod(
			//			this._ServiceConfig.Method,
			//			parameters);
		}

		#endregion


		#region Instance Fields

		private readonly IServiceConfig _ServiceConfig;
		///private DynamicProxy _DynamicProxy;
		private Binding _Binding;


		#endregion
	}


	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public sealed class ProxyFacade<TResult> : ProxyFacade
	{

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceConfig"></param>
		public ProxyFacade(IServiceConfig serviceConfig)
			: base(serviceConfig)
		{
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		new public TResult CallMethod(params object[] parameters)
		{

			return
				ObjectResolverFactory
					.GetResolver<TResult>()
					.GetResult(
						base.CallMethod(parameters));
		}

		#endregion
	}
}
