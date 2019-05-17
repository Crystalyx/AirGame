﻿using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Entities.Items;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
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
        }

        public override void OnDead()
        {
            if (Proxy.GetWindow().serverStarted)
            {
                var r = new Random();
                if (r.Next(10) > 4)
                {
                    worldObj.SpawnEntity(new Coin(worldObj, Position,
                        PlanarVector.GetRandom(0.2)));
                    if (r.Next(10) > 7)
                    {
                        worldObj.SpawnEntity(new Potion(worldObj, Position,
                            PlanarVector.GetRandom(0.2)));
                    }
                }
                else
                    worldObj.SpawnEntity(new EntitySlime(
                        worldObj, Position));
            }
        }

        public override AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(2, 2);
        }

        public override string GetName()
        {
            return "entity.box";
        }
    }
}
