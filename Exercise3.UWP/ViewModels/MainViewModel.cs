using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Exercise3.Compressor;
using Exercise3.UWP.Helpers;
using Exercise3.UWP.Views;

namespace Exercise3.UWP.ViewModels
{
    public class MainViewModel : Observable
    {
        public IOnFileDropped ViewHandler { set; get; }
        public StorageFile SourceFile { get; set; }
        public StorageFile OutputFile { get; set; }

        private string _textOutput;
        public string TextOutput
        {
            get => _textOutput ?? "Drag a file into the box below to compress or decompress it:";
            set => Set(ref _textOutput, value, nameof(TextOutput));
        }
        
        private ICommand _getStorageItemsCommand;
        public ICommand GetStorageItemsCommand => _getStorageItemsCommand
            ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));

        public async void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
        {
            if (items.Count > 1)
            {
                TextOutput = "Can only handle one file at a time, please try again with only one data file";
                return;
            }
                        
            if (items[0].IsOfType(StorageItemTypes.Folder))
            {
                TextOutput = "Cannot handle folders, please try again with a data file";
                return;
            }

            SourceFile = (StorageFile) items[0];            

            TextOutput = "Working...";            
            
            TextOutput = await ViewHandler.OnFileDropped(SourceFile.FileType == ".zip"
                ? CompressionMode.Decompress
                : CompressionMode.Compress).ConfigureAwait(true);                
        }

        public async Task<string> StartCompression(CompressionMode mode)
        {
            if (SourceFile == null || OutputFile == null) return "Output file not selected, please try again";            

            var sourceStream = await SourceFile.OpenStreamForReadAsync().ConfigureAwait(false);
            var outputStream = await OutputFile.OpenStreamForWriteAsync().ConfigureAwait(false);            

            switch (mode)
            {
                case CompressionMode.Compress:
                    return StreamCompressor.Compress(ref sourceStream, ref outputStream).GetCompressionData();
                case CompressionMode.Decompress:
                    return StreamCompressor.Decompress(ref sourceStream, ref outputStream).GetDecompressionData();
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
