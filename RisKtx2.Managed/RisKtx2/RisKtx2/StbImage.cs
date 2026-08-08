using RisKtx2.Utility;

namespace RisKtx2;

/// <summary>
/// The StbImage class represents a raw image loaded from a file using the stb_image library.
/// </summary>
public class StbImage
{
    /// <summary>
    /// The constructor for the StbImage class.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="channels">The number of channels.</param>
    /// <param name="data">The image data.</param>
    /// <param name="format">The image format.</param>
    internal StbImage(int width, int height, int channels, byte[] data, VkFormat format)
    {
        Width = width;
        Height = height;
        Channels = channels;
        Bytes = data;
        Format = format;
    }

    /// <summary>
    /// The width of the image in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height of the image in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The number of color channels in the image (e.g., 3 for RGB, 4 for RGBA).
    /// </summary>
    public int Channels { get; }

    /// <summary>
    /// The raw image data as a byte array.
    /// </summary>
    public byte[] Bytes { get; }

    /// <summary>
    /// The stride (number of bytes per row) of the image data.
    /// This is calculated as Width * Channels.
    /// </summary>
    public int Stride => Width * Channels;

    /// <summary>
    /// The format of the image data.
    /// </summary>
    public VkFormat Format { get; }

    /// <summary>
    /// Swizzles the image data to match the desired format.
    /// </summary>
    /// <param name="desiredFormat">The desired format.</param>
    /// <returns>The new image.</returns>
    public StbImage Swizzle(VkFormat desiredFormat)
    {
        var map = SwizzleUtility.VkFormatToChannels;
        
        if (!map.TryGetValue(desiredFormat, out int desiredChannels))
        {
            throw new ArgumentException(
                $"Unsupported format: {desiredFormat}. Supported formats are: {string.Join(", ", map.Keys)}",
                nameof(desiredFormat));
        }
        
        var newBytes = SwizzleUtility.Swizzle(Bytes, Width, Height, Channels, desiredFormat);
        return new StbImage(Width, Height, desiredChannels, newBytes, desiredFormat);
    }
}