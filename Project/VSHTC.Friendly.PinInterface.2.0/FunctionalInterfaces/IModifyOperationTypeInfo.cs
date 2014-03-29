namespace VSHTC.Friendly.PinInterface.FunctionalInterfaces
{
#if ENG
    /// <summary>
    /// 型情報修飾。
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
        /// 次回呼び出しの型を明確にします。
        /// </summary>
        /// <param name="target">操作を保持する型フルネームです。</param>
        /// <param name="arguments">引数の型のフルネーム配列。</param>
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
