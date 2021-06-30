using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Octopus.DbUp
{
    public static class Constants
    {
        public const string Separator = "-====-";

        public static class ScriptFolder
        {
            public const string RunAlwaysPreScripts = "RunAlwaysPreScripts";
            public const string RunAlwaysPostScripts = "RunAlwaysPostScripts";
            public const string PreDeploymentScripts = "PreDeploymentScripts";
            public const string DeploymentScripts = "DeploymentScripts";
            public const string PostDeploymentScripts = "PostDeploymentScripts";
        }

        public static List<T> GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }
    }
}
