using System;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Exercise3.UWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Exercise3.UWP.Utils;

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
                    await OpenFolderPickerPrompt($"{ViewModel.SourceFile.Name}.zip").ConfigureAwait(false);
                    break;
                case CompressionMode.Decompress:
                    await OpenFolderPickerPrompt(ViewModel.SourceFile.GetNameWithoutZipExtension()).ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionMode), compressionMode, null);
            }
            
            return await ViewModel.StartCompression(compressionMode).ConfigureAwait(false);            
        }

        private async Task OpenFolderPickerPrompt(string fileName)
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");                  

            var folder = await folderPicker.PickSingleFolderAsync();
                        
            var file = await folder.CreateFileAsync(fileName,
                CreationCollisionOption.ReplaceExisting);
            
            if (file != null)
            {
                ViewModel.OutputFile = file;
            }            
        }
    }

    public interface IOnFileDropped
    {
        Task<string> OnFileDropped(CompressionMode compressionMode);
    }
}
