using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// staticインターフェイスです。
    /// </summary>
#else
    /// <summary>
    /// staticインターフェイスです。
    /// </summary>
#endif
    public interface IStatic : IAppFriendFunctions, IModifyOperationTypeInfo, IModifyAsync { }
}
