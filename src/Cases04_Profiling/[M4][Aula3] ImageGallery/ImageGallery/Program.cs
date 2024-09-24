using ImageProcessing;
using System.Diagnostics;

try
{
    //Input Files
    string[] imageFiles = Directory.GetFiles("C:\\Users\\sandro\\source\\repos\\inf0082-examples\\ImageGallery\\images\\", "*.jpg");

    Console.WriteLine("This is the Image Processing Example");
    Console.WriteLine("This program converts a bunch of images to grayscale");
    Console.WriteLine("There are 3 versions:");

    Console.WriteLine("(1) Running in a Single Thread ...");
    var sw = Stopwatch.StartNew(); // Start the stopwatch
    //GrayScale.ProcessST(imageFiles);
    sw.Stop(); // Stop the stopwatch
    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");

    Console.WriteLine("(2) Running in Multi Threads ... ");
    sw.Restart(); // Restart the stopwatch
    GrayScale.ProcessMT(imageFiles);
    sw.Stop(); // Stop the stopwatch
    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");

    Console.WriteLine("(3) Running in Multi Tasks ...");
    sw.Restart(); // Restart the stopwatch
    await GrayScale.ProcessTasks(imageFiles);
    sw.Stop(); // Stop the stopwatch
    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
}
