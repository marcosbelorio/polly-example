# polly-example
Projeto implementando os patterns de retry e circuit breaker com o Polly

## ✅ Pacotes
```cmd
dotnet add package Polly
dotnet add package Polly.Extensions.Http
dotnet add package Microsoft.Extensions.Http.Polly
```

## ✅ Configuração
No arquivo Startup.cs está contido as duas policies (retry e circuit breaker)
```csharp
private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, 
    int durationOfBreakInSeconds)
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, TimeSpan.FromSeconds(durationOfBreakInSeconds));
}
```

Registrando as policies utilizando AddHttpClient() e a extensão AddPolicyHandler()
```csharp
services.AddHttpClient<IHttpBinService, HttpBinService>()
    .AddPolicyHandler(GetRetryPolicy(retryCount: 3))
    .AddPolicyHandler(GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking: 5, durationOfBreakInSeconds: 30));
```

## ✅ Como testar
Chamando o endpoint [GET]/httpbin/{code} com o valor 500 para o parâmetro **code** ele irá realizar uma requisição http para a api HttpBin e retornar status code 500, com isso será possível verificar o retry e o circuit breaker no log do console.
