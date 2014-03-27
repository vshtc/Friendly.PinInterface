using System;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface
{
    /// <summary>
    /// インターフェイス付加のためのヘルパメソッドです。
    /// </summary>
    public static class InterfaceHelper
    {
        /// <summary>
        /// AppVarの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="appVar">アプリケーション変数。</param>
        /// <returns>操作用インターフェイス。</returns>
        public static TInterface Pin<TInterface>(this AppVar appVar)
             where TInterface : IInstance
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(appVar).GetTransparentProxy();
        }

        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <typeparam name="TTarget">対応するタイプ。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <returns>操作用インターフェイス。</returns>
        public static TInterface Pin<TInterface, TTarget>(this AppFriend app)
        where TInterface : IAppFriendFunctions
        {
            return Pin<TInterface>(app, typeof(TTarget));
        }

        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetType">対応するタイプ。</param>
        /// <returns>操作用インターフェイス。</returns>
        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType)
        where TInterface : IAppFriendFunctions
        {
            return Pin<TInterface>(app, targetType.FullName);
        }

        /// <summary>
        /// AppFriendの操作を指定のインターフェイスで固定します。
        /// </summary>
        /// <typeparam name="TInterface">操作用インターフェイス。</typeparam>
        /// <param name="app">アプリケーション操作クラス。</param>
        /// <param name="targetTypeFullName">対応するタイプフルネーム。</param>
        /// <returns>操作用インターフェイス。</returns>
        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName)
        where TInterface : IAppFriendFunctions
        {
            Type proxyType = TypeUtility.HasInterface(typeof(TInterface), typeof(IStatic)) ?
                typeof(FriendlyProxyStatic<>) : typeof(FriendlyProxyConstructor<>);
            return FriendlyProxyFactory.WrapFriendlyProxy<TInterface>(proxyType, app, targetTypeFullName);
        }

        /// <summary>
        /// 指定の型にキャストします。
        /// TがIInstane型であれば、その操作用インターフェイスを返します。
        /// それ以外であれば、AppVarの中身をシリアライズして、対象アプリケーションからテストプロセスへ転送、取得します。
        /// </summary>
        /// <typeparam name="T">キャストする型。</typeparam>
        /// <param name="source">元。</param>
        /// <returns>後。</returns>
        public static T Cast<T>(this IAppVarOwner source)
        {
            return TypeUtility.HasInterface(typeof(T), typeof(IInstance)) ?
                FriendlyProxyFactory.WrapFriendlyProxy<T>(typeof(FriendlyProxyInstance<>), source.AppVar) :
                (T)source.AppVar.Core;
        }
    }
}
