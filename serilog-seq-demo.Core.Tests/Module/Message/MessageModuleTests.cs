// using System;
// using System.Net;
// using System.Net.Http;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;
// using NUnit.Framework;
// using Polly;
// using Polly.Extensions.Http;
//
// namespace serilog_seq_demo.Core.Tests.Module.Message;
//
// public class Tests
// {
//     const string TestClient = "TestClient";
//     private static bool _isOnBreak;
//
//
//     [Test]
//     public async Task Test1()
//     {
//         // Arrange 
//         IServiceCollection services = new ServiceCollection();
//         _isOnBreak = false;
//
//         services.AddHttpClient(TestClient)
//             .AddPolicyHandler(GetRetryPolicy())
//             .AddHttpMessageHandler(() => new StubDelegatingHandler());
//
//         HttpClient configuredClient =
//             services
//                 .BuildServiceProvider()
//                 .GetRequiredService<IHttpClientFactory>()
//                 .CreateClient(TestClient);
//
//         // Act
//         var result = await configuredClient.GetAsync("https://www.stackoverflow.com");
//
//         // Assert
//         Assert.True(_isOnBreak);
//         Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
//     }
//
//     private class StubDelegatingHandler : DelegatingHandler
//     {
//         private int _count = 0;
//
//         protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//         {
//             if (_count != 0) return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
//
//             _count++;
//             return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
//         }
//     }
//
//
//     private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
//     {
//         return HttpPolicyExtensions
//             .HandleTransientHttpError()
//             .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
//             .CircuitBreakerAsync(1, TimeSpan.FromSeconds(30),
//                 (Action<DelegateResult<HttpResponseMessage>, TimeSpan, Context>)((e, t, c) =>
//                 {
//                     _isOnBreak = true;
//                     Console.WriteLine("過載保護");
//                 }),
//                 c =>
//                 {
//                     Console.WriteLine("恢復正常");
//                     _isOnBreak = false;
//                 });
//     }
// }