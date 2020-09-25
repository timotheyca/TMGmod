using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATIB8 : BaseAmmoType
    {
        public ATIB8()
        {
            range = 450f;
            accuracy = 0.82f;
            penetration = 0.4f;
            bulletSpeed = 30f;
            deadly = true;
            bulletThickness = 0.8f;
            bulletLength = 0f;
            immediatelyDeadly = true;
            BulletDamage = 29f;
            DeltaDamage = 0.2f;
            AlphaDamage = 0.7f;
            DistanceConvexity = -0.5f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT762NATOShell(x, y) //AT9mmParabellumShell
            {
                hSpeed = (3f + Rando.Float(-0.1f, 0.1f)) * dir,
                vSpeed = -2.25f + Rando.Float(-0.4f, 0.4f)
            };
            Level.Add(shell);
        }
    }
}