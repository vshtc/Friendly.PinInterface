using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Interface for Constructor.
    /// </summary>
#else
    /// <summary>
    /// コンストラクタ用インターフェイス。
    /// これを継承したインターフェイスのメソッドは指定の引数で対象タイプのコンストラクタを呼び出します。
    /// </summary>
#endif
    public interface IConstructor : IAppFriendFunctions, IModifyOperationTypeInfo { }
}
