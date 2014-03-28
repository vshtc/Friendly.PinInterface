using System;

namespace VSHTC.Friendly.PinInterface
{
    /// <summary>
    /// プロクシ対象のタイプを指定する
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TargetTypeAttribute : Attribute
    {
        /// <summary>
        /// プロクシ対象タイプのフルネームです。
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="fullNmae">プロクシ対象タイプのフルネーム。</param>
        public TargetTypeAttribute(string fullNmae)
        {
            FullName = fullNmae;
        }
    }
}
