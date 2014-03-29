using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.FunctionalInterfaces
{
#if ENG
    /// <summary>
    /// 非同期修飾。
    /// </summary>
#else
    /// <summary>
    /// 非同期修飾。
    /// </summary>
#endif
    public interface IModifyAsync
    {
        /// <summary>
        /// 次回呼び出しを非同期で実行させます。
        /// </summary>
        /// <returns>非同期実行オブジェクト。</returns>
        /// <summary>
        /// 次回呼び出しを非同期で実行させます。
        /// </summary>
        /// <returns>非同期実行オブジェクト。</returns>
        Async AsyncNext();
    }
}
