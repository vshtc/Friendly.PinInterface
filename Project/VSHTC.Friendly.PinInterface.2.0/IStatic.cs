using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Interface for Static.
    /// </summary>
#else
    /// <summary>
    /// staticインターフェイスです。
    /// </summary>
#endif
    public interface IStatic : IAppFriendFunctions, IModifyOperationTypeInfo, IModifyAsync { }
}
