/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a layer in an Aseprite image.
/// </summary>
public abstract class Layer : IUserData
{
    /// <summary>
    ///     Gets whether this <see cref="Layer"/> is visible.
    /// </summary>
    public bool IsVisible { get; internal set; }

    /// <summary>
    ///     Gets whether this <see cref="Layer"/> is a background layer.
    /// </summary>
    public bool IsBackgroundLayer { get; internal set; }

    /// <summary>
    ///     Gets whether this <see cref="Layer"/> is a reference layer.
    /// </summary>
    public bool IsReferenceLayer { get; internal set; }

    /// <summary>
    ///     Gets the child level of this <see cref="Layer"/> relative to the
    ///     previous <see cref="Layer"/>.
    /// </summary>
    public virtual int ChildLevel { get; internal set; }

    /// <summary>
    ///     Gets the <see cref="BlendMode"/> used by this <see cref="Layer"/>.
    /// </summary>
    public BlendMode BlendMode { get; internal set; }

    /// <summary>
    ///     Gets the opacity level of this <see cref="Layer"/>.
    /// </summary>
    public int Opacity { get; internal set; }

    /// <summary>
    ///     Gets the name of this <see cref="Layer"/>.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the <see cref="UserData"/> of this <see cref="Layer"/>.
    /// </summary>
    public UserData UserData { get; internal set; } = new();
}