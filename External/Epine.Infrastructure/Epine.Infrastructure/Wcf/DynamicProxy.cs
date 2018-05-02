
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Epine.Infrastructure.Wcf {
	internal sealed class DynamicProxy : DynamicObject {

		#region Instance Properties

		internal Type ProxyType {
			get {
				return ObjectType;
			}
		}

		internal object Proxy {
			get {
				return ObjectInstance;
			}
		}

		#endregion

		#region Constructor

		internal DynamicProxy(Type proxyType, Binding binding, EndpointAddress address)
			: base(proxyType) {

			base.CallConstructor(
				new[] {
					typeof(Binding),
					typeof(EndpointAddress)
				},
				new object[] {
					binding,
					address
				});
		}

		#endregion

		#region Instance Methods

		internal void Close() {
			base.CallMethod("Close");
		}

		#endregion
	}
}
