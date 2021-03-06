﻿using System;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities.Items
{
    public class Box : EntityLiving
    {
        public Box()
        {
            MaxHealth = 1;
            Health = 800;
            SetCustomRenderer(new BoxRenderer());
        }

        public Box(World _world, RestrictedVector3D _position, uint _health = 800,
            ushort _armor = 1) : base(_health, _armor, _world, _position)
        {
            SetCustomRenderer(new BoxRenderer());
            AaBb = new AxisAlignedBb(-1.5f / 4f, -0.75f / 4f, 1.5f / 4f, 3 / 4f);
        }

        private void Initialize()
        {
            maxVel = new PlanarVector();
        }

        public override void OnDead()
        {
            if (Proxy.GetWindow().serverStarted)
            {
                var r = new Random();
                if (r.Next(10) > 4)
                {
                    worldObj.SpawnEntity(new Coin(worldObj, Position,
                        PlanarVector.GetRandom(0.7f)));
                    if (r.Next(10) > 7)
                        worldObj.SpawnEntity(new Potion(worldObj, Position,
                            PlanarVector.GetRandom(0.7f)));
                }
                else
                {
                    worldObj.SpawnEntity(new EntitySlime(
                        worldObj, Position));
                }
            }
        }

        public override void OnCollideWith(Entity _obj)
        {
            _obj.velocity /= 2;
            velocity += _obj.velocity;
            CheckVelocity();
        }

        public void CheckVelocity()
        {
            if (Math.Abs(velocity.x) > maxVel.x) velocity.x *= maxVel.x / Math.Abs(velocity.x);
            if (Math.Abs(velocity.y) > maxVel.y) velocity.y *= maxVel.y / Math.Abs(velocity.y);
        }

        public override string GetName()
        {
            return "entity.box";
        }
    }
}