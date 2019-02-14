using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class Vertexer
    {
        public static void EnableTextures()
        {
            GL.Enable(EnableCap.Texture2D);
        }
        public static void DisableTextures()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        public static void StartDrawingQuads()
        {
            GL.Begin(PrimitiveType.Quads);
        }
        public static void StartDrawing(PrimitiveType type)
        {
            GL.Begin(type);
        }

        public static void VertexAt(double x,double y)
        {
            GL.Vertex2(x,y);
        }
        public static void VertexAt(double x,double y,double z)
        {
            GL.Vertex3(x,y,z);
        }
        
        public static void VertexWithUvAt(double x,double y,double u,double v)
        {
            GL.TexCoord2(u,v);
            GL.Vertex2(x,y);
        }
        public static void VertexWithUvAt(double x,double y,double z,double u,double v)
        {
            GL.TexCoord2(u,v);
            GL.Vertex3(x,y,z);
        }

        public static void Draw()
        {
            GL.End();
        }

        public static Texture LoadTexture(string path)
        {
            if (_textures.ContainsKey(path))
                return _textures[path];
            var texture = new Texture("textures/"+path);
            _textures.Add(path,texture);
            return texture;
        }

        public static void DrawTexturedModalRect(Texture texture,double x,double y, double u, double v, double width, double height)
        {
            GL.PushMatrix();
            texture.Bind();
            double textureLeft = x;
            double textureUp = y;
            double textureRight = x + width;
            double textureDown = y + height;

            double uvLeft = u / texture._width;
            double uvUp = v / texture._height;
            double uvRight= (u+width) / texture._width;
            double uvDown = (v+height)/ texture._height;
            
            StartDrawingQuads();
            
            VertexWithUvAt(textureLeft,textureUp,uvLeft,uvUp);
            VertexWithUvAt(textureRight,textureUp,uvRight,uvUp);
            VertexWithUvAt(textureRight,textureDown,uvRight,uvDown);
            VertexWithUvAt(textureLeft,textureDown,uvLeft,uvDown);
            
            Draw();
            GL.PopMatrix();
        }

        public static Dictionary<string,Texture> _textures = new Dictionary<string,Texture>();
        
        public static void BindTexture(Texture text)
        {
            text.Bind();
        }
    }
}