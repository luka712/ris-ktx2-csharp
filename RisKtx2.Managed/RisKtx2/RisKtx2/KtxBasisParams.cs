using System.Runtime.InteropServices;

namespace RisKtx2
{
    /// <summary>
    /// The parameters for the Basis Universal compression algorithm.
    /// </summary>
    public class KtxBasisParams : IKtxBasisParams
    {
        /// <inheritdoc/>
        public uint CompressionLevel
        {
            get;
            set
            {
                if (value > 6)
                    throw new ArgumentOutOfRangeException(nameof(CompressionLevel),
                        "CompressionLevel must be in the range [0, 6].");
                field = value;
            }
        } = KtxConstants.ETC1S_DEFAULT_COMPRESSION_LEVEL;

        /// <summary>
        /// Gets or sets the quality level for compression.
        /// </summary>
        /// <remarks>
        /// QualityLevel must be in the range [1, 255].
        /// Lower values give better compression and faster processing but lower quality,
        /// while higher values give less compression, higher quality, and slower processing.
        /// This parameter automatically determines values for maxEndpoints, maxSelectors, endpointRDOThreshold,
        /// and selectorRDOThreshold for the target quality level. 
        /// Setting these parameters overrides the values determined by QualityLevel,
        /// which defaults to 128 if neither it nor both of maxEndpoints and maxSelectors have been set.
        /// </remarks>
        /// <remarks>
        /// This controls the visual quality target of ETC1S encoding.
        /// </remarks>
        public uint QualityLevel
        {
            get;
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentOutOfRangeException(nameof(QualityLevel),
                        "QualityLevel must be in the range [1, 255].");
                field = value;
            }
        } = KtxConstants.BASISU_DEFAULT_QUALITY_LEVEL;

        /// <summary>
        /// Specifies whether to use UASTC encoding. 
        /// </summary>
        public bool Uastc { get; set; }

        /// <summary>
        /// Specifies UASTC encoding options.
        /// </summary>
        public KtxUastcFlags UastcFlags { get; set; }

        /// <summary>
        /// Tunes codec parameters for better quality on normal maps (no selector RDO, no endpoint RDO) and sets the texture's DFD appropriately.
        /// Only valid for linear textures. 
        /// </summary>
        public bool NormalMap { get; set; }

        /// <summary>
        /// Number of threads used for compression. Default is <c>1</c>.
        /// </summary>
        public uint ThreadCount { get; set; } = 1;

        /// <summary>
        /// A swizzle to apply before encoding.
        /// It must match the regular expression /^[rgba01]{4}$/.
        /// If both this and preSwizzle are specified, ktxTexture_CompressBasisEx will raise KTX_INVALID_OPERATION.
        /// Usable with both ETC1S and UASTC. 
        /// </summary>
        public char[] InputSwizzle { get; set; } = ['r', 'g', 'b', 'a'];

        ///<summary>
        ///Enable Rate Distortion Optimization (RDO) post-processing.
        ///</summary>
        public bool UastcRDO { get; set; }

        /// <summary>
        /// UASTC RDO quality scalar (lambda).
        /// Lower values yield higher quality/larger LZ compressed files, higher values yield lower quality/smaller LZ compressed files. 
        /// A good range to try is [.2,4].
        /// The full range is [.001,50.0].
        /// Default is 1.0. 
        /// </summary>
        public float UastcRDOQualityScalar { get; set; }

        /// <summary>
        /// If <c>true</c>>, prints Basis Universal encoder operation details to output stream.
        /// Not recommended for GUI apps. 
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Converts this managed KtxBasisParams instance to its native representation (ris_ktxBasisParams) for use in interop calls.
        /// </summary>
        /// <returns>The <see cref="ris_ktxBasisParams"/>.</returns>
        internal static ris_ktxBasisParams ToNative(IKtxBasisParams basisParams)
        {
            var risKtxBasisParams = new ris_ktxBasisParams()
            {
                uastc = basisParams.Uastc ? 1u : 0,
                uastcFlags = (uint)basisParams.UastcFlags,
                qualityLevel = basisParams.QualityLevel,
                etc1sCompressionLevel = basisParams.CompressionLevel,
                normalMap = basisParams.NormalMap ? 1u : 0,
                threadCount = basisParams.ThreadCount,
                uastcRDO = basisParams.UastcRDO ? 1u : 0,
                uastcRDOQualityScalar = basisParams.UastcRDOQualityScalar,
                verbose = basisParams.Verbose ? 1u : 0,
            };

            // Handle the InputSwizzle array. We need to pin it in memory to get a stable pointer for the native code.
            var inputSwizzle = basisParams.InputSwizzle;
            if (inputSwizzle.Length != 4)
            {
                throw new ArgumentException("InputSwizzle must be an array of 4 characters.", nameof(InputSwizzle));
            }

            risKtxBasisParams.inputSwizzleR = (byte)inputSwizzle[0];
            risKtxBasisParams.inputSwizzleG = (byte)inputSwizzle[1];
            risKtxBasisParams.inputSwizzleB = (byte)inputSwizzle[2];
            risKtxBasisParams.inputSwizzleA = (byte)inputSwizzle[3];

            return risKtxBasisParams;
        }
    }

    /// <summary>
    /// Structure for passing extended parameters to ktxTexture2_CompressBasisEx().
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ris_ktxBasisParams
    {
        public ris_ktxBasisParams()
        {
            etc1sCompressionLevel = 0;
            qualityLevel = 0;
            normalMap = 0;
            threadCount = 0;
            uastc = 0;
            uastcFlags = 0;
            uastcRDO = 0;
            uastcRDOQualityScalar = 0.0f;
            verbose = 0;
            inputSwizzleR = 0;
            inputSwizzleG = 0;
            inputSwizzleB = 0;
            inputSwizzleA = 0;
        }

        public uint etc1sCompressionLevel;
        public uint qualityLevel;
        public uint normalMap;
        public uint threadCount;
        public uint uastc;
        public uint uastcFlags;
        public uint uastcRDO;
        public float uastcRDOQualityScalar;
        public uint verbose;
        public byte inputSwizzleR;
        public byte inputSwizzleG;
        public byte inputSwizzleB;
        public byte inputSwizzleA;
    }
}