// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Services.MicrosoftGraph
{
    /// <summary>
    /// Graph permission scopes.
    /// </summary>
    public static class MicrosoftGraphScope
    {
        /// <summary>
        /// Read user calendars.
        /// </summary>
        public const string CalendarsRead = "Calendars.Read";

        /// <summary>
        /// Read user and shared calendars.
        /// </summary>
        public const string CalendarsReadShare = "Calendars.Read.Shared";

        /// <summary>
        /// Have full access to user calendars.
        /// </summary>
        public const string CalendarsReadWrite = "Calendars.ReadWrite";

        /// <summary>
        /// Read and write user and shared calendars.
        /// </summary>
        public const string CalendarsReadWriteShared = "Calendars.ReadWrite.Shared";

        /// <summary>
        /// Read user contacts.
        /// </summary>
        public const string ContactsRead = "Contacts.Read";

        /// <summary>
        /// Read user and shared contacts.
        /// </summary>
        public const string ContactsReadShared = "Contacts.Read.Shared";

        /// <summary>
        /// Have full access to user contacts.
        /// </summary>
        public const string ContactsReadWrite = "Contacts.ReadWrite";

        /// <summary>
        /// Read and write user and shared contacts.
        /// </summary>
        public const string ContactsReadWriteShared = "Contacts.ReadWrite.Shared";

        /// <summary>
        /// Read user devices.
        /// </summary>
        public const string DeviceRead = "Device.Read";

        /// <summary>
        /// Read Microsoft Intune apps.
        /// </summary>
        public const string DeviceManagementAppsReadAll = "DeviceManagementApps.Read.All";

        /// <summary>
        /// Read and write Microsoft Intune apps.
        /// </summary>
        public const string DeviceManagementAppsReadWriteAll = "DeviceManagementApps.ReadWrite.All";

        /// <summary>
        /// Read Microsoft Intune device configuration and policies.
        /// </summary>
        public const string DeviceManagementConfigurationReadAll = "DeviceManagementConfiguration.Read.All";

        /// <summary>
        /// Read and write Microsoft Intune device configuration and policies.
        /// </summary>
        public const string DeviceManagementConfigurationReadWriteAll = "DeviceManagementConfiguration.ReadWrite.All";

        /// <summary>
        /// Perform user-impacting remote actions on Microsoft Intune devices.
        /// </summary>
        public const string DeviceManagementManagedDevicesPrivilegedOperationsAll = "DeviceManagementManagedDevices.PrivilegedOperations.All";

        /// <summary>
        /// Read Microsoft Intune devices.
        /// </summary>
        public const string DeviceManagementManagedDevicesReadAll = "DeviceManagementManagedDevices.Read.All";

        /// <summary>
        /// Read and write Microsoft Intune devices.
        /// </summary>
        public const string DeviceManagementManagedDevicesReadWriteAll = "DeviceManagementManagedDevices.ReadWrite.All";

        /// <summary>
        /// Read Microsoft Intune RBAC settings.
        /// </summary>
        public const string DeviceManagementRBACReadAll = "DeviceManagementRBAC.Read.All";

        /// <summary>
        /// Read and write Microsoft Intune RBAC settings.
        /// </summary>
        public const string DeviceManagementRBACReadWriteAll = "DeviceManagementRBAC.ReadWrite.All";

        /// <summary>
        /// Read Microsoft Intune configuration.
        /// </summary>
        public const string DeviceManagementServiceConfigReadAll = "DeviceManagementServiceConfig.Read.All";

        /// <summary>
        /// Read and write Microsoft Intune configuration.
        /// </summary>
        public const string DeviceManagementServiceConfigReadWriteAll = "DeviceManagementServiceConfig.ReadWrite.All";

        /// <summary>
        /// Read directory data.
        /// </summary>
        public const string DirectoryReadAll = "Directory.Read.All";

        /// <summary>
        /// Read and write directory data.
        /// </summary>
        public const string DirectoryReadWriteAll = "Directory.ReadWrite.All";

        /// <summary>
        /// Access directory as the signed-in user.
        /// </summary>
        public const string DirectoryAccessAsUserAll = "Directory.AccessAsUser.All";

        /// <summary>
        /// Read user files.
        /// </summary>
        public const string FilesRead = "Files.Read";

        /// <summary>
        /// Read all files that user can access.
        /// </summary>
        public const string FilesReadAll = "Files.Read.All";

        /// <summary>
        /// Have full access to user files.
        /// </summary>
        public const string FilesReadWrite = "Files.ReadWrite";

        /// <summary>
        /// Have full access to all files user can access.
        /// </summary>
        public const string FilesReadWriteAll = "Files.ReadWrite.All";

        /// <summary>
        /// Have full access to the application's folder (preview).
        /// </summary>
        public const string FilesReadWriteAppFolder = "Files.ReadWrite.AppFolder";

        /// <summary>
        /// Read files that the user selects.
        /// </summary>
        public const string FilesReadSelected = "Files.Read.Selected";

        /// <summary>
        /// Read and write files that the user selects.
        /// </summary>
        public const string FilesReadWriteSelected = "Files.ReadWrite.Selected";

        /// <summary>
        /// Read all groups.
        /// </summary>
        public const string GroupReadAll = "Group.Read.All";

        /// <summary>
        /// Read and write all groups.
        /// </summary>
        public const string GroupReadWriteAll = "Group.ReadWrite.All";

        /// <summary>
        /// Read identity risk event information.
        /// </summary>
        public const string IdentityRiskEventReadAll = "IdentityRiskEvent.Read.All";

        /// <summary>
        /// Read identity provider information.
        /// </summary>
        public const string IdentityProviderReadAll = "IdentityProvider.Read.All";

        /// <summary>
        /// Read and write identity provider information.
        /// </summary>
        public const string IdentityProviderReadWriteAll = "IdentityProvider.ReadWrite.All";

        /// <summary>
        /// Read user mail.
        /// </summary>
        public const string MailRead = "Mail.Read";

        /// <summary>
        /// Read and write access to user mail.
        /// </summary>
        public const string MailReadWrite = "Mail.ReadWrite";

        /// <summary>
        /// Read user and shared mail.
        /// </summary>
        public const string MailReadShared = "Mail.Read.Shared";

        /// <summary>
        /// Read and write user and shared mail.
        /// </summary>
        public const string MailReadWriteShared = "Mail.ReadWrite.Shared";

        /// <summary>
        /// Send mail as a user.
        /// </summary>
        public const string MailSend = "Mail.Send";

        /// <summary>
        /// Send mail on behalf of others.
        /// </summary>
        public const string MailSendShared = "Mail.Send.Shared";

        /// <summary>
        /// Read user mailbox settings.
        /// </summary>
        public const string MailboxSettingsRead = "MailboxSettings.Read";

        /// <summary>
        /// Read and write user mailbox settings.
        /// </summary>
        public const string MailboxSettingsReadWrite = "MailboxSettings.ReadWrite";

        /// <summary>
        /// Read hidden memberships.
        /// </summary>
        public const string MemberReadHidden = "Member.Read.Hidden";

        /// <summary>
        /// Read user OneNote notebooks.
        /// </summary>
        public const string NotesRead = "Notes.Read";

        /// <summary>
        /// Create user OneNote notebooks.
        /// </summary>
        public const string NotesCreate = "Notes.Create";

        /// <summary>
        /// Read and write user OneNote notebooks.
        /// </summary>
        public const string NotesReadWrite = "Notes.ReadWrite";

        /// <summary>
        /// Read all OneNote notebooks that user can access.
        /// </summary>
        public const string NotesReadAll = "Notes.Read.All";

        /// <summary>
        /// Read and write all OneNote notebooks that user can access.
        /// </summary>
        public const string NotesReadWriteAll = "Notes.ReadWrite.All";

        /// <summary>
        /// Limited notebook access (deprecated).
        /// </summary>
        public const string NotesReadWriteCreatedByApp = "Notes.ReadWrite.CreatedByApp";

        /// <summary>
        /// View users' email address.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// Access user's data anytime.
        /// </summary>
        public const string OfflineAccess = "offline_access";

        /// <summary>
        /// Sign users in.
        /// </summary>
        public const string OpenId = "openid";

        /// <summary>
        /// View users' basic profile.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Read users' relevant people lists.
        /// </summary>
        public const string PeopleRead = "People.Read";

        /// <summary>
        /// Read all users' relevant people lists.
        /// </summary>
        public const string PeopleReadAll = "People.Read.All";

        /// <summary>
        /// Read items in all site collections.
        /// </summary>
        public const string SitesReadAll = "Sites.Read.All";

        /// <summary>
        /// Read and write items in all site collections.
        /// </summary>
        public const string SitesReadWriteAll = "Sites.ReadWrite.All";

        /// <summary>
        /// Create, edit, and delete items and lists in all site collections.
        /// </summary>
        public const string SitesManageAll = "Sites.Manage.All";

        /// <summary>
        /// Have full control of all site collections.
        /// </summary>
        public const string SitesFullControlAll = "Sites.FullControl.All";

        /// <summary>
        /// Read user tasks.
        /// </summary>
        public const string TasksRead = "Tasks.Read";

        /// <summary>
        /// Read user and shared tasks.
        /// </summary>
        public const string TasksReadShared = "Tasks.Read.Shared";

        /// <summary>
        /// Create, read, update and delete user tasks and containers.
        /// </summary>
        public const string TasksReadWrite = "Tasks.ReadWrite";

        /// <summary>
        /// Read and write user and shared tasks.
        /// </summary>
        public const string TasksReadWriteShared = "Tasks.ReadWrite.Shared";

        /// <summary>
        /// Sign-in and read user profile.
        /// </summary>
        public const string UserRead = "User.Read";

        /// <summary>
        /// Read and write access to user profile.
        /// </summary>
        public const string UserReadWrite = "User.ReadWrite";

        /// <summary>
        /// Read all users' basic profiles.
        /// </summary>
        public const string UserReadBasicAll = "User.ReadBasic.All";

        /// <summary>
        /// Read all users' full profiles.
        /// </summary>
        public const string UserReadAll = "User.Read.All";

        /// <summary>
        /// Read and write all users' full profiles.
        /// </summary>
        public const string UserReadWriteAll = "User.ReadWrite.All";

        /// <summary>
        /// Invite guest users to the organization.
        /// </summary>
        public const string UserInviteAll = "User.Invite.All";
    }
}
