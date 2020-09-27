using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATVintorez : BaseAmmoType
    {
        public ATVintorez()
        {
            range = 550f;
            accuracy = 0.95f;
            bulletSpeed = 17f;
            penetration = 0.4f;
            deadly = true;
            bulletThickness = 0.8f;
            bulletLength = 0f;
            immediatelyDeadly = true;
            BulletDamage = 35f;
            DeltaDamage = 0.2f;
            AlphaDamage = 0.4f;
            DistanceConvexity = 1.3f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new ATSP6Shell(x, y)
            {
                hSpeed = (3f + Rando.Float(-0.1f, 0.1f)) * dir,
                vSpeed = -2.25f + Rando.Float(-0.4f, 0.4f)
            };
            Level.Add(shell);
        }
    }
}