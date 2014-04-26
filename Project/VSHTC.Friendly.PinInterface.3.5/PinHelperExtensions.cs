using System;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Extension methods for pinning interface.
    /// </summary>
#else
    /// <summary>
    /// PinInterfaceHelperの処理を拡張メソッドで提供します。
    /// </summary>
#endif
    public static class PinHelperExtensions
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
        public static TInterface Pin<TInterface>(this AppFriend app)
        {
            return PinHelper.Pin<TInterface>(app);
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
        public static TInterface Pin<TInterface, TTarget>(this AppFriend app)
        {
            return PinHelper.Pin<TInterface, TTarget>(app);
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
        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType)
        {
            return PinHelper.Pin<TInterface>(app, targetType.FullName);
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
        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName)
        {
            return PinHelper.Pin<TInterface>(app, targetTypeFullName);
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
        public static TInterface PinConstructor<TInterface>(this AppFriend app)
        {
            return PinHelper.PinConstructor<TInterface>(app);
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
        public static TInterface PinConstructor<TInterface, TTarget>(this AppFriend app)
        {
            return PinHelper.PinConstructor<TInterface, TTarget>(app);
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
        public static TInterface PinConstructor<TInterface>(this AppFriend app, Type targetType)
        {
            return PinHelper.PinConstructor<TInterface>(app, targetType.FullName);
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
        public static TInterface PinConstructor<TInterface>(this AppFriend app, string targetTypeFullName)
        {
            return PinHelper.PinConstructor<TInterface>(app, targetTypeFullName);
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
        public static TInterface Pin<TInterface>(this AppVar appVar)
        {
            return PinHelper.Pin<TInterface>(appVar);
        }
    }
}
