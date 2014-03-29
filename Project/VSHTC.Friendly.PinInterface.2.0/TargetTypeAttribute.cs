using System;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// プロクシ対象のタイプを指定する
    /// </summary>
#else
    /// <summary>
    /// プロクシ対象のタイプを指定する
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TargetTypeAttribute : Attribute
    {
#if ENG
        /// <summary>
        /// プロクシ対象タイプのフルネームです。
        /// </summary>
#else
        /// <summary>
        /// プロクシ対象タイプのフルネームです。
        /// </summary>
#endif
        public string FullName { get; private set; }

#if ENG
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="fullNmae">プロクシ対象タイプのフルネーム。</param>
#else
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="fullNmae">プロクシ対象タイプのフルネーム。</param>
#endif
        public TargetTypeAttribute(string fullNmae)
        {
            FullName = fullNmae;
        }
    }
}
