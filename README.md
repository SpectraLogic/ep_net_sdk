# Spectra Rio Broker .NET SDK

[![Build Status](https://travis-ci.org/SpectraLogic/ep_net_sdk.svg)](https://travis-ci.org/SpectraLogic/ep_net_sdk)
[![Apache V2 License](http://img.shields.io/badge/license-Apache%20V2-blue.svg)](https://github.com/SpectraLogic/ep_net_sdk/blob/master/LICENSE.md)

An [SDK](http://en.wikipedia.org/wiki/Software_development_kit) conforming to
the Spectra Rio Broker specification.

## Contact Us

Join us at our [Google Groups](https://groups.google.com/d/forum/spectralogicds3-sdks) forum to ask questions, or see frequently asked questions.

## Setting up NuGet

The SDK is distributed as a [NuGet](http://www.nuget.org) package for .Net 4.5.2
and above. From the NuGet website:

> *What is NuGet?*

> NuGet is the package manager for the Microsoft development platform including
> .NET. The NuGet client tools provide the ability to produce and consume
> packages. The NuGet Gallery is the central package repository used by all
> package authors and consumers.

While the Spectra Rio Broker SDK is not yet in the [NuGet
Gallery](http://www.nuget.org/packages), you can easily create a package feed
on your computer using the latest release:

1. Download the .nupkg file from the [Releases](../../releases) page to a new
   directory of your choice.
2. Follow the NuGet instructions on [Creating Local Feeds](http://docs.nuget.org/docs/creating-packages/hosting-your-own-nuget-feeds#Creating_Local_Feeds)
   using the directory that you've created.
   
This makes the Spectra Rio Broker SDK available for installation into a Visual Studio .NET
Project.

## Installing the Spectra Rio Broker SDK Into Your Own Project

1. Open your existing .NET project or create a new one.
2. Right-click the project and click "Manage NuGet Packages..."
3. Choose the package source you created for the NuGet package.
4. Click "Browse" on the left panel.
5. In the search box on the upper right, type "SpectraRioBroker".
6. Click the "Install" button on the right and close the
   package manager dialog.

Your project should now reference the SDK and be able to use its API.

## About the API

SDK documentation can be found here [Documentation](http://spectralogic.github.io/ep_net_sdk)

## Instantiating the API

The example below shows how to configure and instantiate `ISpectraRioBrokerClient`

```csharp
using SpectraLogic.SpectraRioBrokerClient;

namespace application
{
    class Class1
    {
        public ISpectraRioBrokerClient CreateClient()
        {
            var spectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder("localhost", 5050);
            return spectraRioBrokerClientBuilder.DisableSslValidation().Build();
        }
    }
}
```

The example below shows how to use `ISpectraRioBrokerClient` and send a request to Spectra Rio Broker Server (ex. CreateToken)

```csharp
using SpectraLogic.SpectraRioBrokerClient;
using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace your.application
{
    class Class1
    {
        public ISpectraRioBrokerClient CreateClient()
        {
            var spectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder("localhost", 5050);
            return spectraRioBrokerClientBuilder.DisableSslValidation().Build();
        }

        public IToken CreateToken()
        {
            var client = CreateClient();

            var request = new CreateTokenRequest("username", "password");
            return client.CreateToken(request);
        }
    }
}
```
For more examples and uses of the SDK [SpectraLogic.SpectraRioBrokerClient.Integration.Test](../../tree/master/SpectraLogic.SpectraRioBrokerClient.Integration.Test)


## Updating the max concurrent connections allowed
By default there are 2 max concurrent connections allowed and there are two ways to override it. Typically 2 max concurrent connections will be much too low and will limit RioBroker performance. We therefore recommend that for most environments, 10-15 max concurrent connections would be a better value.
1. Update the App.config to include the following:
```xml
<configuration>  
  <system.net>  
    <connectionManagement>  
      <add address = "*" maxconnection = "10" />  
    </connectionManagement>  
  </system.net>  
</configuration>
```
2. Set the `DefaultConnectionLimit` on the `ServicePointManager` to your preferred value once when the AppDomain loads for example `System.Net.ServicePointManager.DefaultConnectionLimit=10`
