namespace RisKtx2;


/// <summary>
/// UASTC encoding configuration flags.
/// </summary>
/// <remarks>
/// This is a packed bitfield matching the KTX/BasisU API:
/// - Bits 0–3: compression level (mutually exclusive)
/// - Bits 4+: optional encoding hints (combinable flags)
/// </remarks>
[Flags]
public enum KtxUastcFlags : uint
{
    // =========================================================
    // Compression Level (LOW 4 BITS — MUTUALLY EXCLUSIVE)
    // =========================================================

    /// <summary>
    /// Fastest compression (the lowest quality, the highest speed). ~43.45 dB.
    /// </summary>
    LEVEL_FASTEST = 0,

    /// <summary>
    /// Faster compression. ~46.49 dB.
    /// </summary>
    LEVEL_FASTER = 1,

    /// <summary>
    /// Default compression level (balanced). ~47.47 dB.
    /// </summary>
    LEVEL_DEFAULT = 2,

    /// <summary>
    /// Slower compression (higher quality). ~48.01 dB.
    /// </summary>
    LEVEL_SLOWER = 3,

    /// <summary>
    /// Very slow compression (the highest quality). ~48.24 dB.
    /// </summary>
    LEVEL_VERY_SLOW = 4,

    /// <summary>
    /// Bitmask used to extract the compression level (0–3/4).
    /// </summary>
    LEVEL_MASK = 0xF,

    // =========================================================
    // Encoding Hints (BIT FLAGS — COMBINABLE)
    // =========================================================

    /// <summary>
    /// Optimize encoding for the lowest UASTC reconstruction error.
    /// </summary>
    FAVOR_UASTC_ERROR = 1u << 3, // 8

    /// <summary>
    /// Optimize encoding for the lowest BC7 decode error.
    /// </summary>
    FAVOR_BC7_ERROR = 1u << 4, // 16

    /// <summary>
    /// Hint to optimize for faster ETC1 transcoding performance.
    /// </summary>
    ETC1_FASTER_HINTS = 1u << 6, // 64

    /// <summary>
    /// Hint to optimize for fastest ETC1 transcoding performance.
    /// </summary>
    ETC1_FASTEST_HINTS = 1u << 7, // 128

    /// <summary>
    /// Disables flip-and-individual optimizations for ETC1 transcoding.
    /// (Advanced/rarely used option.)
    /// </summary>
    ETC1_DISABLE_FLIP_AND_INDIVIDUAL = 1u << 8 // 256
}
