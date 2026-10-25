using System.Runtime.InteropServices;

namespace RisKtx2
{
    /// <summary>
    /// The KTX2 create info structure.
    /// Used to describe and create the KTX2 texture object.
    /// </summary>
    public struct KtxTextureCreateInfo
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        public KtxTextureCreateInfo()
        {
            VkFormat = VkFormat.R8G8B8A8_UNORM;
            NumLevels = 1;
            BaseDepth = 1;
            NumDimensions = 2;
            NumLayers = 1;
            NumFaces = 1;
        }

        /// <summary>
        /// The format of the texture to be created, specified as a value from the <see cref="VkFormat"/> enumeration.
        /// </summary>
        public VkFormat VkFormat { get; set; }

        /// <summary>
        /// The width of the texture to be created.
        /// </summary>
        public uint BaseWidth { get; set; }

        /// <summary>
        /// The height of the texture to be created.
        /// </summary>
        public uint BaseHeight { get; set; }

        /// <summary>
        /// Depth of the base level of the texture. 
        /// </summary>
        public uint BaseDepth { get; set; }

        /// <summary>
        /// Number of dimensions in the texture, <c>1</c>, <c>2</c> or <c>3</c>. 
        /// </summary>
        public uint NumDimensions { get; set; }

        /// <summary>
        /// Number of mip levels in the texture.
        /// Should be <c>1</c> if generateMipmaps is <c>true</c>. 
        /// </summary>
        public uint NumLevels { get; set; }

        /// <summary>
        /// Number of array layers in the texture.
        /// </summary>
        public uint NumLayers { get; set; }

        /// <summary>
        /// Number of faces: <c>6</c> for cube maps, <c>1</c> otherwise. 
        /// </summary>
        public uint NumFaces { get; set; }

        /// <summary>
        /// Set to <c>true</c> if the texture is to be an array texture. 
        /// Means OpenGL will use a <c>GL_TEXTURE_*_ARRAY</c> target.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Set to <c>true</c> if mipmaps should be generated for the texture when loading into a 3D API.
        /// </summary>
        public bool GenerateMipmaps { get; set; }

        /// <summary>
        /// Converts this managed struct to the native struct that can be passed to the native API.
        /// </summary>
        /// <returns>The <see cref="ris_ktxTextureCreateInfo"/>.</returns>
        internal ris_ktxTextureCreateInfo ToNative()
        {
            return new ris_ktxTextureCreateInfo
            {
                vkFormat = VkFormat,
                baseWidth = BaseWidth,
                baseHeight = BaseHeight,
                baseDepth = BaseDepth,
                numDimensions = NumDimensions,
                numLevels = NumLevels,
                numLayers = NumLayers,
                numFaces = NumFaces,
                isArray = IsArray,
                generateMipmaps = GenerateMipmaps
            };
        }
    }

    //! Structure for passing parameters to ktxTexture2_Create().
    [StructLayout(LayoutKind.Sequential)]
    internal struct ris_ktxTextureCreateInfo
    {
        //! VkFormat for texture.
        internal VkFormat vkFormat;

        //! Width of the base level of the texture.
        internal uint baseWidth;

        //! Height of the base level of the texture.
        internal uint baseHeight;

        //! Depth of the base level of the texture. 
        internal uint baseDepth;

        //! Number of dimensions in the texture, 1, 2 or 3. 
        internal uint numDimensions;

        //! Number of mip levels in the texture. Should be 1 if generateMipmaps is 'true'. 
        internal uint numLevels;

        //! Number of array layers in the texture. 
        internal uint numLayers;

        //! Number of faces: 6 for cube maps, 1 otherwise. 
        internal uint numFaces;

        // Set to true if the texture is to be an array texture. Means OpenGL will use a GL_TEXTURE_*_ARRAY target. 
        internal bool isArray;

        //! Set to true if mipmaps should be generated for the texture when loading into a 3D API. 
        internal bool generateMipmaps;
    };
}
