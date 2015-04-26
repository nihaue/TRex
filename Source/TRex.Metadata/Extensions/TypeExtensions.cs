using System;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class TypeExtensions
    {
        public static string AssemblyQualifiedNameNoTypeParams(this Type type)
        {
            var typeParamIndex = type.AssemblyQualifiedName.IndexOf("`",
                StringComparison.OrdinalIgnoreCase);
            return (typeParamIndex == -1) ? type.FullName : type.FullName.Substring(0, typeParamIndex);
        }
    }
}
