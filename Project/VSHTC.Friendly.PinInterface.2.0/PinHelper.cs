using System;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.Properties;
using System.Runtime.Remoting;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Helper methods for pinning interface.
    /// </summary>
#else
    /// <summary>
    /// インターフェイス付加、変更のためのヘルパメソッドです。
    /// </summary>
#endif
    public static class PinHelper
    {
#if ENG
        /// <summary>
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
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
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <typeparam name="TTarget">Proxy target type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
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
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetType">Proxy target type.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
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
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetTypeFullName">Proxy target type full name.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
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
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
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
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <typeparam name="TTarget">Proxy target type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <typeparam name="TTarget">対応するタイプ。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface, TTarget>(AppFriend app)
        {
            return PinConstructor<TInterface>(app, typeof(TTarget));
        }

#if ENG
        /// <summary>
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetType">Proxy target type.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetType">対応するタイプ。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface PinConstructor<TInterface>(AppFriend app, Type targetType)
        {
            return PinConstructor<TInterface>(app, targetType.FullName);
        }

#if ENG
        /// <summary>
        /// Pin AppFriend's opertion by TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="targetTypeFullName">Proxy target type full name.</param>
        /// <returns>Interface for manipulation.</returns>
#else
        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetTypeFullName">対応するタイプフルネーム。</param>
        /// <returns>操作用インターフェイス。</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinnedInterface"></param>
        /// <returns></returns>
        public static AppVar GetAppVar(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IAppVarOwner;
            return proxy == null ? null : proxy.AppVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinnedInterface"></param>
        /// <returns></returns>
        public static T GetValue<T>(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IAppVarOwner;
            if (proxy == null)
            {
                throw new NotSupportedException();
            }
            return (T)proxy.AppVar.Core;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinnedInterface"></param>
        /// <returns></returns>
        public static Async AsyncNext(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyAsync;
            if (proxy == null)
            {
                throw new NotSupportedException();
            }
            return proxy.AsyncNext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinnedInterface"></param>
        /// <param name="operationTypeInfo"></param>
        public static void OperationTypeInfoNext(object pinnedInterface, OperationTypeInfo operationTypeInfo)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyOperationTypeInfo;
            if (proxy == null)
            {
                throw new NotSupportedException();
            }
            proxy.OperationTypeInfoNext(operationTypeInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinnedInterface"></param>
        public static void OperationTypeInfoNext(object pinnedInterface)
        {
            var proxy = RemotingServices.GetRealProxy(pinnedInterface) as IModifyOperationTypeInfo;
            if (proxy == null)
            {
                throw new NotSupportedException();
            }
            proxy.SetOperationTypeInfoNextAuto();
        }
    }
}
