using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATCZS : BaseAmmoType
    {
        public ATCZS()
        {
            range = 380f;
            accuracy = 0.95f;
            penetration = 0.4f;
            bulletSpeed = 37f;
            deadly = true;
            bulletThickness = 1f;
            bulletLength = 0f;
            immediatelyDeadly = true;
            BulletDamage = 26f;
            DeltaDamage = 0.1f;
            AlphaDamage = 0.37f;
            DistanceConvexity = 2f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT556NATOShell(x, y)
            {
                hSpeed = (2.5f + Rando.Float(-0.2f, 0.2f)) * dir,
                vSpeed = 2f + Rando.Float(-0.3f, 0.3f)
            };
            Level.Add(shell);
        }
    }
    // ReSharper disable once InconsistentNaming
    public class ATCZS2 : BaseAmmoType
    {
        public ATCZS2()
        {
            range = 350f;
            accuracy = 0.95f;
            penetration = 0.4f;
            bulletSpeed = 37f;
            deadly = true;
            bulletThickness = 1f;
            bulletLength = 0f;
            immediatelyDeadly = true;
            BulletDamage = 25f;
            DeltaDamage = 0.1f;
            AlphaDamage = 0.37f;
            DistanceConvexity = 2f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT556NATOShell(x, y)
            {
                hSpeed = (2.5f + Rando.Float(-0.2f, 0.2f)) * dir,
                vSpeed = 2f + Rando.Float(-0.3f, 0.3f)
            };
            Level.Add(shell);
        }
    }
}