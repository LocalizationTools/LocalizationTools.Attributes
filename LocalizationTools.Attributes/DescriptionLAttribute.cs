namespace LocalizationTools.Attributes
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// This is a shorter alias for <see cref="LocalizedDescriptionAttribute"/> with identical behavior.
    /// </summary>
    /// <seealso cref="LocalizationTools.Attributes.LocalizedDescriptionAttribute" />
    public class DescriptionLAttribute : LocalizedDescriptionAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with no parameters.</summary>
        public DescriptionLAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with a description.</summary>
        /// <param name="description">The description.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public DescriptionLAttribute([Localizable(true)] string description, [ResourceType] Type resourceType) : base(description, resourceType)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with a description.</summary>
        /// <param name="description">The description text.</param>
        public DescriptionLAttribute([Localizable(true)] string description) : base(description)
        {
        }
    }
}
