using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Logging;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Client.Models;
using MallenomTest.Client.Services.Interfaces;
using MallenomTest.Client.ViewModels;
using MallenomTest.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MallenomTest.Client.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}