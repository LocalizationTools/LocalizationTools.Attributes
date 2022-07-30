namespace LocalizationTools.Attributes
{
    using System;

    using System.Reflection;

    /// <summary>
    /// A helper class for providing a localizable string property.
    /// This class is currently compiled in both System.Web.dll and System.ComponentModel.DataAnnotations.dll.
    /// </summary>
    internal class LocalizableString
    {
        #region Member fields

        private readonly string propertyName;
        private string propertyValue;
        private Type resourceType;

        private Func<string> cachedResult;

        #endregion

        #region All Constructors

        /// <summary>
        /// Constructs a localizable string, specifying the property name associated
        /// with this item.  The <paramref name="propertyName"/> value will be used
        /// within any exceptions thrown as a result of localization failures.
        /// </summary>
        /// <param name="propertyName">The name of the property being localized.  This name
        /// will be used within exceptions thrown as a result of localization failures.</param>
        public LocalizableString(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Constructs a localizable string, specifying the property name associated
        /// with this item.  The <paramref name="propertyName" /> value will be used
        /// within any exceptions thrown as a result of localization failures.
        /// </summary>
        /// <param name="propertyName">The name of the property being localized.  This name
        /// will be used within exceptions thrown as a result of localization failures.</param>
        /// <param name="stringValue">The string value.</param>
        public LocalizableString(string propertyName, string stringValue)
        {
            this.propertyName = propertyName;
            this.Value = stringValue;
        }

        /// <summary>
        /// Constructs a localizable string, specifying the property name associated
        /// with this item.  The <paramref name="propertyName" /> value will be used
        /// within any exceptions thrown as a result of localization failures.
        /// </summary>
        /// <param name="propertyName">The name of the property being localized.  This name
        /// will be used within exceptions thrown as a result of localization failures.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public LocalizableString(string propertyName, string resourceKey, Type resourceType)
        {
            this.propertyName = propertyName;
            this.Value = resourceKey;
            this.ResourceType = resourceType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of this localizable string.  This value can be
        /// either the literal, non-localized value, or it can be a resource name
        /// found on the resource type supplied to <see cref="GetLocalizableValue"/>.
        /// </summary>
        public string Value
        {
            get { return this.propertyValue; }
            set
            {
                if (this.propertyValue != value)
                {
                    this.ClearCache();
                    this.propertyValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the resource type to be used for localization.
        /// </summary>
        public Type ResourceType
        {
            get { return this.resourceType; }
            set
            {
                if (this.resourceType != value)
                {
                    this.ClearCache();
                    this.resourceType = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears any cached values, forcing <see cref="GetLocalizableValue"/> to
        /// perform evaluation.
        /// </summary>
        private void ClearCache()
        {
            this.cachedResult = null;
        }

        /// <summary>
        /// Gets the potentially localized value.
        /// </summary>
        /// <remarks>
        /// If <see cref="ResourceType"/> has been specified and <see cref="Value"/> is not
        /// null, then localization will occur and the localized value will be returned.
        /// <para>
        /// If <see cref="ResourceType"/> is null then <see cref="Value"/> will be returned
        /// as a literal, non-localized string.
        /// </para>
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if localization fails.  This can occur if <see cref="ResourceType"/> has been
        /// specified, <see cref="Value"/> is not null, but the resource could not be
        /// accessed.  <see cref="ResourceType"/> must be a public class, and <see cref="Value"/>
        /// must be the name of a public static string property that contains a getter.
        /// </exception>
        /// <returns>
        /// Returns the potentially localized value.
        /// </returns>
        public string GetLocalizableValue()
        {
            if (this.cachedResult == null)
            {
                // If the property value is null, then just cache that value
                // If the resource type is null, then property value is literal, so cache it
                if (this.propertyValue == null || this.resourceType == null)
                {
                    this.cachedResult = () => this.propertyValue;
                }
                else
                {
                    // Get the property from the resource type for this resource key
                    PropertyInfo property = this.resourceType.GetProperty(this.propertyValue);

                    // We need to detect bad configurations so that we can throw exceptions accordingly
                    bool badlyConfigured = false;

                    // Make sure we found the property and it's the correct type, and that the type itself is public
                    if (!this.resourceType.IsVisible || property == null || property.PropertyType != typeof(string))
                    {
                        badlyConfigured = true;
                    }
                    else
                    {
                        // Ensure the getter for the property is available as public static
                        MethodInfo getter = property.GetGetMethod();

                        if (getter == null || !(getter.IsPublic && getter.IsStatic))
                        {
                            badlyConfigured = true;
                        }
                    }

                    // If the property is not configured properly, then throw a missing member exception
                    if (badlyConfigured)
                    {
                        string exceptionMessage = String.Format("Cannot retrieve property '{0}' because localization failed.  Type '{1}' is not public or does not contain a public static string property with the name '{2}'.",
                            this.propertyName, this.resourceType.FullName, this.propertyValue);
                        this.cachedResult = () => { throw new InvalidOperationException(exceptionMessage); };
                    }
                    else
                    {
                        // We have a valid property, so cache the resource
                        this.cachedResult = () => (string)property.GetValue(null, null);
                    }
                }
            }

            // Return the cached result
            return this.cachedResult();
        }

        #endregion
    }
}