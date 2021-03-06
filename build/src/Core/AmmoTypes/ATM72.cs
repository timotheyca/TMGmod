using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATM72 : AmmoType, IHeavyAmmoType
    {
        public ATM72()
        {
            accuracy = 0.95f;
            penetration = 0.35f;
            bulletSpeed = 15f;
            range = 2000f;
            deadly = true;
            weight = 5f;
            barrelAngleDegrees = -5f;
            bulletThickness = 2f;
            bulletColor = Color.White;
            bulletType = typeof(GrenadeBullet);
            immediatelyDeadly = true;
            sprite = new Sprite("launcherGrenade");
            sprite.CenterOrigin();
        }

        public override void PopShell(float x, float y, int dir)
        {
            var shalker = new M72Shell(x, y) {hSpeed = dir * (3.5f + Rando.Float(1f))};
            Level.Add(shalker);
        }
    }
}