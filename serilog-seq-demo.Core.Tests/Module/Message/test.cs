using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Polly;
using Polly.Extensions.Http;

namespace serilog_seq_demo.Core.Tests.Module.Message;

[TestFixture]
public class HttpClient_Polly_Test
{
    const string TestClient = "TestClient";
    private bool _isRetryCalled;

    [Test]
    public async Task Given_A_Retry_Policy_Has_Been_Registered_For_A_HttpClient_When_The_HttpRequest_Fails_Then_The_Request_Is_Retried()
    {
        // Arrange 
        IServiceCollection services = new ServiceCollection();
        _isRetryCalled = false;

        services.AddHttpClient(TestClient)
            .AddPolicyHandler(GetRetryPolicy())
            .AddHttpMessageHandler(() => new StubDelegatingHandler());

        HttpClient configuredClient =
            services
                .BuildServiceProvider()
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(TestClient);

        // Act
        var result = await configuredClient.GetAsync("https://www.stackoverflow.com");

        // Assert
        Assert.True(_isRetryCalled);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(
                6,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetryAsync: OnRetryAsync);
    }

    private async Task OnRetryAsync(DelegateResult<HttpResponseMessage> outcome, TimeSpan timespan, int retryCount, Context context)
    {
        //Log result
        _isRetryCalled = true;
    }
}

public class StubDelegatingHandler : DelegatingHandler
{
    private int _count = 0;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_count == 0)
        {
            _count++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }
}