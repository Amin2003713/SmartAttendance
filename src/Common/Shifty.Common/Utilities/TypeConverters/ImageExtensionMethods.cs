using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Shifty.Common.Utilities.TypeComverters;

public static class ImageExtensionMethods
{
    private const int TwoMegabytes = 2 * 1024 * 1024;

    /// <summary>
    ///     Converts a Base64 encoded image to two compressed versions: a preview and a main image.
    ///     The preview image is resized to smaller dimensions with low quality,
    ///     while the main image is resized to larger dimensions with moderate quality.
    /// </summary>
    /// <param name="imageBytes"></param>
    /// <param name="compress"></param>
    /// <param name="base64String">The Base64 encoded image string.</param>
    /// <returns>A tuple containing the preview and main image byte arrays.</returns>
    public static async Task<byte[]> CompressAsync(this byte[] imageBytes, bool compress)
    {
        if (!compress) return imageBytes;

        if (imageBytes == null || imageBytes.Length == 0)
            throw new ArgumentException("Input image cannot be null or empty.", nameof(imageBytes));

        using var ms    = new MemoryStream(imageBytes);
        using var image = await Image.LoadAsync(ms);

        // Resize in place:
        // e.g. 1280x720 -> 640x360 with lower JPEG quality
        image.Mutate(ctx =>
            ctx.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max, Size = new Size(640 / 2, 360 / 2)
            })
        );

        // Save directly to a new MemoryStream
        using var previewStream = new MemoryStream();
        await image.SaveAsync(previewStream,
            new JpegEncoder
            {
                Quality = 20 // e.g. "low" quality
            });

        return previewStream.ToArray();
    }

    public static async Task<byte[]> ToByte(this IFormFile image, CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }


    /// <summary>
    ///     If the stream is recognized as an image by ImageSharp, this method re-encodes it as JPEG.
    ///     Otherwise, it returns the original stream unmodified.
    ///     - If the image is over 2MB, it uses a lower JPEG quality (e.g. 50).
    ///     - Otherwise, it uses a high JPEG quality (e.g. 90).
    /// </summary>
    /// <param name="sourceStream">The incoming file stream.</param>
    /// <param name="fileType"></param>
    /// <returns>A new Stream if it was compressed; otherwise the original stream.</returns>
    public static async Task<Stream> CompressIfImageAsync(this Stream sourceStream, FileType fileType)
    {
        if (fileType != FileType.Picture)
            return sourceStream;

        // We want to be able to read from the start of the stream
        if (!sourceStream.CanSeek)
        {
            var tempStream = new MemoryStream();
            await sourceStream.CopyToAsync(tempStream);
            tempStream.Position = 0;
            sourceStream = tempStream;
        }
        else
        {
            sourceStream.Position = 0;
        }

        // If the file is empty, throw
        if (sourceStream.Length == 0)
            throw new ArgumentException("Input stream cannot be empty.", nameof(sourceStream));

        // Detect if the file is an image (JPEG, PNG, GIF, BMP, etc.)
        var format = await Image.DetectFormatAsync(sourceStream);

        if (format == null)
        {
            // Not recognized as an image; return the original stream (reset position to 0)
            sourceStream.Position = 0;
            return sourceStream;
        }

        // It's an image; let's compress it
        sourceStream.Position = 0;
        using var image = await Image.LoadAsync(sourceStream);

        // Decide on quality based on file size
        var quality = sourceStream.Length > TwoMegabytes ? 50 : 90;

        // Optionally, you could also resize if you want to reduce dimensions for large files:
        // if (sourceStream.Length > TwoMegabytes)
        // {
        //     image.Mutate(ctx =>
        //     {
        //         var newWidth  = image.Width  / 2;
        //         var newHeight = image.Height / 2;
        //         ctx.Resize(new ResizeOptions
        //         {
        //             Size = new Size(newWidth, newHeight),
        //             Mode = ResizeMode.Max
        //         });
        //     });
        // }

        // Encode as JPEG
        var outputStream = new MemoryStream();
        var encoder = new JpegEncoder
        {
            Quality = quality
        };

        await image.SaveAsync(outputStream, encoder);

        // Reset the output so the caller can read from the start
        outputStream.Position = 0;
        return outputStream;
    }
}