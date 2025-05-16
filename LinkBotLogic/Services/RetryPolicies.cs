using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBotLogic.Services
{
    using Polly;
    using Polly.Retry;
    using OpenQA.Selenium;
    using System;

    public static class RetryPolicies
    {
        public static readonly RetryPolicy SeleniumRetryPolicy = Policy
            .Handle<WebDriverException>()
            .Or<NoSuchElementException>()
            .Or<StaleElementReferenceException>()
            .Or<ElementNotInteractableException>()
            .Or<TimeoutException>()
            .WaitAndRetry(
        retryCount: 3,
        sleepDurationProvider: attempt =>
            TimeSpan.FromSeconds(3), 
        onRetry: (exception, timespan, attempt, _) =>
        {
            Console.WriteLine($"Retry {attempt} after {timespan.TotalMilliseconds}ms due to {exception.GetType().Name}");
        });
        public static AsyncRetryPolicy SeleniumAsyncRetryPolicy => Policy
            .Handle<WebDriverException>()
            .Or<NoSuchElementException>()
            .Or<StaleElementReferenceException>()
            .Or<ElementNotInteractableException>()
            .Or<TimeoutException>() 
            .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt =>
            TimeSpan.FromSeconds(5), 
        onRetry: (exception, timespan, attempt, _) =>
        {
            Console.WriteLine($"Attempt {attempt}: Retrying in {timespan.TotalSeconds}s");
            Console.WriteLine($"Error: {exception.GetType().Name}");
        });

    }
}
