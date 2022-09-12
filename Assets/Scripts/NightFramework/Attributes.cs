using System;
using UnityEngine;

namespace NightFramework
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyIfNotPersistentAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class HideInInspectorIfNotDebugAttribute : PropertyAttribute { }
}
