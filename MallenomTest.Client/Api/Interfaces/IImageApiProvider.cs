using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MallenomTest.Contracts;
using MallenomTest.Services.Models;

namespace MallenomTest.Client.Api.Interfaces;

public interface IImageApiProvider
{
    public Task<ImageResponse[]> Get();
    public Task<HttpResponseMessage> Add(ImageRequest image);
    public Task<HttpResponseMessage> Update(int id, ImageRequest image);
    public Task<HttpResponseMessage> Delete(int id);
}