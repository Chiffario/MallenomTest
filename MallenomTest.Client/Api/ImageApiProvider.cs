using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Contracts;
using MallenomTest.Services.Models;
using Microsoft.Extensions.Logging;
using Splat;
using ILogger = Splat.ILogger;

namespace MallenomTest.Client.Api;

public class ImageApiProvider : IImageApiProvider
{
    // TODO: Replace with runtime-declared
    private const string ApiBase = "http://localhost:5141/api/images/";
    private const string ApiGetLink = "all";
    private const string ApiAddLink = "add";
    private const string ApiUpdateLink = "update/{0}";
    private const string ApiDeleteLink = "delete/{0}";
    
    
    private readonly HttpClient _httpClient;

    public ImageApiProvider(HttpClient client)
    {
        _httpClient = client;
    }
    
    public async Task<ImageResponse[]> Get()
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(ApiBase + ApiGetLink),
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response is { IsSuccessStatusCode: false })
        {
            return [];
        }

        return await response.Content.ReadFromJsonAsync<ImageResponse[]>() ?? [];
    }

    public async Task<HttpResponseMessage> Add(ImageRequest image)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(ApiBase + ApiAddLink),
            Content = JsonContent.Create(image),
        };

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Update(int id, ImageRequest image)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(ApiBase + string.Format(ApiUpdateLink, id)),
            Content = JsonContent.Create(image)
        };

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Delete(int id)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(ApiBase + string.Format(ApiDeleteLink, id)),
        };

        return await _httpClient.SendAsync(requestMessage);
    }
}