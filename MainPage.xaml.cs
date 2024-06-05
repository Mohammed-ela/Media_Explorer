using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Correct namespace for FilePicker

namespace App;

public partial class MainPage : ContentPage
{
    public ObservableCollection<ImageMetadata> ImageMetadataCollection { get; set; }

    public MainPage()
    {
        InitializeComponent();
        ImageMetadataCollection = new ObservableCollection<ImageMetadata>();
        MetadataListView.ItemsSource = ImageMetadataCollection;
    }

    private async void OnOpenImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Select an image"
        });

        if (result != null)
        {
            DisplayedImage.Source = ImageSource.FromFile(result.FullPath);
            ImageScrollView.IsVisible = true;
            AdjustImageSize();
            AddImageMetadata(result);
        }
    }

    private void OnPageSizeChanged(object sender, EventArgs e)
    {
        if (ImageScrollView.IsVisible)
        {
            AdjustImageSize();
        }
    }

    private void AdjustImageSize()
    {
        double pageWidth = this.Width;
        double pageHeight = this.Height;

        DisplayedImage.WidthRequest = pageWidth * 0.8; // 80% of page width
        DisplayedImage.HeightRequest = pageHeight * 0.8; // 80% of page height
    }

    private void AddImageMetadata(FileResult fileResult)
    {
        var fileInfo = new FileInfo(fileResult.FullPath);
        ImageMetadataCollection.Add(new ImageMetadata
        {
            FileName = fileResult.FileName,
            FilePath = fileResult.FullPath,
            FileSize = (fileInfo.Length / 1024).ToString() // File size in KB
        });
    }
}

public class ImageMetadata
{
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public string? FileSize { get; set; }
}
