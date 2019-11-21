﻿/*
 * ******************************************************************************
 *   Copyright 2014-2019 Spectra Logic Corporation. All Rights Reserved.
 *   Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *   this file except in compliance with the License. A copy of the License is located at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 *   or in the "license" file accompanying this file.
 *   This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *   CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *   specific language governing permissions and limitations under the License.
 * ****************************************************************************
 */

using log4net;
using SpectraLogic.SpectraRioBrokerClient.Calls;

#if DEBUG
using SpectraLogic.SpectraRioBrokerClient.Utils;
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace SpectraLogic.SpectraRioBrokerClient.Runtime
{
    internal class Network : INetwork
    {
        #region Private Fields

        private static readonly ILog Log = LogManager.GetLogger("Network");

        #endregion Private Fields

        #region Public Constructors

        public Network(string hostServerName, int hostServerPort, string token, bool disableSslValidation, Uri proxy = null, string userAgent = null)
        {
            HostServerName = hostServerName;
            HostServerPort = hostServerPort;
            Token = token;
            Proxy = proxy;
            DisableSslValidation = disableSslValidation;
            UserAgent = userAgent;
        }

        #endregion Public Constructors

        #region Private Properties

        private bool DisableSslValidation { get; set; }
        private string HostServerName { get; }
        private int HostServerPort { get; }
        private Uri Proxy { get; }
        private string Token { get; set; }
        private string UserAgent { get; }

        #endregion Private Properties

        #region Public Methods

        public IHttpWebResponse Invoke(RestRequest request)
        {
            if (DisableSslValidation)
            {
                ServerCertificateValidation.OverrideValidation();
            }

            var httpWebRequest = CreateHttpWebRequest(request);

            Log.Debug(GetRequestPrettyPrint(request, httpWebRequest));

            try
            {
                return httpWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw;
                }
                return new SpectraRioBrokerHttpWebResponse((HttpWebResponse)e.Response);
            }
        }

        public void UpdateToken(string token) => Token = token;

        #endregion Public Methods

        #region Private Methods

        private static string BuildQueryParams(Dictionary<string, string> queryParams)
        {
            return string.Join(
                "&",
                from kvp in queryParams
                orderby kvp.Key
                let encodedKey = Uri.EscapeDataString(kvp.Key)
                select !string.IsNullOrEmpty(kvp.Value)
                    ? encodedKey + "=" + Uri.EscapeDataString(kvp.Value)
                    : encodedKey
            );
        }

        private static string GetRequestPrettyPrint(RestRequest request, IHttpWebRequest httpWebRequest)
        {
            var requestPrettyPrint = httpWebRequest.ToString();
#if DEBUG
            if (request.Verb == HttpVerb.POST || request.Verb == HttpVerb.PUT)
            {
                requestPrettyPrint = string.Concat(requestPrettyPrint, string.Concat(Environment.NewLine, request.GetBody().JsonFormat()));
            }
#endif
            return requestPrettyPrint;
        }

        private static string GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        private IHttpWebRequest CreateHttpWebRequest(RestRequest request)
        {
            var uriBuilder = new UriBuilder(HostServerName)
            {
                Path = request.Path,
                Query = BuildQueryParams(request.GetQueryParams()),
                Port = HostServerPort
            };

            var httpRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            httpRequest.Method = request.Verb.ToString();
            if (Token != null)
            {
                httpRequest.Headers.Add("Authorization", GetBearerAuth());
            }
            httpRequest.ContentType = "application/json";

            if (Proxy != null)
            {
                var webProxy = new WebProxy
                {
                    Address = Proxy
                };
                httpRequest.Proxy = webProxy;
            }

            httpRequest.Date = DateTime.UtcNow;

            httpRequest.UserAgent = UserAgent ?? $"Spectra Rio Broker Client v{GetVersion()}";

            if (request.Verb == HttpVerb.PUT || request.Verb == HttpVerb.POST)
            {
                var body = Encoding.UTF8.GetBytes(request.GetBody());
                httpRequest.ContentLength = body.Length;

                using (var requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                }
            }

            return new SpectraRioBrokerHttpWebRequest(httpRequest);
        }

        private string GetBearerAuth() => string.Concat("Bearer ", Token);

        #endregion Private Methods
    }
}