namespace RisKtx2.Utility;

/// <summary>
/// The utility class for swizzling image data.
/// </summary>
internal static class SwizzleUtility
{
    internal static readonly Dictionary<VkFormat, int> VkFormatToChannels = new()
    {
        [VkFormat.R8G8B8A8_UNORM] = 4,
        [VkFormat.B8G8R8A8_UNORM] = 4,
        [VkFormat.R8G8B8_UNORM] = 3,
        [VkFormat.B8G8R8_UNORM] = 3,
    };
    
    /// <summary>
    /// Swizzles (reorders) the color channels of the image data to match the desired format.
    /// For example, converts RGBA to BGRA format. Also handles expanding 3-channel images to 4-channel.
    /// </summary>
    /// <param name="data">The source image data as a byte array.</param>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    /// <param name="channels">The number of color channels in the source image (3 or 4).</param>
    /// <param name="desiredFormat">The target <see cref="VkFormat"/> to swizzle the data to.</param>
    /// <returns>A new byte array with swizzled color channel data.</returns>
    /// <exception cref="NotSupportedException">Thrown when the requested format conversion is not supported.</exception>
    internal static byte[] Swizzle(byte[] data, int width, int height, int channels, VkFormat desiredFormat)
    {
        if (desiredFormat == VkFormat.R8G8B8A8_UNORM && channels == 4)
        {
            return data; // No swizzling needed
        }

        int desiredChannels = VkFormatToChannels[desiredFormat];
        byte[] swizzledData = new byte[width * height * desiredChannels];

        int channelIndex0 = 0;
        int channelIndex1 = 1;
        int channelIndex2 = 2;
        int channelIndex3 = 3;

        if (channels == 4)
        {
            if (desiredFormat == VkFormat.B8G8R8A8_UNORM)
            {
                channelIndex0 = 2; // R
                channelIndex1 = 1; // G
                channelIndex2 = 0; // B
                channelIndex3 = 3; // A
            }
            else if (desiredFormat == VkFormat.R8G8B8A8_UNORM)
            {
                channelIndex0 = 0; // R
                channelIndex1 = 1; // G
                channelIndex2 = 2; // B
                channelIndex3 = 3; // A
            }
            else
            {
                throw new NotSupportedException(
                    $"Swizzling from 4 channels to {desiredFormat} is not supported.");
            }
        }
        else if (channels == 3)
        {
            if (desiredFormat == VkFormat.B8G8R8A8_UNORM)
            {
                channelIndex0 = 2; // R
                channelIndex1 = 1; // G
                channelIndex2 = 0; // B
                channelIndex3 = 3; // A
            }
            else if (desiredFormat == VkFormat.R8G8B8A8_UNORM)
            {
                channelIndex0 = 0; // R
                channelIndex1 = 1; // G
                channelIndex2 = 2; // B
                channelIndex3 = 3; // A
            }
            else if (desiredFormat == VkFormat.B8G8R8_UNORM)
            {
                channelIndex0 = 2; // R
                channelIndex1 = 1; // G
                channelIndex2 = 0; // B
                channelIndex3 = -1; // A not relevant, set to -1 to avoid issues later 
            }
            else if (desiredFormat == VkFormat.R8G8B8_UNORM)
            {
                channelIndex0 = 0; // R
                channelIndex1 = 1; // G
                channelIndex2 = 2; // B
                channelIndex3 = -1; // A not relevant, set to -1 to avoid issues later 
            }
            else
            {
                throw new NotSupportedException(
                    $"Swizzling from 3 channels to {desiredFormat} is not supported.");
            }
        }

        for (int i = 0; i < width * height; i++)
        {
            swizzledData[i * desiredChannels + channelIndex0] = data[i * channels + 0]; // R
            swizzledData[i * desiredChannels + channelIndex1] = data[i * channels + 1]; // G
            swizzledData[i * desiredChannels + channelIndex2] = data[i * channels + 2]; // B

            if (channelIndex3 > -1 && channels == 3)
            {
                swizzledData[i * desiredChannels + channelIndex3] = 255;
            }
            else if (channelIndex3 > -1 && channels == 4)
            {
                swizzledData[i * desiredChannels + channelIndex3] = data[i * channels + 3]; // A;
            }
        }

        return swizzledData;
    }
}