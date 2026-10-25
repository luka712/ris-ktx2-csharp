# RisKtx2

**C# bindings for the [KTX-Software](https://github.com/KhronosGroup/KTX-Software) library.**

RisKtx2 provides idiomatic, managed .NET access to the Khronos KTX2 texture
container format — including loading, writing, Basis Universal compression and
GPU-format transcoding (BC7, ETC2, ASTC, …).

Although these bindings were created primarily to power the **Ris Game Engine**
and the surrounding Ris tooling, the public C# API is intentionally generic and
has no engine-specific dependencies, so it can be dropped into **any .NET
project** that needs to work with `.ktx2` textures.

---

## Repository layout

This repository contains the C# (.NET 10) bindings only: [`RisKtx2.Managed/`](./RisKtx2.Managed).
They call into the native `ris_ktx2` shared library via P/Invoke.

The native C++ library lives in its own repository,
[luka712/ris-ktx2-cpp](https://github.com/luka712/ris-ktx2-cpp), which publishes prebuilt
binaries for every platform as GitHub releases. Build instructions, licenses and the C ABI
are documented there.

---

## Features

- Load `.ktx2` textures from disk (`Ktx2Texture(path)`).
- Create textures from scratch and populate them from raw memory
  (`Ktx2Texture(KtxTextureCreateInfo, …)` + `SetImageFromMemory`).
- Write textures back out to `.ktx2` files (`WriteToNamedFile`).
- **Basis Universal** compression (ETC1S and UASTC) via `CompressBasis`.
- **Transcoding** of Basis-compressed textures to GPU-native formats
  (BC7, ETC2, ASTC, RGBA8, …) via `TranscodeBasis`.
- Query helpers: width, height, mip levels, row pitch, element size,
  Vulkan format, supercompression scheme, image size/offset, raw data pointer.
- Bonus: a tiny `StbImageLoader` for loading PNG/JPG/etc. as raw RGBA bytes —
  handy for feeding pixel data into a freshly created KTX2 texture.
- Deterministic cleanup through `IDisposable` (native handles are released
  automatically on `Dispose`).

---

## Requirements

### Consuming the C# bindings (most users)

- .NET 10 SDK (see [`RisKtx2.csproj`](./RisKtx2.Managed/RisKtx2/RisKtx2/RisKtx2.csproj))
- Windows x64, Linux x64 and Linux arm64 (the repo currently ships `runtimes/<rid>/native/` binaries).
  Other platforms: download the matching archive from the [ris-ktx2-cpp releases](https://github.com/luka712/ris-ktx2-cpp/releases).

### Building the native library (contributors only)

See [ris-ktx2-cpp](https://github.com/luka712/ris-ktx2-cpp#readme).

---

## Getting started

### 1. Reference the managed project

Add a project reference to `RisKtx2.Managed/RisKtx2/RisKtx2/RisKtx2.csproj`,
or copy its build output (the managed assembly **plus** the
`runtimes/win-x64/native/*.dll` files) into your application.

The native DLLs (`ris_ktx2.dll`, `ktx.dll`, `zstd.dll`, `spdlog*.dll`,
`fmt*.dll`) are automatically copied next to your executable thanks to the
`<None Update="runtimes\win-x64\native\..." />` entries in the csproj.

### 2. Load an existing `.ktx2` texture

```csharp
using RisKtx2;

using var texture = new Ktx2Texture("assets/wall_basis_uastc.ktx2");

Console.WriteLine($"{texture.Width} x {texture.Height}");
Console.WriteLine($"Supercompression: {texture.SupercompressionScheme}");

if (texture.NeedsTranscoding)
{
    // Transcode Basis Universal data to a GPU-native format.
    texture.TranscodeBasis(KtxTranscodeFormat.BC7_RGBA);
}

ulong offset = texture.GetImageOffset(level: 0, layer: 0, faceSlice: 0);
IntPtr  data   = texture.GetTextureData(offset);
ulong   size   = texture.GetImageSize(0);

// `data` / `size` can now be uploaded straight to your GPU API
// (Vulkan, D3D12, WebGPU, …).
```

### 3. Create a KTX2 texture from a PNG and write it to disk

```csharp
using RisKtx2;

var loader = new StbImageLoader();
byte[] pixels = loader.LoadImage("assets/test.png",
                                 out int width, out int height, out int channels);

var createInfo = new KtxTextureCreateInfo
{
    BaseWidth  = (uint)width,
    BaseHeight = (uint)height,
    VkFormat   = VkFormat.R8G8B8A8_UNORM,
};

using var texture = new Ktx2Texture(createInfo, KtxTextureCreateStorage.ALLOC_STORAGE);
texture.SetImageFromMemory(level: 0, layer: 0, faceSlice: 0,
                           pixels, (uint)pixels.Length);

// Optional: Basis Universal compression
texture.CompressBasis(new KtxBasisParams
{
    UseUastc        = true,
    CompressionLevel = 2,
    QualityLevel     = 128,
});

texture.WriteToNamedFile("assets/test.ktx2");
```

For more end-to-end examples, see the
[`RisKtx2.Tests`](./RisKtx2.Managed/RisKtx2/RisKtx2.Tests) project.

---

## Native library

The native binaries are checked in under
`RisKtx2.Managed/RisKtx2/RisKtx2/runtimes/<rid>/native/`, so most users do **not** need
to build the C++ side themselves. To update or add a platform, download the archive from the
[ris-ktx2-cpp releases](https://github.com/luka712/ris-ktx2-cpp/releases) and place the
`ris_ktx2` shared library under `runtimes/<rid>/native/`.

---

## Testing

- **Native (C++) tests** live in [ris-ktx2-cpp](https://github.com/luka712/ris-ktx2-cpp).
- **Managed (C#) tests** — NUnit, located in
  [`RisKtx2.Managed/RisKtx2/RisKtx2.Tests/`](./RisKtx2.Managed/RisKtx2/RisKtx2.Tests).
  Run them with:

  ```bash
  dotnet test RisKtx2.Managed/RisKtx2/RisKtx2.slnx
  ```

---

## Used by / intended for

- The **Ris Game Engine** — used as the primary texture container loader.
- The **Ris tooling** suite (sprite/asset pipelines, content cookers, …).
- Any third-party .NET project that needs KTX2 + Basis Universal support —
  the public API has no engine dependencies and is safe to use standalone.

---

## License

See [LICENSE](./LICENSE) for license information.

This project builds on top of the excellent
[KTX-Software](https://github.com/KhronosGroup/KTX-Software) library by the
Khronos Group and [`stb_image`](https://github.com/nothings/stb) by Sean Barrett,
each of which are distributed under their own licenses.
