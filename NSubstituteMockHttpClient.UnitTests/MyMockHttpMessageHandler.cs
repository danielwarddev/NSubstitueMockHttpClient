﻿using System.Net;
using System.Net.Http.Json;

namespace NSubstituteMockHttpClient.UnitTests;

public class MyMockHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly object? _responseContent;

    public MyMockHttpMessageHandler(HttpStatusCode statusCode, object? responseContent = null)
    {
        _statusCode = statusCode;
        _responseContent = responseContent;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = JsonContent.Create(_responseContent)
        });
    }
}
