﻿/* Zed Attack Proxy (ZAP) and its related class files.
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


using OWASPZAPDotNetAPI.Generated;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OWASPZAPDotNetAPI
{
    public sealed class ClientApi : IDisposable
    {
        private IWebClient _webClient;

        private readonly string _apiDomain = "zap";
        private readonly int _apiPort = 80;

        private string _apiKey;
        private string _format = "xml";
        private string _otherFormat = "other";
        private string _zapApiKeyHeaderName = "X-ZAP-API-Key";
        private static string _zapApiKeyParameterName = "apikey";

        //New API needs to be added here, preferably in alphabetical order
        public AccessControl accesscontrol;
        public Acsrf acsrf;
        public AjaxSpider ajaxspider;
        public OWASPZAPDotNetAPI.Generated.Alert alert;
        public AlertFilter alertFilter;
        public Ascan ascan;
        public Authentication authentication;
        public Authorization authorization;
        public Automation automation;
        public Autoupdate autoupdate;
        public Break brk;
        public Context context;
        public Core core;
        public Exim exim;
        public ForcedUser forcedUser;
        public Graphql graphql;
        public HttpSessions httpSessions;
        public Network network;
        public ImportLogFiles importLogFiles;
        public Importurls importurls;
        public LocalProxies localProxies;
        public Openapi openapi;
        public Params parameters;
        public Pnh pnh;
        public Pscan pscan;
        public Replacer replacer;
        public Reports reports;
        public Retest retest;
        public Reveal reveal;
        public Revisit revisit;
        public RuleConfig ruleConfig;
        public Script script;
        public Search search;
        public Selenium selenium;
        public SessionManagement sessionManagement;
        public Soap soap;
        public Spider spider;
        public Stats stats;
        public Users users;
        public Wappalyzer wappalyzer;
        public Websocket websocket;

        public ClientApi(string zapAddress, int zapPort, string apiKey)
        {
            this._apiKey = apiKey;

            if (zapAddress != null && _apiDomain != zapAddress)
            {
                _apiDomain = zapAddress;
            }
            
            if (zapPort != 0 && _apiPort != zapPort)
            {
                _apiPort = zapPort;
            }
            
            _webClient = new SystemWebClient(zapAddress, zapPort);

            InitializeApiObjects();
        }
        
        private void InitializeApiObjects()
        {
            //New API needs to be instantiated here, in the same alphabetical order as above
            accesscontrol = new AccessControl(this);
            acsrf = new Acsrf(this);
            ajaxspider = new AjaxSpider(this);
            alert = new OWASPZAPDotNetAPI.Generated.Alert(this);
            alertFilter = new AlertFilter(this);
            ascan = new Ascan(this);
            authentication = new Authentication(this);
            authorization = new Authorization(this);
            automation = new Automation(this);
            autoupdate = new Autoupdate(this);
            brk = new Break(this);
            context = new Context(this);
            core = new Core(this);
            exim = new Exim(this);
            forcedUser = new ForcedUser(this);
            graphql = new Graphql(this);
            httpSessions = new HttpSessions(this);
            network = new Network(this);
            importLogFiles = new ImportLogFiles(this);
            importurls = new Importurls(this);
            localProxies = new LocalProxies(this);
            openapi = new Openapi(this);
            parameters = new Params(this);
            pnh = new Pnh(this);
            pscan = new Pscan(this);
            replacer = new Replacer(this);
            reports = new Reports(this);
            retest = new Retest(this);
            reveal = new Reveal(this);
            revisit = new Revisit(this);
            ruleConfig = new RuleConfig(this);
            script = new Script(this);
            search = new Search(this);
            selenium = new Selenium(this);
            sessionManagement = new SessionManagement(this);
            soap = new Soap(this);
            spider = new Spider(this);
            stats = new Stats(this);
            users = new Users(this);
            wappalyzer = new Wappalyzer(this);
            websocket = new Websocket(this);
        }

        public void AccessUrl(string url)
        {
            _ = _webClient.DownloadString(url);
        }

        public List<Alert> GetAlerts(string baseUrl, int start, int count, string riskId)
        {
            List<Alert> alerts = new List<Alert>();
            IApiResponse response = alert.alerts(baseUrl, Convert.ToString(start), Convert.ToString(count), riskId);
            if (response != null && response is ApiResponseList)
            {
                ApiResponseList apiResponseList = (ApiResponseList)response;
                foreach (var alertSet in apiResponseList.List)
                {
                    ApiResponseSet apiResponseSet = (ApiResponseSet)alertSet;
                    alerts.Add(GetNewAlertFromAResponseSet(apiResponseSet));
                }
            }
            return alerts;
        }

        private static Alert GetNewAlertFromAResponseSet(ApiResponseSet apiResponseSet)
        {
            return new Alert(apiResponseSet.Dictionary.TryGetDictionaryString("alert"), apiResponseSet.Dictionary.TryGetDictionaryString("url"))
            {
                Attack = apiResponseSet.Dictionary.TryGetDictionaryString("attack"),
                Confidence = string.IsNullOrWhiteSpace(apiResponseSet.Dictionary.TryGetDictionaryString("confidence")) ?
                    Alert.ConfidenceLevel.Low :
                    (Alert.ConfidenceLevel)Enum.Parse(typeof(Alert.ConfidenceLevel), apiResponseSet.Dictionary.TryGetDictionaryString("confidence")),
                CWEId = int.Parse(apiResponseSet.Dictionary.TryGetDictionaryString("cweid")),
                Description = apiResponseSet.Dictionary.TryGetDictionaryString("description"),
                Evidence = apiResponseSet.Dictionary.TryGetDictionaryString("evidence"),
                Other = apiResponseSet.Dictionary.TryGetDictionaryString("other"),
                Parameter = apiResponseSet.Dictionary.TryGetDictionaryString("param"),
                Reference = apiResponseSet.Dictionary.TryGetDictionaryString("reference"),
                Risk = string.IsNullOrWhiteSpace(apiResponseSet.Dictionary.TryGetDictionaryString("risk")) ?
                    Alert.RiskLevel.Low :
                    (Alert.RiskLevel)Enum.Parse(typeof(Alert.RiskLevel), apiResponseSet.Dictionary.TryGetDictionaryString("risk")),
                Solution = apiResponseSet.Dictionary.TryGetDictionaryString("solution"),
                WASCId = int.Parse(apiResponseSet.Dictionary.TryGetDictionaryString("wascid"))
            };

        }

        public IApiResponse CallApi(string component, string operationType, string operationName, Dictionary<string, string> parameters)
        {
            XmlDocument xmlDocument = this.CallApiRaw(component, operationType, operationName, parameters);
            return ApiResponseFactory.GetResponse(xmlDocument.ChildNodes[1]);
        }

        private XmlDocument CallApiRaw(string component, string operationType, string operationName, Dictionary<string, string> parameters)
        {
            Uri requestUrl = PrepareZapRequest(this._format, component, operationType, operationName, parameters);
            string responseString = _webClient.DownloadString(requestUrl);
            XmlDocument responseXmlDocument = new XmlDocument();
            responseXmlDocument.LoadXml(responseString);
            return responseXmlDocument;
        }

        public byte[] CallApiOther(string component, string operationType, string operationName, Dictionary<string, string> parameters)
        {
            Uri requestUrl = PrepareZapRequest(this._otherFormat, component, operationType, operationName, parameters);
            byte[] response = _webClient.DownloadData(requestUrl);
            return response;
        }

        private Uri PrepareZapRequest(string format, string component, string operationType, string operationName, Dictionary<string, string> parameters)
        {
            Uri requestUrl = BuildZapRequestUrl(this._apiKey, format, component, operationType, operationName, parameters);
            string apiKeyValueFromRequestHeader = _webClient.GetRequestHeaderValue(this._zapApiKeyHeaderName);
            if (String.IsNullOrWhiteSpace(apiKeyValueFromRequestHeader))
            {
                _webClient.AddRequestHeader(this._zapApiKeyHeaderName, this._apiKey);
            }
            return requestUrl;
        }

        private Uri BuildZapRequestUrl(string apikey, string format, string component, string operationType, string operationName, Dictionary<string, string> parameters)
        {
            UriBuilder uriBuilder = new UriBuilder();

            uriBuilder.Scheme = "http";
            uriBuilder.Host = _apiDomain;
            uriBuilder.Port = _apiPort;

            uriBuilder.Path = new StringBuilder()
                                    .Append(format)
                                    .Append("/")
                                    .Append(component)
                                    .Append("/")
                                    .Append(operationType)
                                    .Append("/")
                                    .Append(operationName)
                                    .ToString();

            StringBuilder query = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Value != null)
                    {
                        query.Append(Uri.EscapeDataString(parameter.Key));
                        query.Append("=");
                        query.Append(Uri.EscapeDataString(parameter.Value));
                        query.Append("&");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(apikey))
            {
                query.Append(Uri.EscapeDataString(_zapApiKeyParameterName));
                query.Append("=");
                query.Append(Uri.EscapeDataString(apikey));
                query.Append("&");
            }


            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        public void Dispose()
        {
            ((IDisposable)_webClient).Dispose();
        }
    }
}
