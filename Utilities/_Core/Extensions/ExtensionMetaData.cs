using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    ///   This class contains information about an extension.
    /// </summary>
    public class ExtensionMetaData
    {
        /// <summary>
        ///   List of additional attributes.
        /// </summary>
        public List<object> AdditionalAttributes = new List<object>();

        /// <summary>
        ///   Extension attribute.
        /// </summary>
        public ExtensionAttribute Attribute;

        /// <summary>
        ///   Data type.
        /// </summary>
        public Type DataType;

        /// <summary>
        ///   Extension id.
        /// </summary>
        public string Id;

        /// <summary>
        ///   Extension instance.
        /// </summary>
        public object Instance;

        /// <summary>
        ///   Lamda associated with extension.
        /// </summary>
        public object Lamda;
    }
}