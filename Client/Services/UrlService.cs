﻿//-----------------------------------------------------------------------
// <copyright file="UrlService.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents the address for the server URL.</summary>
//-----------------------------------------------------------------------

namespace Client.Services
{
    /// <summary>
    /// Contains the connection for the azure service the server is hosted on and a connection for a local host.
    /// </summary>
    public class UrlService
    {
        /// <summary>
        /// Gets the lobby address.
        /// </summary>
        /// <value>
        /// The lobby address.
        /// </value>
        public string LobbyAddress => $"https://localhost:5001/game"; // https://localhost:5001/game https://tictactoefyn.azurewebsites.net/game

        /// <summary>
        /// Gets the lobby address.
        /// </summary>
        /// <value>
        /// The lobby address.
        /// </value>
        public string ApiAddress => $"https://ticserver.azurewebsites.net/api";

        public string Test { get; set; }

    }
}
