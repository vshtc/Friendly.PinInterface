using System;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.FunctionalInterfaces;
using VSHTC.Friendly.PinInterface.Properties;

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
    public static class InterfaceHelper
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
        where TInterface : IAppFriendFunctions
        {
            string targetTypeFullName = TargetTypeUtility.GetFullName(app, typeof(TInterface));
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
        where TInterface : IAppFriendFunctions
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
        where TInterface : IAppFriendFunctions
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
        where TInterface : IAppFriendFunctions
        {
            Type proxyType = TypeUtility.HasInterface(typeof(TInterface), typeof(IStatic)) ?
                typeof(FriendlyProxyStatic<>) : typeof(FriendlyProxyConstructor<>);
            return FriendlyProxyFactory.WrapFriendlyProxy<TInterface>(proxyType, app, targetTypeFullName);
        }

#if ENG
        /// <summary>
        /// Copies indicated object into the target application and returns a DynamicAppVar to access it.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <param name="obj">Object to be sent (must be serializable, AppVar, or DynamicAppVar).</param>
        /// <returns>DynamicAppVar.</returns>
#else
        /// <summary>
        /// テスト対象アプリケーション内に指定のオブジェクトをコピーし、その変数を操作するインターフェイスを返します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="obj">初期化オブジェクト（シリアライズ可能なオブジェクトもしくはAppVarであること）。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface PinCopy<TInterface>(AppFriend app, object obj)
        where TInterface : IInstance
        {
            return Pin<TInterface>(app.Dim(obj));
        }

#if ENG
        /// <summary>
        /// Declares a null variable in the target application and returns a DynamicAppVar to access it.
        /// </summary>
        /// <typeparam name="TInterface">Interface type.</typeparam>
        /// <param name="app">Application manipulation object.</param>
        /// <returns>DynamicAppVar.</returns>
#else
        /// <summary>
        /// テスト対象アプリケーション内にnullの変数を宣言し、その変数を操作するインターフェイスを返します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
#endif
        public static TInterface PinNull<TInterface>(AppFriend app)
        where TInterface : IInstance
        {
            return Pin<TInterface>(app.Dim());
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
             where TInterface : IInstance
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(appVar).GetTransparentProxy();
        }

#if ENG
        /// <summary>
        /// Cast.
        /// </summary>
        /// <typeparam name="T">Cast type.</typeparam>
        /// <param name="source">Source.</param>
        /// <returns>Destination.</returns>
#else
        /// <summary>
        /// 指定の型にキャストします。
        /// TがIInstane型であれば、その操作用インターフェイスを返します。
        /// それ以外であれば、AppVarの中身をシリアライズして、対象アプリケーションからテストプロセスへ転送、取得します。
        /// </summary>
        /// <typeparam name="T">キャストする型。</typeparam>
        /// <param name="source">元。</param>
        /// <returns>後。</returns>
#endif
        public static T Cast<T>(IAppVarOwner source)
        {
            if (TypeUtility.HasInterface(typeof(T), typeof(IInstance)))
            {
                return FriendlyProxyFactory.WrapFriendlyProxy<T>(typeof(FriendlyProxyInstance<>), source.AppVar);
            }
            else if (UserWrapperUtility.IsAppVarWrapper(typeof(T)))
            {
                return (T)UserWrapperUtility.CreateWrapper(typeof(T), source.AppVar);
            }
            else
            {
                return (T)source.AppVar.Core;
            }
        }
    }
}
