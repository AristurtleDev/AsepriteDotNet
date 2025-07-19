# AsepriteDotNet Coding Guidelines

## Introduction

As AsepriteDotNet evolves, we need consistent coding conventions to ensure maintainability and readability. This document outlines the standards expected to be followed when contributing to AsepriteDotNet.

These guidelines follow the general principle of "use Visual Studio defaults" and align with standard .NET coding conventions while addressing specific needs of the AsepriteDotNet library.

## Code Formatting

1. **Braces**: Use [Allman style](http://en.wikipedia.org/wiki/Indent_style#Allman_style) braces, where each brace begins on a new line.

2. **Indentation**: Use four spaces of indentation (no tabs).

3. **Single-line Statements**: All `if`/`else if`/`else` blocks should always use braces, even for single statements. This maintains consistency across the codebase.

    ```csharp
    // correct
    if (cel != null)
    {
        ProcessCel(cel);
    }

    // incorrect
    if (cel != null)
        ProcessCel(cel);
    ```

4. **Line Spacing**: Avoid more than one empty line at any time. Do not have two blank lines between members of a type.

5. **White Space**: Avoid spurious free spaces (e.g., avoid `if (someVar == 0)...`).

6. **This Keyword**: Avoid `this.` unless absolutely necessary.

## Naming Conventions

### Type Naming

- **Classes and Structs**: PascalCase (e.g., `AsepriteFile`, `Rgba32`, `AsepriteColorUtilities`)
- **Interfaces**: Prefix with "I" + PascalCase (e.g., `IImageData`, `ICelProcessor`)
- **Enums**: PascalCase for type and values (e.g., `AsepriteBlendMode.Multiply`, `AsepriteColorDepth.RGBA`)
- **Type Parameters**: Single capital letter (T) or prefixed with "T" (e.g., `TConverter`, `TPixel`)
- **Make all internal and private types static or sealed** unless derivation is required.

### Member Naming

- **Methods**: PascalCase for all method names, including local functions (e.g., `LoadFile()`, `RenderFrame()`, `BlendPixels()`)
- **Properties**: PascalCase (e.g., `CanvasSize`, `ColorDepth`, `BlendMode`)
- **Private/Internal Fields**:
  - Use `_camelCase` for internal and private instance fields (e.g., `_pixels`, `_layers`, `_isVisible`)
  - Prefix static fields with `s_` (e.g., `s_defaultPalette`, `s_magicNumber`)
  - Prefix thread static fields with `t_` (e.g., `t_readerContext`)
  - Use `readonly` where possible (after `static` when used together)
- **Public Fields**: Use PascalCasing with no prefix (use public fields sparingly; prefer properties)
- **Constants**: Use CONSTANT_CASE (e.g., `HEADER_SIZE`, `FRAME_MAGIC`, `MAX_PALETTE_SIZE`)
- **Events**: PascalCase with "EventHandler" suffix for delegate types (e.g., `FrameProcessedEventHandler`)

### Extension Methods

**Preferred Approach for AsepriteDotNet Types**:
- For types that are part of AsepriteDotNet (where we control the source code), prefer static methods within the type itself rather than extension methods
- Extension methods should primarily be used for extending types from .NET BCL or external libraries where we don't control the source

**When Extension Methods Are Necessary**:
- **Extension Class Naming**: `{TypeName}Extensions` (e.g., `AsepriteFileExtensions`, `Rgba32Extensions`)
- **One Extension Class Per Type**: Each type being extended should have its own extension class
- **No Mixed Extensions**: Don't extend multiple types in a single extension class

### Conversion Methods

- **Method Naming Standard**:
  - Use `From{SourceType}` for static factory methods on the target type
    - Example: `Rgba32.FromColor(color)` creates an `Rgba32` from a `Color`
  - Use `To{TargetType}` for extension methods on the source type
    - Example: `rgba32.ToColor()` as an extension method on `Rgba32` returns a `Color`
  - Helper classes should use `{SourceType}To{TargetType}` naming for conversion methods
    - Example: `ColorHelper.Rgba32ToColor(rgba32)`

### Helper Classes

- **Helper Class Naming**: `{Domain}Helper` or `{Domain}Utilities` (e.g., `AsepriteColorUtilities`, `PixelHelper`)
- **Purpose**: For utility methods that don't belong to a specific type
- **Static Only**: Helper classes should be static classes with static methods
- **No Instance State**: Helper classes should not maintain instance state

## Declaration and Usage Guidelines

1. **Visibility**: Always specify visibility, even if it's the default (e.g., `private Rgba32[] _pixels` not `Rgba32[] _pixels`).
   - Visibility should be the first modifier (e.g., `public sealed` not `sealed public`).

2. **Namespace Imports**:
   - Specify at the top of the file, *outside* of `namespace` declarations
   - Order as follows:
     1. `System.*` namespaces
     2. External library namespaces
     3. AsepriteDotNet namespaces
   - Example:

     ```csharp
     using System;
     using System.Collections.Generic;
     using System.Drawing;
     using System.IO;

     using AsepriteDotNet.Types;
     using AsepriteDotNet.IO;
     ```

3. **Type References**:
   - Use language keywords instead of BCL types (e.g., `int, string, float` instead of `Int32, String, Single`)
   - This applies to both type references and method calls (e.g., `int.Parse` instead of `Int32.Parse`)

4. **Variable Declarations**:
   - Highly discourage the use of `var`. C# is a strongly typed language, and using explicit types improves code readability.
   - Use `var` only when absolutely necessary, not as a general practice.
   - Target-typed `new()` can only be used when the type is explicitly named on the left-hand side (e.g., `AsepriteFile file = new()`)

5. **String References**: Use `nameof(...)` instead of `"..."` whenever possible and relevant.

## Code Organization

### File and Class Organization

- **File Layout**: One type per file with matching filename
- **Fields**: Fields should be specified at the top within type declarations
- **Member Ordering**:
  - Fields (private, then protected, then public)
  - Properties
  - Constructors
  - Methods (grouped by functionality)
  - Nested types

## Documentation

- **Public APIs**: All public APIs must have XML documentation comments following .NET conventions
- **Parameter Documentation**: Document all parameters with `<param>` tags, focusing on constraints and expected values
- **Return Value**: Document return values with `<returns>` tags, including value ranges and meanings
- **Exceptions**: Document exceptions with `<exception>` tags

## Error Handling

- **Validation**:
  - Validate parameters using `ArgumentNullException.ThrowIfNull` and similar methods where available
- **File Format Errors**: Use `InvalidDataException` for corrupt or invalid Aseprite file data
- **I/O Errors**: Let I/O exceptions bubble up naturally unless specific handling is required
- **Avoid Swallowing Exceptions**: Do not catch exceptions without handling or re-throwing

## Implementation Examples

### Extension Methods

```csharp
// CORRECT: Dedicated extension class for AsepriteFile
public static class AsepriteFileExtensions
{
    /// <summary>
    /// Renders the specified frame as an array of color values.
    /// </summary>
    /// <param name="file">The Aseprite file to render from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to render.</param>
    /// <returns>A span of RGBA color values representing the rendered frame.</returns>
    public static ReadOnlySpan<Rgba32> RenderFrame(this AsepriteFile file, int frameIndex)
    {
        // Implementation
    }
}
```

### Conversion Methods

```csharp
// CORRECT: Factory method on target type
public struct Rgba32
{
    /// <summary>
    /// Creates an <see cref="Rgba32"/> from the specified <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>An <see cref="Rgba32"/> representation of the color.</returns>
    public static Rgba32 FromColor(Color color)
    {
        return new Rgba32(color.R, color.G, color.B, color.A);
    }
}

// CORRECT: Extension method on source type
public static class ColorExtensions
{
    /// <summary>
    /// Converts this <see cref="Color"/> to an <see cref="Rgba32"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>An <see cref="Rgba32"/> representation of the color.</returns>
    public static Rgba32 ToRgba32(this Color color)
    {
        return Rgba32.FromColor(color);
    }
}
```

### File I/O Operations

```csharp
// CORRECT: Clear separation of concerns for file operations
public static class AsepriteFileLoader
{
    /// <summary>
    /// Loads an Aseprite file from the specified file path.
    /// </summary>
    /// <param name="path">The path to the Aseprite file.</param>
    /// <returns>An <see cref="AsepriteFile"/> containing the loaded data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is <see langword="null"/>.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    /// <exception cref="InvalidDataException">Thrown when the file is not a valid Aseprite file.</exception>
    public static AsepriteFile FromFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        string fileName = Path.GetFileNameWithoutExtension(path);
        using FileStream stream = File.OpenRead(path);
        return FromStream(fileName, stream);
    }
}
```

## Example File Structure

Below is an example following our style guidelines:

**AsepriteImageCel.cs:**

```csharp
using System;
using System.Drawing;

namespace AsepriteDotNet.Types
{
    /// <summary>
    /// Defines the properties of a cel in an Aseprite file that contains image data.
    /// </summary>
    public sealed class AsepriteImageCel : AsepriteCel
    {
        private readonly Rgba32[] _pixels;

        /// <summary>
        /// Gets the size of this image cel in pixels.
        /// </summary>
        public Size Size { get; internal set; }

        /// <summary>
        /// Gets the collection of color data that represents the pixels of this image cel.
        /// Pixels are ordered left-to-right, top-to-bottom.
        /// </summary>
        public ReadOnlySpan<Rgba32> Pixels => _pixels;

        internal AsepriteImageCel()
        {
            _pixels = Array.Empty<Rgba32>();
        }

        internal AsepriteImageCel(Rgba32[] pixels)
        {
            ArgumentNullException.ThrowIfNull(pixels);
            _pixels = pixels;
        }

        /// <summary>
        /// Creates a copy of this cel's pixel data.
        /// </summary>
        /// <returns>A new array containing the pixel data.</returns>
        public Rgba32[] ToArray()
        {
            Rgba32[] result = new Rgba32[_pixels.Length];
            _pixels.AsSpan().CopyTo(result);
            return result;
        }
    }
}
```

## Aseprite-Specific Considerations

- **File Format Compliance**: All implementations should strictly follow the official Aseprite file format specification
- **Performance**: Consider memory usage when working with large sprite files and pixel data
- **Color Space**: Maintain color accuracy throughout all operations, especially in blend modes
- **Backward Compatibility**: Support multiple versions of the Aseprite file format where possible
- **Resource Management**: Properly dispose of streams and other resources, especially in file I/O operations
