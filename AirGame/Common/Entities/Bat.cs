﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    internal class Bat : EntityLiving
    {

        internal long InternalTime
            => DateTime.Now.Ticks - _spawnTime;

        private const int UpdateFrame = 12;
        private long _spawnTime;

        public Bat()
        {
            Initialize();
        }

        public Bat(World _world, RestrictedVector3D _position) : base(100, 2, _world, _position)
        {
            SetCustomRenderer(new BatRenderer());
            Initialize();
        }


        private void Initialize()
        {
            //TODO move to server time
            _spawnTime = DateTime.Now.Ticks;
            SetCustomRenderer(new BatRenderer());
            AttackRange = 7;
            AttackValue = 5;
        }

        public override string GetName()
        {
            return "entity.living.bat";
        }

        public Bat(bool _inMove, bool _inWaiting, int _attackRange, long _spawnTime)
        {
            InMove = _inMove;
            InWaiting = _inWaiting;
            this._spawnTime = _spawnTime;
            Target = null;
            Initialize();
        }

        private Player Target { get; set; }
        public bool InMove { get; }
        public bool InWaiting { get; }
        public int AttackRange { get; private set; }

        public bool IsAttacking
        {
            get => !(Target is null);
        }

        public bool IsWaiting
        {
            get => (Target is null);
        }

        //public override Mov
        public override void Update()
        {
            var entities = worldObj.GetEntitiesWithinAaBb(Position.ExpandBothTo(AttackRange, AttackRange));

            if (Target is null && !(entities is null))
            {
                Target = (Player) entities
                    .FirstOrDefault(_e => _e is Player);
            }

            if (InternalTime % UpdateFrame == 0 ||
                (!(Target is null) && Target.IsDead))
            {
                Target = (Player) entities
                    .FirstOrDefault(_e => _e is Player);

                if (!(Target is null) &&
                    (Target.Position - position).Length > 1)
                    MoveToTarget();
            }

            base.Update();
        }

        public override void OnCollideWith(Entity _obj)
        {
            if (_obj is EntityLiving
                && !(_obj is Bat)
                && InternalTime % UpdateFrame == 0
                && InternalTime > 30000000)
                (_obj as EntityLiving).DealDamage(AttackValue);
        }

        private void MoveToTarget()
        {
            velocity = Target.Position - position;
            velocity.Normalize();
            velocity /= 5;
        }

        public int AttackValue { get; set; }
    }
}

