using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessing
{
    internal class GrayScale
    {
        public static void ProcessST(string[] imageFiles)
        {
            string baseOutputPath = "C:\\Users\\sandro\\source\\repos\\inf0082-examples\\ImageGallery\\output-st";
            foreach (string imagePath in imageFiles)
            {
                ProcessImage(imagePath, baseOutputPath);
            }
        }

        public static void ProcessMT(string[] imageFiles)
        {
            string baseOutputPath = "C:\\Users\\sandro\\source\\repos\\inf0082-examples\\ImageGallery\\output-mt";

            // Definir as opções para rodar com no máximo 4 threads
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4 // Limitar a 4 threads
            };

            Parallel.ForEach(imageFiles, parallelOptions, imagePath => ProcessImage(imagePath, baseOutputPath));
        }

        public static async Task ProcessTasks(string[] imageFiles)
        {
            string baseOutputPath = "C:\\Users\\sandro\\source\\repos\\inf0082-examples\\ImageGallery\\output-tasks";

            var tasks = imageFiles.Select(imagePath =>
            {
                return Task.Run(() => ProcessImage(imagePath, baseOutputPath));
            });

            await Task.WhenAll(tasks);
        }

        private static void ProcessImage(string imagePath, string baseOutputPath)
        {
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        int grayScale = (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
                        Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                        bitmap.SetPixel(x, y, grayColor);
                    }
                }
                string outputPath = Path.Combine(baseOutputPath,
                                                 Path.GetFileName(imagePath));
                bitmap.Save(outputPath, ImageFormat.Jpeg);
            }
        }
    }
}