namespace VSHTC.Friendly.PinInterface.FunctionalInterfaces
{
#if ENG
    /// <summary>
    /// Interface for OperationTypeInfo modifying.
    /// </summary>
#else
    /// <summary>
    /// 型情報修飾。
    /// </summary>
#endif
    public interface IModifyOperationTypeInfo
    {
#if ENG
        /// <summary>
        /// Set OperationTypeInfo to next operation.
        /// </summary>
        /// <param name="target">The full class name of the target type for the target operation.</param>
        /// <param name="arguments">The full class names of the the target operation's parameters.</param>
#else
        /// <summary>
        /// 次回呼び出しの型を明確にします。
        /// </summary>
        /// <param name="target">操作を保持する型フルネームです。</param>
        /// <param name="arguments">引数の型のフルネーム配列。</param>
#endif
        void OperationTypeInfoNext(string target, params string[] arguments);
    }
}
