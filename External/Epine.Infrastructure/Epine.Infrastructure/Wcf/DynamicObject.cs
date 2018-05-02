using Epine.Infrastructure.Extensions;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Epine.Infrastructure.Wcf {
	internal class DynamicObject {

		#region Instance Properties

		internal Type ObjectType {
			get {
				return this._ObjType;
			}
		}

		internal object ObjectInstance {
			get {
				return this._Obj;
			}
		}

		#endregion

		#region Constructor

		internal DynamicObject(object obj) {
			"obj".IsNotNullArgument(obj);

			this._Obj = obj;
			this._ObjType = obj.GetType();
		}

		internal DynamicObject(Type type) {
			"type".IsNotNullArgument(type);

			this._ObjType = type;
		}

		#endregion

		#region Instance Methods

		internal void CallConstructor() {
			this.CallConstructor(
				new Type[0], 
				new object[0]);
		}

		internal void CallConstructor(Type[] paramTypes, object[] paramValues) {
			ConstructorInfo ctor = this._ObjType.GetConstructor(paramTypes);
			if (ctor == null) {
				throw 
					new DynamicProxyException(
						Constants.ErrorMessages.PROXY_CTOR_NOTFOUND);
			}

			this._Obj = ctor.Invoke(paramValues);
		}

		internal object GetProperty(string property) {
			return 
				this._ObjType
					.InvokeMember(
						property,
						BindingFlags.GetProperty | this.CommonBindingFlags,
						null /* Binder */,
						this._Obj,
						null /* args */);
		}

		internal object SetProperty(string property, object value) {
			return
				this._ObjType.
					InvokeMember(
						property,
						BindingFlags.SetProperty | this.CommonBindingFlags,
						null /* Binder */,
						this._Obj,
						new object[] { value });
		}

		internal object GetField(string field) {
			return
				this._ObjType
					.InvokeMember(
						field,
						BindingFlags.GetField | this.CommonBindingFlags,
						null /* Binder */,
						this._Obj,
						null /* args */);
		}

		internal object SetField(string field, object value) {
			return
				this._ObjType
					.InvokeMember(
						field,
						BindingFlags.SetField | this.CommonBindingFlags,
						null /* Binder */,
						this._Obj,
						new object[] { value });
		}

		internal object CallMethod(string method, params object[] parameters)
		{

		    var response = this._ObjType
		        .InvokeMember(
		            method,
		            BindingFlags.InvokeMethod | CommonBindingFlags,
		            null /* Binder */,
		            this._Obj,
		            parameters /* args */);

            return
                 response ?? new JArray();
		}

		internal object CallMethod(string method, Type[] types, object[] parameters) {

			if (types.Length != parameters.Length) {
				throw new ArgumentException(
					Constants.ErrorMessages.PARAMETER_VALUE_MISTMATCH);
			}

			MethodInfo methodInfo = this._ObjType.GetMethod(method, types);

			if (methodInfo == null) {
				throw new ApplicationException(string.Format(
					Constants.ErrorMessages.METHOD_NOTFOUND, method));
			}

			return
				methodInfo.Invoke(
					this._Obj, 
					this.CommonBindingFlags,
					null /* Binder */,
					parameters, 
					null /* Culture */);
		}


		#endregion

		#region Instance Fields

		private Type _ObjType;
		private object _Obj;
		private readonly BindingFlags CommonBindingFlags = BindingFlags.Instance | BindingFlags.Public;

		#endregion
	}
}
