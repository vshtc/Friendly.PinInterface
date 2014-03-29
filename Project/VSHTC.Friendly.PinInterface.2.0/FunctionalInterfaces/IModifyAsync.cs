using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.FunctionalInterfaces
{
#if ENG
    /// <summary>
    /// Interface for Async modifying.
    /// </summary>
#else
    /// <summary>
    /// 非同期修飾。
    /// </summary>
#endif
    public interface IModifyAsync
    {
#if ENG
        /// <summary>
        /// Set Async to next operation.
        /// </summary>
        /// <returns>Asynchronous object.</returns>
#else
        /// <summary>
        /// 次回呼び出しを非同期で実行させます。
        /// </summary>
        /// <returns>非同期実行オブジェクト。</returns>
#endif
        Async AsyncNext();
    }
}
