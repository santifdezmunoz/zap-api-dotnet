/* Zed Attack Proxy (ZAP) and its related class files.
 *
 * ZAP is an HTTP/HTTPS proxy for assessing web application security.
 *
 * Copyright the ZAP development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace OWASPZAPDotNetAPI
{
    class SystemWebClient : IWebClient, IDisposable
    {
        HttpClient httpClient;
        WebProxy webProxy;

        public SystemWebClient(string proxyHost, int proxyPort)
        {
            webProxy = new WebProxy(proxyHost, proxyPort);
            httpClient = new HttpClient(new HttpClientHandler() { Proxy = webProxy, UseProxy = true });
        }

        public string DownloadString(string address)
        {
            return httpClient.GetStringAsync(address).Result;
        }

        public string DownloadString(Uri uri)
        {
            string retVal = string.Empty;
            try
            {
                retVal = httpClient.GetStringAsync(uri).Result;
            }
            catch (WebException webException)
            {
                var responseStream = webException.Response?.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        retVal = reader.ReadToEnd();
                    }
                }
            }

            return retVal;
        }

        public byte[] DownloadData(Uri uri)
        {
            byte[] retVal = default(byte[]);
            try
            {
                retVal = httpClient.GetByteArrayAsync(uri).Result;
            }
            catch (WebException)
            {
                throw;
            }
            return retVal;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public void AddRequestHeader(string headerName, string headerValue)
        {
            httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
        }

        public string GetRequestHeaderValue(string headerName)
        {
            return httpClient.DefaultRequestHeaders.GetValues(headerName).ToString();
        }

        public void SetRequestHeader(string headerName, string headerValue)
        {
            if (httpClient.DefaultRequestHeaders.Contains(headerName))
            {
                httpClient.DefaultRequestHeaders.Remove(headerName);
            }
            httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
        }
    }
}
