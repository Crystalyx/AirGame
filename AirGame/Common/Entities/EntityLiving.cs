using System;
using System.Net.Json;
using GlLib.Common.Map;
using GlLib.Utils.Math;

namespace GlLib.Common.Entities
{
    public class EntityLiving : Entity
    {
        public const ushort MaxArmor = 100;

        private ushort _armor;

        public EntityLiving(uint _health,
            ushort _armor, World _world,
            RestrictedVector3D _position) : base(_world, _position)
        {
            Armor = _armor;
            Health = _health;
            MaxHealth = _health;

            DamageTimer = -1;
        }

        public EntityLiving()
        {
            Armor = 0;
            Health = 100;
            MaxHealth = 100;
            DamageTimer = 0;
        }

        public bool CanDie { get; set; } = true;

        public float Health { get; protected set; }
        public float MaxHealth { get; protected set; }
        public bool GodMode { get; protected set; }
        public double DamageTimer { get; set; }
        public float LastDamage { get; private set; }

        public ushort Armor
        {
            get => _armor;
            protected set
            {
                if (value < 100) _armor = value;
                else
                    throw new ArgumentException("Armor shouldn't be greater than 100.");
            }
        }

        public void SetGodMode(bool _enable = true)
        {
            GodMode = _enable;
        }

        public override JsonObject Serialize(string _objectName)
        {
            var obj = base.Serialize(_objectName);
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonNumericValue("Armor", Armor));
                collection.Add(new JsonNumericValue("Health", Health));
                collection.Add(new JsonNumericValue("MaxHealth", MaxHealth));
                collection.Add(new JsonStringValue("GodMode", GodMode + ""));
                collection.Add(new JsonNumericValue("IsTakingDamage", DamageTimer));
            }

            return obj;
        }

        public override void Deserialize(JsonObject _jsonObject)
        {
            base.Deserialize(_jsonObject);

            if (_jsonObject is JsonObjectCollection collection)
            {
                Armor = (ushort) ((JsonNumericValue) collection[8]).Value;
                Health = (float) ((JsonNumericValue) collection[9]).Value;
                MaxHealth = (float) ((JsonNumericValue) collection[10]).Value;
                GodMode = ((JsonStringValue) collection[11]).Value == "True";
                DamageTimer = (int) ((JsonNumericValue) collection[12]).Value;
            }
        }

        public override void Update()
        {
            if (Health <= 0 && Math.Abs(DamageTimer) < 1e-3 && CanDie)
                SetDead();
            if (DamageTimer * 16 < 1)
                DamageTimer = 0;
            if (DamageTimer < 0)
                DamageTimer++;

            if (state is EntityState.Dead && Health > 0) SetState(EntityState.Idle, -1, true);
            if (!(state is EntityState.Dead) && Health <= 0) SetState(EntityState.Dead, -1, true);

            if (Health <= 0) return;
            base.Update();
        }

        public virtual void DealDamage(float _damage)
        {
            if (GodMode) return;
            if (!state.Equals(EntityState.Dead))
            {
                SetState(EntityState.AttackInterrupted, 3);
                var takenDamage = _damage * (1 - Armor / (float) MaxArmor);
                if (takenDamage >= Health)
                {
                    Health = 0;
                    DamageTimer = 4;
                    SetState(EntityState.Dead, -1);
//                    SidedConsole.WriteLine("Dead");
                }
                else
                {
                    DamageTimer = 4;
                    Health -= takenDamage;
                }

                LastDamage = takenDamage;

//            SidedConsole.WriteLine("Damage Dealt: " + takenDamage + "; " + Health);
            }
        }

        public virtual void Heal(float _damage)
        {
            Health += _damage;
            DamageTimer = -2;
            if (Health > MaxHealth) Health = MaxHealth;
        }

//        public virtual void PerformAttack()
//        {
//            
//        }

        public override string GetName()
        {
            return "entity.entityLiving";
        }
    }
}