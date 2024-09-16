﻿// --------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Health.DICOM.Listener.Configuration;
using Microsoft.Health.DICOM.Listener.Endpoints;
using Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage;
using Microsoft.Health.DICOM.Listener.Listener;
using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener.Extensions
{
    internal static class ServiceCollectionExtensions
    {     

        internal static IServiceCollection AddLocalStorageEndpoint(this IServiceCollection services, IConfiguration configuration, ref int endpointCount)
        {
            var localStorageEndpointConfiguration = new LocalStorageEndpointConfiguration();
            configuration.Bind(nameof(LocalStorageEndpointConfiguration), localStorageEndpointConfiguration);

            if (localStorageEndpointConfiguration.IsValid())
            {
                if (!Directory.Exists(localStorageEndpointConfiguration.Path))
                {
                    throw new InvalidConfigurationException("The path provided for the local storage endpoint does not exist.");
                }

                services.AddOptions<LocalStorageEndpointConfiguration>()
                .Bind(configuration.GetSection(nameof(LocalStorageEndpointConfiguration)))
                .Validate(configuration => configuration.IsValid())
                .ValidateOnStart();

                services.AddSingleton<IExternalEndpoint<LocalStorageMessageContext>, LocalStorageEndpoint>();
                services.AddSingleton<IMessageContextFactory<LocalStorageMessageContext>, LocalStorageMessageContextFactory>();
                services.AddHostedService<Worker<LocalStorageMessageContext>>();
                services.AddSingleton<IMessageListener<LocalStorageMessageContext>, MessageListener<LocalStorageMessageContext>>();

                endpointCount++;
            }

            return services;
        }
    }
}
