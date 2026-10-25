namespace RisKtx2
{
    /// <summary>
    /// The KtxTexture class represents a texture loaded from a KTX file. 
    /// It provides methods for loading, transcoding, and retrieving texture data.
    /// The class manages the lifecycle of the native KTX texture object, ensuring that resources are properly released when the texture is no longer needed. 
    /// It also includes error handling to provide informative exceptions when operations fail, such as loading or transcoding errors.
    /// </summary>
    public interface IKtx2Texture : IDisposable
    {
        /// <summary>
        /// The file path to the .ktx or .ktx2 texture if it was loaded from a file.
        /// </summary>
        string? FilePath { get; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        uint Width { get; }

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        uint Height { get; }

        /// <summary>
        /// Gets the data size.
        /// </summary>
        ulong DataSize { get; }

        /// <summary>
        /// Checks if the texture needs transcoding. 
        /// This is typically true for textures that are compressed using BasisU/ETC1S or UASTC formats 
        /// and have not yet been transcoded to a GPU-compatible format.
        /// </summary>
        bool NeedsTranscoding { get; }

        /// <summary>
        /// Gets the size of the image data for a specific mip level.
        /// </summary>
        /// <param name="mipLevel">The mip level.</param>
        /// <returns>The size of the image.</returns>
        ulong GetImageSize(uint mipLevel);

        /// <summary>
        /// Gets the row pitch (the number of bytes between the start of one row of pixel data and the start of the next row) for a specific mip level.
        /// </summary>
        /// <param name="mipLevel">The mip level.</param>
        /// <returns>The size of a row pitch.</returns>
        uint GetRowPitch(uint mipLevel);

        /// <summary>
        /// Transcode the basis texture to the specified transcoding format.
        /// </summary>
        /// <param name="transcodeFormat">The transcoding format.</param>
        /// <param name="transcodeFlags">The transcode flags.</param>
        void TranscodeBasis(KtxTranscodeFormat transcodeFormat,
            KtxTranscodeFlags transcodeFlags = KtxTranscodeFlags.NONE);

        /// <summary>
        /// Gets the offset of the image data for a specific level, layer, and face/slice of the texture.
        /// </summary>
        /// <param name="level">The mip level of the image.</param>
        /// <param name="layer">The array layer level of the image.</param>
        /// <param name="faceSlice">The cube map face or depth slice of the image. </param>
        /// <returns>The offset.</returns>
        ulong GetImageOffset(uint level, uint layer, uint faceSlice);

        /// <summary>
        /// Compresses the texture using the specified basis parameters.
        /// </summary>
        /// <param name="basisParams">The <see cref="IKtxBasisParams"/>.</param>
        void CompressBasis(IKtxBasisParams basisParams);

        /// <summary>
        /// Compresses a KTX2 texture using Basis Universal supercompression.
        ///
        /// The source images are encoded into Basis Universal format (typically ETC1S, depending on configuration)
        /// and stored in a supercompressed form inside the KTX2 container.
        ///
        /// This process replaces the original uncompressed image data and updates the texture metadata (including DFD)
        /// to reflect the new compressed state.
        ///
        /// <para/>
        /// After compression, the texture cannot be directly uploaded to the GPU. It must first be transcoded
        /// into a GPU-supported block-compressed format (such as ASTC, BC7, or ETC2) before use in rendering APIs.
        /// </summary>
        /// <param name="quality">
        /// Compression quality value in the range 1–255.
        /// If 0 is provided, a default value of 128 is used by the underlying implementation.
        /// Lower values produce faster compression and smaller files with lower quality.
        /// Higher values produce better quality but slower compression and larger output.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if the Basis compression operation fails.
        /// </exception>
        /// <remarks>
        /// Based on KTX-Software / libktx API:
        /// https://github.khronos.org/KTX-Software/libktx/group__writer.html#ga405c44d6daf8ddf83dc805810bf4f989
        /// </remarks>
        void CompressBasis(uint quality = 0);

        /// <summary>
        /// Encode and compress a ktx texture with uncompressed images to astc.
        /// The images are either encoded to ASTC block-compressed format.
        /// The encoded images replace the original images and the texture's fields including the DFD are modified to reflect the new state.
        /// Such textures can be directly uploaded to a GPU via a graphics API.
        /// </summary>
        /// <param name="quality">
        /// Compression quality, a value from 0 to 100.
        /// Higher=higher quality/slower speed.
        /// Lower=lower quality/faster speed.
        /// </param>
        void CompressAstc(uint quality);

        /// <summary>
        /// Deflate the data in a ktxTexture2 object using miniz (ZLIB).
        /// The texture's levelIndex, dataSize, DFD, data pointer, and supercompressionScheme
        /// will all be updated after successful deflation to reflect the deflated data.
        /// </summary>
        /// <param name="compressionLevel">
        /// Set speed vs. compression ratio trade-off.
        /// Values between 1 and 9 are accepted.
        /// The lower the level, the faster.</param>
        void DeflateZLIB(uint compressionLevel);

        /// <summary>
        /// Deflate the data in a ktxTexture2 object using Zstandard.
        /// The texture's levelIndex, dataSize, DFD, data pointer, and supercompressionScheme will
        /// all be updated after successful deflation to reflect the deflated data.
        /// </summary>
        /// <param name="compressionLevel">
        /// Set speed vs. compression ratio trade-off. Values between 1 and 22 are accepted.
        /// The lower the level, the faster.
        /// Values above 20 should be used with caution as they require more memory.
        /// </param>
        void DeflateZstd( uint compressionLevel);
        
        /// <summary>
        /// Gets the raw texture data from the native KTX texture object.
        /// </summary>
        /// <returns>The texture data as an int ptr.</returns>
        IntPtr GetTextureData(ulong offset = 0);

        /// <summary>
        /// Gets the texture data from the native KTX texture object.
        /// </summary>
        /// <param name="level">The mipmap level.</param>
        /// <param name="layer">The array layer.</param>
        /// <param name="faceSlice">The face or slice index.</param>
        /// <returns>The texture data as a byte array.</returns>
        byte[] GetTextureDataAsByteArray(uint level = 0, uint layer = 0, uint faceSlice = 0);

        /// <summary>
        /// Gets the number of mipmap levels.
        /// </summary>
        uint NumLevels { get; }

        /// <summary>
        /// Gets the Vulkan format of the texture.
        /// </summary>
        VkFormat VkFormat { get; }

        /// <summary>
        /// Gets the KHR Data Format color model of the texture.
        /// This describes how the color channels are interpreted (e.g. RGBSDA, UASTC, ETC1S, ASTC).
        /// </summary>
        KhrDfModel ColorModel { get; }

        /// <summary>
        /// Get a TextureFormatInfo instance based on the provided KtxTranscodeFormat.
        /// </summary>
        /// <param name="format">The <see cref="KtxTranscodeFormat"/>.</param>
        /// <returns>The <see cref="TextureFormatInfo"/>.</returns>
        TextureFormatInfo GetTextureFormatInfo(KtxTranscodeFormat format);

        /// <summary>
        /// Creates an independent deep copy of the texture, including image data and metadata.
        /// The returned texture owns its own native resources and must be disposed separately.
        /// </summary>
        /// <returns>A new <see cref="IKtx2Texture"/> that is a copy of this texture.</returns>
        IKtx2Texture CreateCopy();
    }
}
