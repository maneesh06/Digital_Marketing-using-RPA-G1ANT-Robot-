

using System;
/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Selenium
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
namespace G1ANT.Addon.Tumblr.Api.Enums
{
    public enum AttributeOperationType
    {
        PreferAttribute,
        ForceAttribute,
        ForceProperty,
        Default = PreferAttribute
    }

    public static class AttributeOperationTypeParser
    {
        public static AttributeOperationType Parse(string operationTypeName)
        {
            if (string.IsNullOrEmpty(operationTypeName))
                return AttributeOperationType.Default;

            return (AttributeOperationType)Enum.Parse(typeof(AttributeOperationType), operationTypeName, true);
        }
    }
}
