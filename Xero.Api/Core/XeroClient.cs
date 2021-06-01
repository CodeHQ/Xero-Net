using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Xero.Api.Core.Models;
using Xero.Api.Infrastructure.Exceptions;

namespace Xero.Api.Core
{
    public class XeroClient
    {
        private readonly IRestClient _identityClient;
        private readonly TraceSource _logger;
        public XeroOAuthToken AuthToken { get; set; }

        public XeroClient()
        {
            _logger = new TraceSource("Xero");
            _client = new RestClient("https://api.xero.com");

            var clientId = ConfigurationManager.AppSettings["XeroClientId"];
            var secret = ConfigurationManager.AppSettings["XeroSecret"];
            _identityClient = new RestClient("https://identity.xero.com/");
            var userPass = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
            _identityClient.AddDefaultHeader("authorization", $"Basic {userPass}");
        }

        public XeroClient(IRestClient client)
        {
            _client = client;
            _logger = new TraceSource("Xero");
        }
        public IRestClient _client { get; }



        public List<XeroTenant> GetTenants()
        {
            var request = new RestRequest("connections");
            request.AddHeader("Authorization", $"Bearer {AuthToken?.AccessToken}");
            var response = _client.Get<List<XeroTenant>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            throw new Exception(response.Content);
        }

        public XeroOAuthToken GetToken(string code)
        {
            var request = new RestRequest("connect/token");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", ConfigurationManager.AppSettings["XeroRedirectUrl"]);
            var response = _identityClient.Post<XeroOAuthToken>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            throw new Exception(response.Content);
        }

        public XeroOAuthToken RefreshToken()
        {
            // Refresh token
            var request = new RestRequest("connect/token");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", AuthToken.RefreshToken);
            var response = _identityClient.Post<XeroOAuthToken>(request);
            if (response.IsSuccessful)
            {
                AuthToken.AccessToken = response.Data.AccessToken;
                AuthToken.IdToken = response.Data.IdToken;
                AuthToken.RefreshToken = response.Data.RefreshToken;
                AuthToken.ExpiresIn = response.Data.ExpiresIn;

                AuthToken.Expires = DateTime.Now.AddSeconds(AuthToken.ExpiresIn);
                return AuthToken;
            }
            else
            {
                throw new Exception(response.Content);
            }
        }

        private IRestResponse Execute(RestRequest request, Guid? documentId = null)
        {
            if (AuthToken?.Expires < DateTime.UtcNow.AddMinutes(5))
            {
                RefreshToken();
            }
            request.AddHeader("Authorization", $"Bearer {AuthToken?.AccessToken}");
            //var body = request.GetBody();
            //_logger.TraceData(TraceEventType.Verbose, 0, body);
            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                _logger.TraceData(TraceEventType.Critical, 0, $"Error executing {request.Method} {request.Resource}", response.Content);
                //if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                //{
                //    // parse possible validation exception
                //    var xeroError = JsonConvert.DeserializeObject<XeroErrorRoot>(response.Content);
                //    if(xeroError != null)
                //    {
                //        var validationException = new XeroValidationError()
                //        {
                //            Body = request.GetBody(),
                //            ValidationErrors = xeroError.Elements.SelectMany(e => e.ValidationErrors).ToList()
                //        };
                //        ExceptionAlerts.Email(validationException);
                //        throw validationException;
                //    }
                //    else
                //    {
                //        ExceptionAlerts.Email(response.ErrorException);
                //        throw new Exception(response.Content);
                //    }
                //}
            }
            else
            {
                _logger.TraceData(TraceEventType.Verbose, 0, response.Content);
            }
            return response;
        }
    }
}
