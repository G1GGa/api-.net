using System.Net.Http;
using PhishingDetectorApi.Models;

namespace PhishingDetectorApi.Services;

public class PhishingCheckerService
{
    private readonly HttpClient _httpClient;

    public PhishingCheckerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // ვუწესებთ ტაიმაუტს, რომ დიდხანს არ გაიჭედოს
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<CheckResponse> AnalyzeUrlAsync(string targetUrl)
    {
        // 1. საბაზისო ვალიდაცია (სწორი ფორმატი)
        if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out var uri))
        {
            return new CheckResponse
            {
                IsSafe = false,
                RiskScore = 100,
                Reason = "Invalid URL format."
            };
        }

        int riskScore = 0;
        var reasons = new List<string>();

        // 2. HTTPS შემოწმება (მილიწამები)
        if (uri.Scheme != Uri.UriSchemeHttps)
        {
            riskScore += 35;
            reasons.Add("Not using secure HTTPS protocol.");
        }

        // 3. მსუბუქი HTTP (HEAD/GET) მოთხოვნა რეალურ დროში (სწრაფი ტესტი)
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Head, targetUrl);
            // ვურტყამთ იუზერ-აგენტს, რომ ბოტად არ გვაგდებდეს
            requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                riskScore += 30;
                reasons.Add($"Server responded with status code: {(int)response.StatusCode}.");
            }
        }
        catch (TaskCanceledException)
        {
            riskScore += 40;
            reasons.Add("Request timed out. The website might be down or blocking automated checks.");
        }
        catch (Exception ex)
        {
            riskScore += 50;
            reasons.Add($"Could not connect to the website: {ex.Message}");
        }

        // საბოლოო გადაწყვეტილება
        bool isSafe = riskScore < 50;
        string finalReason = reasons.Count > 0 ? string.Join(" ", reasons) : "No threats detected. URL is secure and responsive.";

        return new CheckResponse
        {
            IsSafe = isSafe,
            RiskScore = Math.Min(riskScore, 100),
            Reason = finalReason
        };
    }
}