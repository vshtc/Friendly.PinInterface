using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Interface for Instance.
    /// </summary>
#else
    /// <summary>
    /// インスタンスのインターフェイスです。
    /// </summary>
#endif
    public interface IInstance : IModifyAsync, IAppVarFunctions { }
}
