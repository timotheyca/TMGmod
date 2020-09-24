using System;
using System.Globalization;
using DuckGame;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public interface IDamage
    {
        float BulletDamage { get; }
        float DeltaDamage{ get; }
        float DistanceConvexity { get; }
        float AlphaDamage { get; }
    }
    public static class Damage
    {
        private static float GetDamage(AmmoType ammo)
        {
            return ammo is IDamage damage ? damage.BulletDamage : ammo is ATShrapnel ? 30f : 50f;
        }

        private static float GetDelta(AmmoType ammo)
        {
            return ammo is IDamage damage ? damage.DeltaDamage : 1f;
        }

        private static float GetConvexity(AmmoType ammo)
        {
            return ammo is IDamage damage ? damage.DistanceConvexity : 0f;
        }

        private static float GetAlpha(AmmoType ammo)
        {
            return ammo is IDamage damage ? damage.AlphaDamage: 0f;
        }

        private static float CalculateBase(AmmoType ammo)
        {
            var delta = GetDelta(ammo);
            return Rando.Float(1 - delta, 1 + delta) * GetDamage(ammo);
        }

        private static float CalculateCoeff(Bullet bullet)
        {
            var z = GetConvexity(bullet.ammo);
            var q = bullet.bulletDistance / Math.Max(1, bullet.ammo.range);
            q = Math.Min(1f, q);
            q = Math.Max(0f, q);
            var x = .5 + q * .9142135623730951;
            var r = .2857142857142857 / x / x - .14285714285714285;
            var s = 1 - q * q;
            var rc = Math.Exp(+z);
            var sc = Math.Exp(-z);
            var c = (rc * r + sc * s) / (rc + sc);
            c = Math.Min(1f, c);
            c = Math.Max(0f, c);
            return (float) c;
        }
        public static float Calculate(Bullet bullet)
        {
            Level.Add(new DamageDealt(bullet.end.x, bullet.end.y, CalculateCoeff(bullet)));
            return CalculateBase(bullet.ammo) * CalculateCoeff(bullet);
        }
    }

    public class DamageDealt : Thing
    {
        private readonly float _d;
        private float _alive = 1f;

        public DamageDealt(float x, float y, float d): base(x, y)
        {
            _d = d;
            _hSpeed = Rando.Float(-1f, 1f);
            _vSpeed = Rando.Float(-1f, 1f);
        }

        public override void Update()
        {
            x += hSpeed;
            y += vSpeed;
            _alive -= 0.05f;
            if (_alive < 0) Level.Remove(this);
        }

        public override void Draw()
        {
            Graphics.DrawString(_d.ToString(CultureInfo.InvariantCulture), position, new Color(Rando.Int(192, 255), Rando.Int(0, 63), 0, 128), 2f);
        }
    }
}