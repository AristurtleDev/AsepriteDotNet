// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core;
using AsepriteDotNet.Core.IO;
using AsepriteDotNet.Core.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Load the file. In this example, we're not using the MGCB/Content Pipeline and have the Aseprite file set as
        /// a file in our project that is copied the output directory.  Because of this, we can use the
        /// TitleContainer.OpenStream to get a stream to the file and use that to load it.
        ///
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        AsepriteFile aseFile;
        using (Stream stream = TitleContainer.OpenStream("adventurer.aseprite"))
        {
            aseFile = AsepriteFileLoader.FromStream("adventurer", stream);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Flatten a frame so that we can get the full color data of that frame.
        ///
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Rgba32[] frame0Pixels = aseFile.Frames[0].FlattenFrame();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create the texture
        ///
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _texture = new Texture2D(GraphicsDevice, aseFile.Frames[0].Size.Width, aseFile.Frames[0].Size.Height);


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// AsepriteDotNet internally uses it's own Rgba32 color struct to represent color data. This struct is
        /// compatible with MonoGame's Color struct when setting texture data so we can use it directly without
        /// needing to convert back and forth between the two.
        ///
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _texture.SetData<Rgba32>(frame0Pixels);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_texture, new Vector2(100, 100), null, Color.White, 0.0f, Vector2.Zero, 5.0f, SpriteEffects.None, 0.0f);
        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
