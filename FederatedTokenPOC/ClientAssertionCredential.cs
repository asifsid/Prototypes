namespace FederatedTokenPOC
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;
    using Azure.Core;
    using System.Threading;
    using System.Net;
    using System.IO;
    using System.Net.Http.Json;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using System.Net.Http.Headers;

    public class ClientAssertionCredential : TokenCredential
    {
        private readonly string clientID;
        private readonly string tenantID;
        private readonly string aadAuthority;

        public ClientAssertionCredential(string clientID, string tenantID, string aadAuthority)
        {
            this.clientID = clientID;
            this.tenantID = tenantID;
            this.aadAuthority = aadAuthority;  // https://login.microsoftonline.com/                
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {

            return GetTokenImplAsync(false, requestContext, cancellationToken).GetAwaiter().GetResult();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            return await GetTokenImplAsync(true, requestContext, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            const string TenantID = "72f988bf-86f1-41af-91ab-2d7cd011db47";
            const string ClientID = "c8bb859c-3c34-417c-a3bd-e0ccca6b099f";
            const string ClientSecret = "***************************";
            const string scope = "0cdb527f-a8d1-4bf8-9436-b352c68682b2/.default";
            const string secSvcUrl = "https://gw.us-il100.gateway.test.island.powerapps.com/securityservice/v2/token";

            var app = ConfidentialClientApplicationBuilder.Create(ClientID)
                .WithTenantId(TenantID)
                .WithClientSecret(ClientSecret)
                .Build();

            var token = await app.AcquireTokenForClient(new[] { scope }).ExecuteAsync();
            var req = new HttpRequestMessage(HttpMethod.Post, secSvcUrl);
            //req.Headers.Add("x-ms-use-igw-issuer", string.Empty);
            
            Console.WriteLine(token.AccessToken);
            Console.WriteLine();

            req.Content = new StringContent(
                @"{  
                    grant_type: ""client_credentials"", 
                    client_assertion_type: ""aad_app"",
                    client_assertion : """ + token.AccessToken + @""",
                    context_type: ""common"",
                    scope: ""https://azureadtokenexchange.dev-operations365.dynamics.com/.default""
                  }", MediaTypeHeaderValue.Parse("application/json"));

            var response = await new HttpClient().SendAsync(req);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var idToken = json.RootElement.GetProperty("access_token").GetString();

            Console.WriteLine(idToken);
            Console.WriteLine();

            try
            {
                // pass token as a client assertion to the confidential client app
                app = ConfidentialClientApplicationBuilder.Create(this.clientID)
                    .WithClientAssertion(idToken)
                    .Build();

                var authResult = app.AcquireTokenForClient(requestContext.Scopes)
                    .WithAuthority(this.aadAuthority + this.tenantID)
                    .ExecuteAsync();

                AccessToken xtoken = new AccessToken(authResult.Result.AccessToken, authResult.Result.ExpiresOn);

                return xtoken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}