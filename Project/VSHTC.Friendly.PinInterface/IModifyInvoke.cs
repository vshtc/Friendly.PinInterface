using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface
{
    /// <summary>
    /// 次回呼び出しの修飾。
    /// </summary>
    public interface IModifyInvoke
    {
        /// <summary>
        /// 次回呼び出しを非同期で実行させます。
        /// </summary>
        /// <returns>非同期実行オブジェクト</returns>
        Async AsyncNext();

        /// <summary>
        /// 次回呼び出しの型を明確にします。
        /// </summary>
        /// <param name="target">操作を保持する型フルネームです。</param>
        /// <param name="arguments">引数の型のフルネーム配列。</param>
        void OperationTypeInfoNext(string target, params string[] arguments);
    }
}
