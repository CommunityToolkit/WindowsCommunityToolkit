// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// User's Fields
    /// </summary>
    public enum MicrosoftGraphUserFields
    {
        /// <summary>
        /// A freeform text entry field for the user to describe themselves.
        /// </summary>
        AboutMe,

        /// <summary>
        /// true if the account is enabled; otherwise, false. This property is required when a user is created.
        /// </summary>
        AccountEnabled,

        /// <summary>
        /// The licenses that are assigned to the user. Not nullable.
        /// </summary>
        AssignedLicenses,

        /// <summary>
        /// The plans that are assigned to the user. Read-only. Not nullable.
        /// </summary>
        AssignedPlans,

        /// <summary>
        /// The birthday of the user. The Timestamp type represents date and time information using ISO 8601 format and is always in UTC time. For example, midnight UTC on Jan 1, 2014 would look like this: '2014-01-01T00:00:00Z'
        /// </summary>
        Birthday,

        /// <summary>
        ///  The business phones
        /// </summary>
        BusinessPhones,

        /// <summary>
        /// The city in which the user is located.
        /// </summary>
        City,

        /// <summary>
        /// Company name.
        /// </summary>
        CompanyName,

        /// <summary>
        /// The country/region in which the user is located; for example, “US” or “UK”.
        /// </summary>
        Country,

        /// <summary>
        /// The name for the department in which the user works.
        /// </summary>
        Department,

        /// <summary>
        /// The name displayed in the address book for the user. This is usually the combination of the user's first name, middle initial and last name.
        /// </summary>
        DisplayName,

        /// <summary>
        /// The given name (first name) of the user
        /// </summary>
        GivenName,

        /// <summary>
        /// The hire date of the user. The Timestamp type represents date and time information using ISO 8601 format and is always in UTC time. For example, midnight UTC on Jan 1, 2014 would look like this: '2014-01-01T00:00:00Z'
        /// </summary>
        HireDate,

        /// <summary>
        /// Id of the uses
        /// </summary>
        Id,

        /// <summary>
        /// A list for the user to describe their interests.
        /// </summary>
        Interests,

        /// <summary>
        /// The user’s job title.
        /// </summary>
        JobTitle,

        /// <summary>
        /// The SMTP address for the user, for example, 'jeff@contoso.onmicrosoft.com'. Read-Only
        /// </summary>
        Mail,

        /// <summary>
        /// The mail alias for the user. This property must be specified when a user is created.
        /// </summary>
        MailNickname,

        /// <summary>
        /// The primary cellular telephone number for the user.
        /// </summary>
        MobilePhone,

        /// <summary>
        /// The URL for the user's personal site.
        /// </summary>
        MySite,

        /// <summary>
        /// The office location in the user's place of business.
        /// </summary>
        OfficeLocation,

        /// <summary>
        /// This property is used to associate an on-premises Active Directory user account to their Azure AD user object. This property must be specified when creating a new user account in the Graph if you are using a federated domain for the user’s userPrincipalName (UPN) property. Important: The $ and **_** characters cannot be used when specifying this property.
        /// </summary>
        OnPremisesImmutableId,

        /// <summary>
        /// Indicates the last time at which the object was synced with the on-premises directory; for example: '2013-02-16T03:04:54Z'. The Timestamp type represents date and time information using ISO 8601 format and is always in UTC time. For example, midnight UTC on Jan 1, 2014 would look like this: '2014-01-01T00:00:00Z'. Read-only.
        /// </summary>
        OnPremisesLastSyncDateTime,

        /// <summary>
        /// Contains the on-premises security identifier (SID) for the user that was synchronized from on-premises to the cloud. Read-only.
        /// </summary>
        OnPremisesSecurityIdentifier,

        /// <summary>
        /// true if this object is synced from an on-premises directory; false if this object was originally synced from an on-premises directory but is no longer synced; null if this object has never been synced from an on-premises directory (default). Read-only
        /// </summary>
        OnPremisesSyncEnabled,

        /// <summary>
        /// Specifies password policies for the user. This value is an enumeration with one possible value being “DisableStrongPassword”, which allows weaker passwords than the default policy to be specified. “DisablePasswordExpiration” can also be specified. The two may be specified together; for example: 'DisablePasswordExpiration, DisableStrongPassword'.
        /// </summary>
        PasswordPolicies,

        /// <summary>
        /// Specifies the password profile for the user. The profile contains the user’s password. This property is required when a user is created. The password in the profile must satisfy minimum requirements as specified by the passwordPolicies property. By default, a strong password is required.
        /// </summary>
        PasswordProfile,

        /// <summary>
        /// A list for the user to enumerate their past projects.
        /// </summary>
        PastProjects,

        /// <summary>
        /// The postal code for the user's postal address. The postal code is specific to the user's country/region. In the United States of America, this attribute contains the ZIP code.
        /// </summary>
        PostalCode,

        /// <summary>
        /// The preferred language for the user. Should follow ISO 639-1 Code; for example 'en-US'.
        /// </summary>
        PreferredLanguage,

        /// <summary>
        /// The preferred name for the user.
        /// </summary>
        PreferredName,

        /// <summary>
        /// The plans that are provisioned for the user. Read-only. Not nullable.
        /// </summary>
        ProvisionedPlans,

        /// <summary>
        /// A list for the user to enumerate their responsibilities.
        /// </summary>
        Responsibilities,

        /// <summary>
        /// A list for the user to enumerate the schools they have attended.
        /// </summary>
        Schools,

        /// <summary>
        /// A list for the user to enumerate their skills.
        /// </summary>
        Skills,

        /// <summary>
        /// The state or province in the user's address.
        /// </summary>
        State,

        /// <summary>
        /// The street address of the user's place of business.
        /// </summary>
        StreetAddress,

        /// <summary>
        /// The user's surname (family name or last name).
        /// </summary>
        Surname,

        /// <summary>
        /// A two letter country code (ISO standard 3166). Required for users that will be assigned licenses due to legal requirement to check for availability of services in countries.  Examples include: 'US', 'FR', and 'GB'.
        /// </summary>
        UsageLocation,

        /// <summary>
        /// The user principal name (UPN) of the user. The UPN is an Internet-style login name for the user based on the Internet standard RFC 822. By convention, this should map to the user's email name. The general format is alias@domain, where domain must be present in the tenant’s collection of verified domains. This property is required when a user is created. The verified domains for the tenant can be accessed from the verifiedDomains property of organization.
        /// </summary>
        UserPrincipalName,

        /// <summary>
        /// A string value that can be used to classify user types in your directory, such as “Member” and “Guest”.
        /// </summary>
        UserType
    }
}
