﻿#if DEBUG
using System;
using System.Globalization;
using DuckGame;
using JetBrains.Annotations;
using TMGmod.Core.AmmoTypes;

namespace TMGmod.Buddies
{
    [EditorGroup("TMG|Misc")]
    [UsedImplicitly]
    public class HpArmor : Equipment
    {
        public override Vec2 collisionSize
        {
            get => _equippedDuck?.collisionSize ?? _collisionSize;
            set => _collisionSize = value;
        }

        public override Vec2 collisionOffset
        {
            get => _equippedDuck?.collisionOffset ?? _collisionOffset;
            set => _collisionOffset = value;
        }

        public override float top
        {
            get => -1.0f + _equippedDuck?.top ?? base.top;
            set => base.top = value;
        }

        public override float left
        {
            get => -1.0f +  _equippedDuck?.left ?? base.left;
            set => base.left = value;
        }

        public override float bottom
        {
            get => +1.0f + _equippedDuck?.bottom ?? base.bottom;
            set => base.bottom = value;
        }

        public override float right
        {
            get => +1.0f + _equippedDuck?.right ?? base.right;
            set => base.right = value;
        }

        private const float HpMax = 99f;

        public HpArmor(float xpos, float ypos) : base(xpos, ypos)
        {
            _hitPoints = HpMax;
            _collisionOffset = new Vec2(-6f, -4f);
            _collisionSize = new Vec2(11f, 8f);
            _equippedCollisionOffset = new Vec2(-6f, -11f);
            _equippedCollisionSize = new Vec2(12f, 22f);
            _hasEquippedCollision = true;
            _center = new Vec2(8f, 8f);
            physicsMaterial = PhysicsMaterial.Duck;
            _equippedDepth = 2;
            _wearOffset = new Vec2(0, 0);
            _isArmor = true;
            _equippedThickness = 666f;
            canPickUp = false;
        }

        private bool QHit(Bullet bullet)
        {
            var hit = bullet.end;
            var v = bullet.bulletSpeed * bullet.travelDirNormalized;
            var u = new Vec2(v.y, -v.x).normalized;
            
            return Vec2.Dot(u, (_equippedDuck.topLeft - hit).normalized) * Vec2.Dot(u, (_equippedDuck.bottomRight - hit).normalized) < 0.1f &&
                   Vec2.Dot(u, (_equippedDuck.topRight - hit).normalized) * Vec2.Dot(u, (_equippedDuck.bottomLeft - hit).normalized) < 0.1f;
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            DotMarker.Show(bullet.end);
            if (_equippedDuck == null || bullet.owner == _equippedDuck || !bullet.isLocal)
                return false;
            if (!QHit(bullet))
                return false;
            _equippedDuck.hSpeed *= 0.25f;
            _hitPoints -= Damage.Calculate(bullet);
            if (_hitPoints < 0)
            {
                Mod.Debug.Log("destroyed");
                var equippedDuck1 = _equippedDuck;
                equippedDuck1.KnockOffEquipment(this, true, bullet);
                Fondle(this, DuckNetwork.localConnection);
                equippedDuck1.Destroy(new DTShot(bullet));
                //kill owner
            }
            if (bullet.isLocal && Network.isActive)
                _netTing.Play();
            Level.Add(MetalRebound.New(hitPos.x, hitPos.y, bullet.travelDirNormalized.x > 0 ? 1 : -1));
            for (var index = 0; index < 6; ++index)
                Level.Add(Spark.New(x, y, bullet.travelDirNormalized));
            return true;
        }

        public override void Draw()
        {
#if DEBUG
            Graphics.DrawRect(rectangle, new Color(255, 0, 0, 128));
#endif
            if (_equippedDuck == null) return;
            var start = (_equippedDuck.topLeft + _equippedDuck.topRight) / 2 + new Vec2(-32, 0);
            Graphics.DrawRect(start, start + new Vec2(64, -8), Color.Red, 0.0f);
            Graphics.DrawRect(start, start + new Vec2(0, -8) + new Vec2(64, 0) * Math.Max(_hitPoints / HpMax, 0), Color.Green, 0.1f);
#if DEBUG
            Graphics.DrawString(_hitPoints.ToString(CultureInfo.InvariantCulture), start + new Vec2(64, -8), Color.GreenYellow);
            Graphics.DrawRect(_equippedDuck.rectangle, new Color(0, 0, 255, 128));
#endif
        }

        public override bool Destroy(DestroyType type1 = null)
        {
            return base.Destroy(type1);
        }

        public override void Update()
        {
            base.Update();
            if (_equippedDuck is null)
            {
                Level.Remove(this);
                return;
            }

            _hitPoints = Math.Min(_hitPoints, HpMax * Math.Max(0.1f, 2 * (1 - _equippedDuck.burnt)));
        }
    }
}
#endif