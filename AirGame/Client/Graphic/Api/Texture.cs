using System;
using System.Collections.Generic;
using GlLib.Common.Io;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

namespace GlLib.Client.Graphic
{
    public class Texture : IDisposable
    {
        private readonly int _handle;
        private bool _disposedValue;
        public int height;

        public int width;

        //Create texture from path.
        public Texture(string _path)
        {
            //Generate handle
            _handle = GL.GenTexture();

            //Bind the handle
            Bind();

            //Load the image
            var image = Image.Load(_path);
            width = image.Width;
            height = image.Height;
            SidedConsole.WriteLine($"Texture: path: {_path}, width: {width}, height: {height}");
            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            //            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Get an array of the pixels, in ImageSharp's internal format.
            var tempPixels = image.GetPixelSpan().ToArray();

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
            var pixels = new List<byte>();

            foreach (var p in tempPixels)
            {
                pixels.Add(p.R);
                pixels.Add(p.G);
                pixels.Add(p.B);
                pixels.Add(p.A);
            }

            //Now that have our pixels, we need to set a few settings.
            //If you don't include these settings, OpenTK will refuse to draw the texture.

            //First, we set the min and mag filter. These are used for when the texture is scaled down and up, respectively.
            //Here, we use Linear for both. This means that OpenGL will try to blend pixels, meaning that textures scaled too far will look blurred.
            //You could also use (amongst other options) Nearest, which just grabs the nearest pixel, which makes the texture look pixelated if scaled too far.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);


            //Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);

            //Now that our pixels have been loaded and our settings are prepared, it's time to generate a texture. We do this with GL.TexImage2D
            //Arguments:
            //  The type of texture we're generating. There are various different types of textures, but the only one we need right now is Texture2D.
            //  Level of detail. We can use this to start from a smaller mipmap (if we want), but we don't need to do that, so leave it at 0.
            //  Target format of the pixels.
            //  Width of the image
            //  Height of the image.
            //  Border of the image. This must always be 0; it's a legacy parameter that Khronos never got rid of.
            //  The format of the pixels, explained above.
            //  Data type of the pixels.
            //  And finally, the actual pixels.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());


            //Next, generate mipmaps.
            //Mipmaps are smaller copies of the texture, scaled down. Each mipmap level is half the size of the previous one
            //Generated mipmaps go all the way down to just one pixel.
            //OpenGL will automatically switch between mipmaps when an object gets sufficiently far away.
            //This prevents distant objects from having their colors become muddy, as well as saving on memory.
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //Activate texture
        //Multiple textures can be bound, if your shader needs more than just one.
        //If you want to do that, use GL.ActiveTexture to set which slot GL.BindTexture binds to.
        //The OpenGL standard requires that there be at least 16, but there can be more depending on your graphics card.
        public void Bind(TextureUnit _unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(_unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        protected virtual void Dispose(bool _disposing)
        {
            if (!_disposedValue)
            {
                if (_disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                GL.DeleteProgram(_handle);

                _disposedValue = true;
            }
        }


        // Check the main tests.
        // There is something happened that break the app in Windows OS, without any exceptions 


        //~Texture()
        //{
        //    if (!_disposedValue)
        //    {
        //        GL.DeleteProgram(_handle);
        //    }
        //}
    }
}