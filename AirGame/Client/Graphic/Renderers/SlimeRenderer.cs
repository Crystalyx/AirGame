using System;
using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class SlimeRenderer : EntityRenderer
    {
        protected LinearSprite idleSprite;
        protected LinearSprite walkSprite;
        protected AlagardFontSprite Text;
        private float start = -4;
        protected Color4 color = Color4.Aquamarine;

        public SlimeRenderer() : base()
        {

        }

        public SlimeRenderer(Color4 _color) : base()
        {
            color = _color;
        }

        public override void Setup(Entity _p)
        {

            Text= new AlagardFontSprite();

            var idle = new TextureLayout("slime/slime_idle.png", 10, 1);
            var walk = new TextureLayout("slime/slime_waiting.png", 7, 1);

            idleSprite = new LinearSprite(idle, 10, 30);
            walkSprite = new LinearSprite(walk, 7, 30);

            idleSprite.SetColor(color);
            walkSprite.SetColor(color);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            switch (_e.state)
            {
                case (EntityState.Idle):
                    idleSprite.Render();
                    break;
                case (EntityState.Walk):
                    walkSprite.Render();
                    break;
            }


            GL.PushMatrix();
            GL.Translate(idleSprite.texture.layout.startU + Math.Sin(start / 2) * 3,
                idleSprite.texture.layout.startV - start,
                0);
            start += 0.1f;
            Text.DrawText("Hello, I'm Slime", 12);
            GL.PopMatrix();
        }
    }
}