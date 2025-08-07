// See https://aka.ms/new-console-template for more information
using Azure.Core;
using FederatedTokenPOC;
using Microsoft.Identity.Client;

const string Authority = "https://login.microsoftonline.com/";
const string TenantID = "daa8799f-d4b3-4292-b5dc-e9e1f713774a";
const string ClientID = "5880de5b-6599-41d5-987f-3025abcc9f06";
const string ClientID2 = "2e3dd246-3fdf-47b6-96a5-bd6e8a1d2783";

string scope = ".default";

Console.WriteLine("Requesting Token");

var credential = new ClientAssertionCredential(ClientID2, TenantID, Authority);

var req = new TokenRequestContext(new[] { scope });

var token2 = await credential.GetTokenAsync(req);

Console.WriteLine(token2.Token);