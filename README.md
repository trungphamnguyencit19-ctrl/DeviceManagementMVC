# Device Management MVC

ASP.NET Core MVC application for Practice Assignment 2. Uses SQLite so no separate database server is required.

## Requirements
- .NET 8 SDK (or a newer SDK that can target .NET 8)
- Internet access on first run to restore the EF Core SQLite NuGet package and load Bootstrap/jQuery CDN assets.

## Run in Visual Studio
1. Extract the ZIP.
2. Open `DeviceManagementMVC.csproj` in Visual Studio.
3. Allow NuGet restore to finish.
4. Set `DeviceManagementMVC` as the startup project if prompted.
5. Press F5 or Ctrl+F5.
6. The database `devices.db` is created automatically in the application working directory.

If your installed .NET SDK does not support .NET 8, install the .NET 8 SDK or change `TargetFramework` and the EF Core package to a compatible version.

## Features
- Device category CRUD, duplicate-name validation, prevents deleting categories in use.
- Device CRUD with unique code validation, category, status and date of entry.
- User CRUD with required fields, email and phone validation, duplicate-email validation.
- Device search by name/code and filtering by category/status.
- MVC separation into Models, Views, Controllers and Data.

## Suggested demo
1. Create categories: Computer, Printer, Phone.
2. Add devices with different statuses.
3. Search by device name/code and try both filters.
4. Add, edit and delete a user.
5. Try deleting a category that still has devices to see the validation message.

## Notes
This is a coursework/demo app, not a production identity system. User records represent employee contact information; the application does not implement login/authentication.
