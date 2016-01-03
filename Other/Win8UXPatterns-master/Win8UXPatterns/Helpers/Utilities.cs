using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Win8UXPatterns.Helpers
{

    public class SampleData
    {
        public string Text { get; set; }
        public BitmapImage Image { get; set; }
    }

    public static class Utilities
    {

        static Utilities()
        {
            
        }

        public static List<SampleData> GetSampleData(int count)
        {
            List<SampleData> l = new List<SampleData>();
            for (int i = 0; i < count; i++)
            {
                l.Add(new SampleData() { Image = Images[GetNextRandom(Images.Count)], Text = Guid.NewGuid().ToString() });
            }
            return l;
        }

        public static int GetNextRandom(int max)
        {
            return RandomGenerator.Next(0,max);
        }

        private static Random RandomGenerator = new Random();
        private static List<BitmapImage> images;
        public static List<BitmapImage> Images
        {
            get
            {
                if (images == null) images = new List<BitmapImage>();
                return images;
            }
        }

        public static async Task InitializeAllImages()
        {

            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var imagesFolder = await installedLocation.GetFolderAsync("Images");
            var files = await imagesFolder.GetFilesAsync();
            foreach (var item in files)
            {
                Images.Add(await GetImageAsync(item));
            }
        }


        public async static Task<BitmapImage> GetImageAsync(StorageFile storageFile)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await storageFile.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }
    }
}
