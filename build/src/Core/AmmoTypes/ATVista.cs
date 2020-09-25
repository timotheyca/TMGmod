using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATVista : BaseAmmoType
    {
        public ATVista()
        {
            penetration = 0.4f;
            bulletSpeed = 37f;
            deadly = true;
            bulletThickness = 0.8f;
            bulletLength = 6f;
            immediatelyDeadly = true;
            BulletDamage = 24f;
            DeltaDamage = 0.33f;
            AlphaDamage = 0.33f;
            DistanceConvexity = -2f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT9mmShell(x, y)
            {
                hSpeed = (1.5f + Rando.Float(-0.1f, 0.1f)) * dir,
                vSpeed = -3f + Rando.Float(-0.4f, 0.4f)
            };
            Level.Add(shell);
        }
    }
}