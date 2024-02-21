//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace AsepriteDotNet.IO;

public static partial class AsepriteFileLoader
{
    /// <summary>
    /// Loads the Aseprite file at the specified path.
    /// </summary>
    /// <param name="path">The absolute file path to the Aseprite file to load.</param>
    /// <returns>
    /// A new instance of the <see cref="AsepriteFile"/> class containing the contents of the Aseprite file that was
    /// loaded.
    /// </returns>
    public static AsepriteFile Load(string path)
    {
        string fileName = Path.GetFileNameWithoutExtension(path);
        using FileStream stream = File.OpenRead(path);
        return Load(fileName, stream, true);
    }

    /// <summary>
    /// Attempts to load the Aseprite file at the specified path.
    /// </summary>
    /// <param name="path">The absolute file path to the Aseprite file to load.</param>
    /// <param name="asepriteFile">
    /// When this method returns <see langword="true"/>, contains the contents of the Aseprite file that was loaded;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the Aseprite file was loaded successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryLoad(string path, out AsepriteFile? asepriteFile)
    {
        asepriteFile = null;

        try
        {
            asepriteFile = Load(path);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("The following exception was caught while trying to load the Aseprite file");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Loads an Aseprite file from a given stream.
    /// </summary>
    /// <param name="fileName">The name of the Aseprite file, minus the extension.</param>
    /// <param name="stream">The stream to load the Aseprite file from.</param>
    /// <param name="leaveOpen">
    /// <see langword="true"/> to leave the given <paramref name="stream"/> open after loading the Aseprite file;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="AsepriteFile"/> class containing the contents fo the Aseprite file that was
    /// loaded.
    /// </returns>
    public static AsepriteFile Load(string fileName, Stream stream, bool leaveOpen = false)
    {
        using AsepriteBinaryReader reader = new AsepriteBinaryReader(stream, leaveOpen);
        return LoadFile(fileName, reader);
    }

    public static bool TryLoad(string fileName, Stream stream, bool leaveOpen, out AsepriteFile? asepriteFile)
    {
        asepriteFile = null;

        try
        {
            asepriteFile = Load(fileName, stream, leaveOpen);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("The following exception was caught while trying to load the Aseprite file");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
