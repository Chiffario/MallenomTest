using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Contracts;
using MallenomTest.Services.Models;

namespace MallenomTest.Client.Api;

public class ImageApiProvider : IImageApiProvider
{
    private readonly string _apiBase;
    private const string ApiGetLink = "all";
    private const string ApiAddLink = "add";
    private const string ApiUpdateLink = "update/{0}";
    private const string ApiDeleteLink = "delete/{0}";


    private readonly HttpClient _httpClient;

    public ImageApiProvider(HttpClient client, string apiBase)
    {
        _apiBase = apiBase;
        _httpClient = client;
    }

    public async Task<ImageResponse[]> Get()
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_apiBase + ApiGetLink),
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
            RequestUri = new Uri(_apiBase + ApiAddLink),
            Content = JsonContent.Create(image),
        };

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Update(int id, ImageRequest image)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_apiBase + string.Format(ApiUpdateLink, id)),
            Content = JsonContent.Create(image)
        };

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Delete(int id)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(_apiBase + string.Format(ApiDeleteLink, id)),
        };

        return await _httpClient.SendAsync(requestMessage);
    }
}