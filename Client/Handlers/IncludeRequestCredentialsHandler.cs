﻿using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Client.Handlers
{
    public class IncludeRequestCredentialsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
