using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class AppVarUtility
    {
        internal static bool IsNull(AppVar appVar)
        {
            return (bool)appVar.App[typeof(object), "ReferenceEquals"](null, appVar).Core;
        }
    }
}
