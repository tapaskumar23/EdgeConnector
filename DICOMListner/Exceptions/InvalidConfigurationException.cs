// --------------------------------------------------------------------------
// <copyright file="InvalidConfigurationException.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

public class InvalidConfigurationException : Exception
{
    public InvalidConfigurationException()
    {
    }

    public InvalidConfigurationException(string message)
        : base(message)
    {
    }

    public InvalidConfigurationException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}
