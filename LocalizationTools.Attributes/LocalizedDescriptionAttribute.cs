
namespace LocalizationTools.Attributes
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// LocalizedDescriptionAttribute is a localizable descendant of the <see cref="DescriptionAttribute"/>.
    /// The string value of this class can be used either as literal or as resource identifier into a specified
    /// <see cref="ResourceType"/>
    /// </summary>
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string description;
        private Type resourceType;
        private readonly LocalizableString localizableString;

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with no parameters.</summary>
        public LocalizedDescriptionAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with a description.</summary>
        /// <param name="description">The description.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public LocalizedDescriptionAttribute([Localizable(true)] string description, [ResourceType] Type resourceType)
        {
            this.localizableString  = new LocalizableString(nameof(this.Description), description, resourceType);
            this.ResourceType = resourceType;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute"></see> class with a description.</summary>
        /// <param name="description">The description text.</param>
        public LocalizedDescriptionAttribute([Localizable(true)] string description)
        {
            this.description = description;
            this.localizableString  = new LocalizableString(nameof(this.Description), description);
            this.ResourceType = null;
        }

        /// <summary>
        /// Gets the UI display string for Description.
        /// <para>
        /// This can be either a literal, non-localized string provided to <see cref="Description"/> or the
        /// localized string found when <see cref="ResourceType"/> has been specified and <see cref="Description"/>
        /// represents a resource key within that resource type.
        /// </para>
        /// </summary>
        /// <returns>
        /// When <see cref="ResourceType"/> has not been specified, the value of
        /// <see cref="Description"/> will be returned.
        /// <para>
        /// When <see cref="ResourceType"/> has been specified and <see cref="Description"/>
        /// represents a resource key within that resource type, then the localized value will be returned.
        /// </para>
        /// </returns>
        public override string Description
        {
            get
            {
                return this.localizableString.GetLocalizableValue();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Type"/> that contains the resources for <see cref="Description"/>.
        /// </summary>
        [ResourceType]
        public Type ResourceType
        {
            get
            {
                return this.resourceType;
            }

            set
            {
                if (this.resourceType != value)
                {
                    this.resourceType = value;

                    this.localizableString.ResourceType = value;
                }
            }
        }
    }
}
