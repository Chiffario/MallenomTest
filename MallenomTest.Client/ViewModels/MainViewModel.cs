using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Client.Models;
using MallenomTest.Client.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace MallenomTest.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private ObservableCollection<ImageModel> _images;
    public ObservableCollection<ImageModel> Images
    {
        get
        {
            return _images;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _images, value);
        }
    }

    public MainViewModel()
    {
        Images = new();
    }
    
    [RelayCommand]
    private async Task GetImagesAsync()
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>();
        if (imageApiProvider is null)
        {
            Console.WriteLine("Image api provider not registered");
            return;
        }
        Console.WriteLine("Requesting images");
        var images = await imageApiProvider.Get();
        var imageList = new ObservableCollection<ImageModel>();
        foreach (var image in images)
        {
            Console.WriteLine($"{image.Id}: {image.Name}");
            var byteForm = image.Base64EncodedImage;
            var imageStream = new MemoryStream(byteForm);
            var bitmap = new Bitmap(imageStream);
            // imageControl.Source = bitmap;
            
            var imageModel = new ImageModel(image.Name, bitmap, image.Id);
            imageList.Add(imageModel);
        };
        Images = imageList;
        Console.WriteLine(Images.Count);
    }

    [RelayCommand]
    private async Task AddImage()
    {
        Console.WriteLine("Opening a file manager");
        var filesService = App.Current?.ServiceProvider?.GetService<IFilesService>();
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>();

        var file = await filesService?.OpenFileAsync();

        if (file is null)
        {
            Console.WriteLine("File not chosen");
            return;
        }

        Console.WriteLine($"Got file: {file.Name}");
        var fileBytes = File.ReadAllBytes(file.Path.AbsolutePath);
        var extension = Path.GetExtension(file.Path.AbsolutePath);
        var resp = await imageApiProvider.Add(new ImageRequest
        {
            Name = file.Name,
            FileType = extension,
            Base64EncodedImage = Convert.ToBase64String(fileBytes)
        });

        if (resp.IsSuccessStatusCode)
        {
            await GetImagesAsync();
        }
        else
        {
            Console.WriteLine($"Couldn't add an image - {resp.StatusCode}");
        }
        
    }
    
    [RelayCommand]
    private async Task UpdateImage(ImageModel imageModel)
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>()!;        
        var filesService = App.Current?.ServiceProvider?.GetService<IFilesService>();

        var file = await filesService?.OpenFileAsync();

        if (file is null)
        {
            Console.WriteLine("File not chosen");
            return;
        }
        
        var fileBytes = File.ReadAllBytes(file.Path.AbsolutePath);
        var extension = Path.GetExtension(file.Path.AbsolutePath);

        var req = new ImageRequest
        {
            Name = file.Name,
            FileType = extension,
            Base64EncodedImage = Convert.ToBase64String(fileBytes)
        };
        var resp = await imageApiProvider.Update(imageModel.Id, req);
        
        if (resp.IsSuccessStatusCode)
        {
            await GetImagesAsync();
        }
        else
        {
            Console.WriteLine($"Couldn't update an image {imageModel.Id} - {resp.StatusCode}");
        }
    }
    
    [RelayCommand]
    private async Task DeleteImage(ImageModel imageModel)
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>()!;
        var resp = await imageApiProvider.Delete(imageModel.Id);
        if (resp.IsSuccessStatusCode)
        {
            await GetImagesAsync();
        }
        else
        {
            Console.WriteLine($"Couldn't delete an image {imageModel.Id} - {resp.StatusCode}");
        }
    }
}