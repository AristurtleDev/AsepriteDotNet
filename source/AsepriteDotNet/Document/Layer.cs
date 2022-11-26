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

public abstract class Layer : IUserData
{
    private int _opacity;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Layer"/> is visible.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Layer"/> is editable.
    /// </summary>
    public bool IsEditable { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     movement of this <see cref="Layer"/> is locked in the Aseprite UI.
    /// </summary>
    public bool IsMovementLocked { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Layer"/> is the background layer.
    /// </summary>
    public bool IsBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicats whether this
    ///     <see cref="Layer"/> perfers linked cels.
    /// </summary>
    public bool PrefersLinkedCels { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Layer"/> is displed collapsed in the Asperite UI.
    /// </summary>
    public bool IsDisplayedCollapsed { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Layer"/> is a reference layer.
    /// </summary>
    public bool IsReferenceLayer { get; set; } = false;
    
    /// <summary>
    ///     Gets the child level of this <see cref="Layer"/>.
    /// </summary>
    public virtual int ChildLevel { get; internal set; } = 0;

    /// <summary>
    ///     Gets or Sets a <see cref="BlendMode"/> value that indicates the
    ///     blend mode used by this layer.
    /// </summary>
    public BlendMode BlendMode { get; set; } = BlendMode.Normal;

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that defines the opacity
    ///     level of this layer.  When set, the value will be clamped in the
    ///     inclusive range of 0 to 255.
    /// </summary>
    public int Opacity
    {
        get => _opacity;
        set
        {
            _opacity = Math.Clamp(value, 0, 255);
        }
    }

    /// <summary>
    ///     Gets or Sets the name of this layer.
    /// </summary>
    public string Name { get; set; } = "Layer";

    public UserData UserData { get; set; } = new();
}