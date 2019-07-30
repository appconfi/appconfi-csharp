# Appconfi

[Appconfi](https://www.appconfi.com) - Service to centrally manage application settings and feature toggles for applications and services.

## Installation

The Appconfi .NET SDK is available as a Nuget package, to install run the following command in the [Package Manager Console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio)
```
Install-Package Appconfi
```
More info is available on [nuget](https://www.nuget.org/packages/Appconfi/)

## Usage

In order to use the Appconfi you will need to [create an account](https://appconfi.com/account/register).

From there you can create your first application and setup your configuration. To use the Appconfi API to access your configuration go to `/accesskeys` there you can find the `application_id` and your `application_key`.

## How to use

```csharp

var manager = Configuration.NewInstance(applicationId, apiKey);

//Start monitoring changes in your application settings and features toggles.
manager.StartMonitor();

var setting = manager.GetSetting("my_good_setting");

var isFeatureEnabled = manager.IsFeatureEnabled("my_awesome_feature");

```

## Optional parameters

Change your environments:

```csharp
var env = "PRODUCTION";
var refreshInterval =  TimeSpan.FromMinutes(1);
var manager = Configuration.NewInstance(applicationId, apiKey, env, refreshInterval);
```

## Links

 * [Web](https://appconfi.com)
