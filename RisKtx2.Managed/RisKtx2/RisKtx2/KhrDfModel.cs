namespace RisKtx2;

/// <summary>
/// Model in which the color coordinate space is defined.
/// There is no requirement that a color format uses all the
/// channel types that are defined in the color model. 
/// </summary>
public enum KhrDfModel : uint
{
    /// <summary>
    /// No interpretation of color channels defined.
    /// </summary>
    UNSPECIFIED = 0U,

    /// <summary>
    /// Color primaries (red, green, blue) + alpha, depth and stencil.
    /// </summary>
    RGBSDA = 1U,

    /** Color differences (Y', Cb, Cr) + alpha, depth and stencil */
    KHR_DF_MODEL_YUVSDA = 2U,

    /** Color differences (Y', I, Q) + alpha, depth and stencil */
    KHR_DF_MODEL_YIQSDA = 3U,

    /** Perceptual color (CIE L*a*b*) + alpha, depth and stencil */
    KHR_DF_MODEL_LABSDA = 4U,

    /** Subtractive colors (cyan, magenta, yellow, black) + alpha */
    KHR_DF_MODEL_CMYKA = 5U,

    /** Non-color coordinate data (X, Y, Z, W) */
    KHR_DF_MODEL_XYZW = 6U,

    /** Hue, saturation, value, hue angle on color circle, plus alpha */
    KHR_DF_MODEL_HSVA_ANG = 7U,

    /** Hue, saturation, lightness, hue angle on color circle, plus alpha */
    KHR_DF_MODEL_HSLA_ANG = 8U,

    /** Hue, saturation, value, hue on color hexagon, plus alpha */
    KHR_DF_MODEL_HSVA_HEX = 9U,

    /** Hue, saturation, lightness, hue on color hexagon, plus alpha */
    KHR_DF_MODEL_HSLA_HEX = 10U,

    /** Lightweight approximate color difference (luma, orange, green) */
    KHR_DF_MODEL_YCGCOA = 11U,

    /** ITU BT.2020 constant luminance YcCbcCrc */
    KHR_DF_MODEL_YCCBCCRC = 12U,

    /** ITU BT.2100 constant intensity ICtCp */
    KHR_DF_MODEL_ICTCP = 13U,

    /** CIE 1931 XYZ color coordinates (X, Y, Z) */
    KHR_DF_MODEL_CIEXYZ = 14U,

    /** CIE 1931 xyY color coordinates (X, Y, Y) */
    KHR_DF_MODEL_CIEXYY = 15U,

    /* Compressed formats start at 128. */
    /* These compressed formats should generally have a single sample,
       sited at the 0,0 position of the texel block. Where multiple
       channels are used to distinguish formats, these should be cosited. */
    /* Direct3D (and S3) compressed formats */
    /* Note that premultiplied status is recorded separately */
    /** DXT1 "channels" are RGB (0), Alpha (1)
        DXT1/BC1 with one channel is opaque
        DXT1/BC1 with a cosited alpha sample is transparent */
    KHR_DF_MODEL_DXT1A = 128U,
    KHR_DF_MODEL_BC1A = 128U,

    /** DXT2/DXT3/BC2, with explicit 4-bit alpha */
    KHR_DF_MODEL_DXT2 = 129U,
    KHR_DF_MODEL_DXT3 = 129U,
    KHR_DF_MODEL_BC2 = 129U,

    /** DXT4/DXT5/BC3, with interpolated alpha */
    KHR_DF_MODEL_DXT4 = 130U,
    KHR_DF_MODEL_DXT5 = 130U,
    KHR_DF_MODEL_BC3 = 130U,

    /** BC4 - single channel interpolated 8-bit data
        (The UNORM/SNORM variation is recorded in the channel data) */
    KHR_DF_MODEL_BC4 = 131U,

    /** BC5 - two channel interpolated 8-bit data
        (The UNORM/SNORM variation is recorded in the channel data) */
    KHR_DF_MODEL_BC5 = 132U,

    /** BC6H - DX11 format for 16-bit float channels */
    KHR_DF_MODEL_BC6H = 133U,

    /** BC7 - DX11 format */
    KHR_DF_MODEL_BC7 = 134U,
    /* Gap left for future desktop expansion */

    /* Mobile compressed formats follow */
    /** A format of ETC1 indicates that the format shall be decodable
        by an ETC1-compliant decoder and not rely on ETC2 features */
    KHR_DF_MODEL_ETC1 = 160U,

    /** A format of ETC2 is permitted to use ETC2 encodings on top of
        the baseline ETC1 specification.
        The ETC2 format has channels "red", "green", "RGB" and "alpha",
        which should be cosited samples.
        Punch-through alpha can be distinguished from full alpha by
        the plane size in bytes required for the texel block */
    KHR_DF_MODEL_ETC2 = 161U,

    /** Adaptive Scalable Texture Compression */
    /** ASTC HDR vs LDR is determined by the float flag in the channel */
    /** ASTC block size can be distinguished by texel block size */
    KHR_DF_MODEL_ASTC = 162U,

    /// <summary>
    /// ETC1S is a simplified subset of ETC1.
    /// </summary>
    ETC1S = 163U,

    /** PowerVR Texture Compression v1 */
    KHR_DF_MODEL_PVRTC = 164U,

    /** PowerVR Texture Compression v2 */
    KHR_DF_MODEL_PVRTC2 = 165U,

    /// <summary>
    /// UASTC is a transcodable subset of ASTC
    /// with additions to support the transcoding.
    /// </summary>
    UASTC = 166U,

    /* Proprietary formats (ATITC, etc.) should follow */
    KHR_DF_MODEL_MAX = 0xFFU
}