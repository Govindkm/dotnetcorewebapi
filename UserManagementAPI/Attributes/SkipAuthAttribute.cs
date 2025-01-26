using System;

namespace UserManagementAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SkipAuthAttribute : Attribute
    {
    }
}
