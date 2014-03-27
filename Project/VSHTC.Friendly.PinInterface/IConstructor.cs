using VSHTC.Friendly.PinInterface.BaseInterfaces;

namespace VSHTC.Friendly.PinInterface
{
    /// <summary>
    /// コンストラクタ用インターフェイス。
    /// これを継承したインターフェイスのメソッドは指定の引数で対象タイプのコンストラクタを呼び出します。
    /// </summary>
    public interface IConstructor : IAppFriendFunctions, IModifyOperationTypeInfo { }
}
