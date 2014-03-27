namespace VSHTC.Friendly.PinInterface.BaseInterfaces
{   
    /// <summary>
    /// 型情報修飾
    /// </summary>
    public interface IModifyOperationTypeInfo
    {
        /// <summary>
        /// 次回呼び出しの型を明確にします。
        /// </summary>
        /// <param name="target">操作を保持する型フルネームです。</param>
        /// <param name="arguments">引数の型のフルネーム配列。</param>
        void OperationTypeInfoNext(string target, params string[] arguments);
    }
}
