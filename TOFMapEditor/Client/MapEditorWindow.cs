﻿using System;
using System.Threading;
using GlLib.Client.Api.Cameras;
using GlLib.Client.Api.Gui;
using GlLib.Client.Graphic;
using GlLib.Client.Input;
using GlLib.Common;
using GlLib.Common.Entities;
using GlLib.Common.Io;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Utils.Math;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace TOFMapEditor.Client
{
    public class MapEditorWindow : GameWindow
    {
        public ICamera camera;
        public Entity cameraEntity;
        public GuiFrame guiFrame;
        public int guiTimeout = 0;

        public GuiFrame hud;
        public VSyncMode vSync = VSyncMode.On;

        public MapEditorWindow(int _width, int _height, string _title)
            : base(_width, _height, GraphicsMode.Default, _title)
        {
            EditWorld = new World("Overworld", 1);

            var registry = new GameRegistry();
            registry.Load();
            Proxy.RegisterRegistry(registry);

            WorldManager.LoadWorld(EditWorld);

            WorldRenderer = new WorldRenderer(EditWorld);
            MouseHandler.Setup();
            SidedConsole.WriteLine("Window constructed");
            hud = new MapEditorHud();
            cameraEntity = new Entity(EditWorld, new RestrictedVector3D(0));
            EditWorld.SpawnEntity(cameraEntity);
            camera = new EntityTrackingCamera(cameraEntity);
        }

        private World EditWorld { get; }
        private WorldRenderer WorldRenderer { get; }

        protected override void OnUpdateFrame(FrameEventArgs _e)
        {
            //SidedConsole.WriteLine(EditWorld.jsonObj);
            MouseHandler.Update();
            KeyboardHandler.Update();
            hud.Update(this);
            WorldRenderer.Render(cameraEntity.Position.x, cameraEntity.Position.y);

            SidedConsole.WriteLine(cameraEntity.Position);
            OnRenderFrame(_e);


            base.OnUpdateFrame(_e);
        }

        protected override void OnKeyPress(KeyPressEventArgs _e)
        {
            guiFrame?.OnKeyTyped(this, _e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs _e)
        {
            SidedConsole.WriteLine(_e.Key);
            switch (_e.Key)
            {
                case Key.W:
                {
                    cameraEntity.Position += new PlanarVector(1);
                    break;
                    ;
                }

                case Key.S:
                {
                    cameraEntity.Position += new PlanarVector(-1);
                    break;
                }

                case Key.A:
                {
                    cameraEntity.Position += new PlanarVector(0, 1);
                    break;
                }

                case Key.D:
                {
                    cameraEntity.Position += new PlanarVector(0, -1);
                    break;
                }
            }

            base.OnKeyDown(_e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs _e)
        {
            base.OnKeyUp(_e);
            KeyboardHandler.SetPressed(_e.Key, false);
        }

        protected override void OnResize(EventArgs _e)
        {
            base.OnResize(_e);
            GL.Viewport(0, 0, Width, Height);
            guiFrame?.Update(this);
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);
            VSync = VSyncMode.On;
        }

        protected override void OnMouseDown(MouseButtonEventArgs _e)
        {
            base.OnMouseDown(_e);
            guiFrame?.OnMouseClick(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnMouseMove(MouseMoveEventArgs _e)
        {
            base.OnMouseMove(_e);
            if ((bool) MouseHandler.pressed[MouseButton.Left])
                guiFrame?.OnMouseDrag(this, _e.X, _e.Y, _e.XDelta, _e.YDelta);
        }

        protected override void OnMouseUp(MouseButtonEventArgs _e)
        {
            base.OnMouseUp(_e);
            guiFrame?.OnMouseRelease(this, _e.Button, _e.X, _e.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            base.OnRenderFrame(_e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.Clear(ClearBufferMask.DepthBufferBit);
            //GUI render is not connected to the world
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);
            GL.PushMatrix();
            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            hud.Update(this);
            hud.Render(this);


            RenderWorld();

            guiFrame?.Update(this);
            guiFrame?.Render(this);
            GL.PopMatrix();
            GL.Disable(EnableCap.Blend);

            SwapBuffers();
        }


        public void RenderWorld()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 1.0, 0.0, -4.0, 4.0);

            GL.Scale(1d / Width, 1d / Height, 1);

            Vertexer.EnableTextures();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.PushMatrix();
            GL.Translate(Width / 2d, Height / 2d, 0);
//            camera.Update(this);
//            camera.PerformTranslation(this);
            WorldRenderer.Render(000, 000);
            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
        }

        protected override void OnUnload(EventArgs _e)
        {
            foreach (var key in Vertexer.textures.Keys) Vertexer.textures[key].Dispose();

            base.OnUnload(_e);
        }

        public static void RunWindow()
        {
            var graphicThread = new Thread(()
                    => new MapEditorWindow(800, 600, "Tracing of F")
                        .Run(60))
                {Name = Side.Graphics.ToString()};
            graphicThread.Start();
        }
    }
}