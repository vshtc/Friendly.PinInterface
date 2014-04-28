using System;
using System.Runtime.Remoting;
using VSHTC.Friendly.PinInterface.Inside;
using VSHTC.Friendly.PinInterface.Properties;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Helper methods for pinning interface.
    /// </summary>
#else
    /// <summary>
    /// PinInterfaceを使用するためのヘルパメソッドです。
    /// </summary>
#endif
    public static class PinHelper
    {
#if ENG
        /// <summary>
        /// Pin static opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// Static操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface Pin<TInterface>(AppFriend app)
        {
            string targetTypeFullName = TargetTypeUtility.GetFullNameTopMustHaveAttr(app, typeof(TInterface));
            if (string.IsNullOrEmpty(targetTypeFullName))
            {
                throw new NotSupportedException(Resources.ErrorNotFoundTargetType);
            }
            return Pin<TInterface>(app, targetTypeFullName);
        }

#if ENG
        /// <summary>
        /// Pin static opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <typeparam name="TTarget">Proxy target type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// Static操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <typeparam name="TTarget">対応するタイプ。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface Pin<TInterface, TTarget>(AppFriend app)
        {
            return Pin<TInterface>(app, typeof(TTarget));
        }

#if ENG
        /// <summary>
        /// Pin static opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetType">Proxy target type.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// Static操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetType">対応するタイプ。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface Pin<TInterface>(AppFriend app, Type targetType)
        {
            return Pin<TInterface>(app, targetType.FullName);
        }

#if ENG
        /// <summary>
        /// Pin static opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetTypeFullName">Proxy target type full name.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// Static操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetTypeFullName">対応するタイプフルネーム。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface Pin<TInterface>(AppFriend app, string targetTypeFullName)
        {
            return FriendlyProxyFactory.WrapFriendlyProxy<TInterface>(typeof(FriendlyProxyStatic<>), app, targetTypeFullName);
        }

#if ENG
        /// <summary>
        /// Pin Constructor by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for Constructor.</returns>
#else
        /// <summary>
        /// コンストラクタの呼び出しを指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">コンストラクタインターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>コンストラクタインターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface>(AppFriend app)
        {
            string targetTypeFullName = TargetTypeUtility.GetFullNameTopMustHaveAttr(app, typeof(TInterface));
            if (string.IsNullOrEmpty(targetTypeFullName))
            {
                throw new NotSupportedException(Resources.ErrorNotFoundTargetType);
            }
            return PinConstructor<TInterface>(app, targetTypeFullName);
        }

#if ENG
        /// <summary>
        /// Pin Constructor by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <typeparam name="TTarget">Proxy target type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for Constructor.</returns>
#else
        /// <summary>
        /// コンストラクタの呼び出しを指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">コンストラクタインターフェイス。</typeparam>
        /// <typeparam name="TTarget">対応するタイプ。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>コンストラクタインターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface, TTarget>(AppFriend app)
        {
            return PinConstructor<TInterface>(app, typeof(TTarget));
        }

#if ENG
        /// <summary>
        /// Pin Constructor by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetType">Proxy target type.</param>
        /// <returns>Interface for Constructor.</returns>
#else
        /// <summary>
        /// コンストラクタの呼び出しを指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">コンストラクタインターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetType">対応するタイプ。</param>
        /// <returns>コンストラクタインターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface>(AppFriend app, Type targetType)
        {
            return PinConstructor<TInterface>(app, targetType.FullName);
        }

#if ENG
        /// <summary>
        /// Pin Constructor by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetTypeFullName">Proxy target type full name.</param>
        /// <returns>Interface for Constructor.</returns>
#else
        /// <summary>
        /// コンストラクタの呼び出しを指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">コンストラクタインターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetTypeFullName">対応するタイプフルネーム。</param>
        /// <returns>コンストラクタインターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface>(AppFriend app, string targetTypeFullName)
        {
            return FriendlyProxyFactory.WrapFriendlyProxy<TInterface>(typeof(FriendlyProxyConstructor<>), app, targetTypeFullName);
        }


#if ENG
        /// <summary>
        /// Pin AppVar's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="appVar">Application varialbe manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppVarの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="appVar">アプリケーション変数。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface Pin<TInterface>(AppVar appVar)
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(appVar).GetTransparentProxy();
        }
       
#if ENG
        /// <summary>
        /// Get AppVar from interface.
        /// </summary>
        /// <param name="pinnedInterface">Interface.</param>
        /// <returns>AppVar.</returns>
#else
        /// <summary>
        /// 指定のインターフェイスの元となったAppVarを取得します。
        /// </summary>
        /// <param name="pinnedInterface">インターフェイス。</param>
        /// <returns>AppVar。</returns>
#endif
        public static AppVar GetAppVar(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IAppVarOwner;
            return proxy == null ? null : proxy.AppVar;
        }
        
#if ENG
        /// <summary>
        /// Get copy of object.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="pinnedInterface">Interface.</param>
        /// <returns>Copy of object.</returns>
#else
        /// <summary>
        /// 対象のオブジェクトのコピーを取得します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型</typeparam>
        /// <param name="pinnedInterface">インターフェイス。</param>
        /// <returns>オブジェクトのコピー。</returns>
#endif
        public static T GetValue<T>(object pinnedInterface)
        {
            var appVar = GetAppVar(pinnedInterface);
            if (appVar == null)
            {
                throw new NotSupportedException(Resources.ErrorNotProxy);
            }
            return (T)appVar.Core;
        }

#if ENG
        /// <summary>
        /// A next call is made asynchronous. 
        /// </summary>
        /// <param name="pinnedInterface">Interface.</param>
        /// <returns>Asynchronous execution.</returns>
#else        
        /// <summary>
        /// 次回の呼び出しを非同期にします。
        /// </summary>
        /// <param name="pinnedInterface">インターフェイス。</param>
        /// <returns>非同期実行オブジェクト。</returns>
#endif
        public static Async AsyncNext(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyAsync;
            if (proxy == null)
            {
                throw new NotSupportedException(Resources.ErrorCanNotUseAsync);
            }
            return proxy.AsyncNext();
        }

#if ENG
        /// <summary>
        /// Specify next call type.
        /// Please refer to description of OperationTypeInfo for details. 
        /// </summary>
        /// <param name="pinnedInterface">Interface.</param>
        /// <param name="operationTypeInfo">Inovke type infomation.</param>
#else
        /// <summary>
        /// 次回呼び出しの型を特定します。
        /// 詳しくはOperationTypeInfoの解説を参照してください。
        /// </summary>
        /// <param name="pinnedInterface">インターフェイス。</param>
        /// <param name="operationTypeInfo">呼び出し型情報</param>
#endif
        public static void OperationTypeInfoNext(object pinnedInterface, OperationTypeInfo operationTypeInfo)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyOperationTypeInfo;
            if (proxy == null)
            {
                throw new NotSupportedException(Resources.ErrorNotProxy);
            }
            proxy.OperationTypeInfoNext(operationTypeInfo);
        }

#if ENG
        /// <summary>
        /// Specify next call type.
        /// Type information is automatically guessed from interface definition.
        /// Please refer to description of OperationTypeInfo for details. 
        /// </summary>
        /// <param name="pinnedInterface">Interface.</param>
#else
        /// <summary>
        /// 次回呼び出しの型を特定します。
        /// 型情報はインターフェイスの定義から自動で推測されます。
        /// 詳しくはOperationTypeInfoの解説を参照してください。
        /// </summary>
        /// <param name="pinnedInterface">インターフェイス。</param>
#endif
        public static void OperationTypeInfoNext(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyOperationTypeInfo;
            if (proxy == null)
            {
                throw new NotSupportedException(Resources.ErrorNotProxy);
            }
            proxy.SetOperationTypeInfoNextAuto();
        }
    }
}
