using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Client.Models;
using MallenomTest.Client.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace MallenomTest.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region Properties
    private bool _enable = false;

    public bool Enable
    {
        get => _enable;
        set => this.RaiseAndSetIfChanged(ref _enable, value);
    }

    private bool _isNotificationOpen;

    public bool IsNotificationOpen
    {
        get => _isNotificationOpen;
        set => this.RaiseAndSetIfChanged(ref _isNotificationOpen, value);
    }

    private ObservableCollection<ImageModel> _images = null!;
    
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

    private ImageModel? _selectedImage;

    public ImageModel? SelectedImage
    {
        get => _selectedImage;
        set
        {
            Enable = value is not null;
            _selectedImage = value;
        }
    }
    
    #endregion
    
    #region .ctor

    public MainViewModel()
    {
        Images = new ObservableCollection<ImageModel>();
        Enable = false;
    }
    
    #endregion

    #region Commands
    
    
    [RelayCommand]
    private async Task GetImagesAsync()
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>();
        if (imageApiProvider is null)
        {
            Console.WriteLine("Image Provider was not found");
            return;
        }
        
        var images = await imageApiProvider.Get();
        var imageList = new List<ImageModel>();
        
        foreach (var image in images)
        {
            var byteForm = Convert.FromBase64String(image.Base64EncodedImage);
            var imageStream = new MemoryStream(byteForm);
            var bitmap = new Bitmap(imageStream);
            
            var imageModel = new ImageModel(image.Name, bitmap, image.Id);
            imageList.Add(imageModel);
        };
        
        // Comparison is defined in-place because this is the only case of it being used
        imageList.Sort((lhs, rhs) => lhs.Id.CompareTo(rhs.Id));
        
        Images = new ObservableCollection<ImageModel>(imageList);
    }

    [RelayCommand]
    private async Task AddImage()
    {
        Console.WriteLine("Opening a file manager");
        var filesService = App.Current?.ServiceProvider?.GetService<IFilesService>();
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>();

        if (imageApiProvider is null)
        {
            Console.WriteLine("Image Provider was not found");
            return;
        }
        
        var file = await filesService?
            .OpenFileAsync()!;

        if (file is null)
        {
            Console.WriteLine("File not chosen");
            return;
        }
        
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
    private async Task UpdateImage()
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>()!;        
        var filesService = App.Current?.ServiceProvider?.GetService<IFilesService>();

        var file = await filesService?.OpenFileAsync()!;

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
        // SelectedImage is never `null` as the respective button cannot be pushed without a selected image
        var resp = await imageApiProvider.Update(SelectedImage!.Id, req);
        
        if (resp.IsSuccessStatusCode)
        {
            await GetImagesAsync();
        }
        else
        {
            Console.WriteLine($"Couldn't update an image {SelectedImage!.Id} - {resp.StatusCode}");
        }
    }
    
    [RelayCommand]
    private async Task DeleteImage()
    {
        var imageApiProvider = App.Current?.ServiceProvider?.GetService<IImageApiProvider>()!;
        var resp = await imageApiProvider.Delete(SelectedImage!.Id);
        if (resp.IsSuccessStatusCode)
        {
            var popup = ShowPopup();
            var images = GetImagesAsync();
            await Task.WhenAll(popup, images);
        }
        else
        {
            Console.WriteLine($"Couldn't delete an image {SelectedImage.Id} - {resp.StatusCode}");
        }
    }
    
    #endregion
    
    #region Utility

    private async Task ShowPopup()
    {
        IsNotificationOpen = true;
        await Task.Delay(2000);
        IsNotificationOpen = false;
    }
    
    #endregion
}