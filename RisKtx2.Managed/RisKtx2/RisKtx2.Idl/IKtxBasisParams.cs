namespace RisKtx2;

/// <summary>
/// The parameters for the Basis Universal compression algorithm.
/// </summary>
public interface IKtxBasisParams
{
    /// <summary>
    /// Encoding speed vs. quality tradeoff.
    /// Range is [0,5].
    /// Higher values are slower but give higher quality.
    /// There is no default.
    /// Callers must explicitly set this value.
    /// Currently, this is 2.
    /// </summary>
    public uint CompressionLevel { get; set; }

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
    public uint QualityLevel { get; set; }

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
    public uint ThreadCount { get; set; }

    /// <summary>
    /// A swizzle to apply before encoding.
    /// It must match the regular expression /^[rgba01]{4}$/.
    /// If both this and preSwizzle are specified, ktxTexture_CompressBasisEx will raise KTX_INVALID_OPERATION.
    /// Usable with both ETC1S and UASTC. 
    /// </summary>
    public char[] InputSwizzle { get; set; }

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
}