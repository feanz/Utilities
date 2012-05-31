using System;

namespace Utilities
{
    /// <summary>
    ///   This is a default attribute class, used to decorate implementation of dynamically loadable extension classes.
    /// </summary>
    public class ExtensionArgAttribute : Attribute
    {
        /// <summary>
        ///   Date type of the argument.
        /// </summary>
        public Type DataType;

        /// <summary>
        ///   Default value.
        /// </summary>
        public object DefaultValue;

        /// <summary>
        ///   Describes the argument name.
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        ///   Example value.
        /// </summary>
        public string Example = string.Empty;

        /// <summary>
        ///   Example of mutliple various values.
        /// </summary>
        public string ExampleMultiple = string.Empty;

        /// <summary>
        ///   Indicates if is required.
        /// </summary>
        public bool IsRequired;

        /// <summary>
        ///   Name of the arg
        /// </summary>
        public string Name;

        /// <summary>
        ///   Order number for the argument.
        /// </summary>
        public int OrderNum;

        /// <summary>
        ///   Used to group various arguments. e.g. The tag can be used to separate base/derived argument defintions.
        /// </summary>
        public string Tag = string.Empty;
    }
}