using System;

namespace VSHTC.Friendly.PinInterface
{
#if ENG
    /// <summary>
    /// Specify proxy target type.
    /// </summary>
#else
    /// <summary>
    /// プロクシ対象のタイプを指定します。
    /// </summary>
#endif
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TargetTypeAttribute : Attribute
    {
#if ENG
        /// <summary>
        /// Proxy target type full name.
        /// </summary>
#else
        /// <summary>
        /// プロクシ対象タイプのフルネームです。
        /// </summary>
#endif
        public string FullName { get; private set; }

#if ENG
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fullNmae">Proxy target type full name.</param>
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
