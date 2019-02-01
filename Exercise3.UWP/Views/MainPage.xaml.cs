using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Exercise3.UWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Exercise3.UWP.Models;

namespace Exercise3.UWP.Views
{
    public sealed partial class MainPage : Page, IOnFileDropped
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();        

        public MainPage()
        {
            InitializeComponent();
            ViewModel.ViewHandler = this;            
        }

        public async Task<string> OnFileDropped(CompressionMode compressionMode)
        {            
            switch (compressionMode)
            {
                case CompressionMode.Compress:
                    await LaunchCompressOutputPrompt().ConfigureAwait(false);
                    break;
                case CompressionMode.Decompress:
                    await LaunchDecompressOutputPrompt().ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionMode), compressionMode, null);
            }
            
            return await ViewModel.StartCompression(compressionMode).ConfigureAwait(false);            
        }

        private async Task LaunchCompressOutputPrompt()
        {
            var fileSavePicker = new FileSavePicker();
            fileSavePicker.FileTypeChoices.Add("Compressed file", new List<string> {".zip"});
            fileSavePicker.SuggestedFileName = $"{ViewModel.SourceFile.Name}";

            var compressedFile = await fileSavePicker.PickSaveFileAsync();
            if (compressedFile != null)
            {
                ViewModel.OutputFile = compressedFile;
            }
        }

        private async Task LaunchDecompressOutputPrompt()
        {
            var fileSavePicker = new FileSavePicker();
            fileSavePicker.FileTypeChoices.Add("Decompressed file", new List<string> {".txt"});
            fileSavePicker.SuggestedFileName = $"{ViewModel.SourceFile.Name}";

            var decompressedFile = await fileSavePicker.PickSaveFileAsync();
            if (decompressedFile != null)
            {                
                ViewModel.OutputFile = decompressedFile;
            }
        }
    }

    public interface IOnFileDropped
    {
        Task<string> OnFileDropped(CompressionMode compressionMode);
    }
}
