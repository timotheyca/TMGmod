using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class AT762NATO : BaseAmmoType
    {
        public AT762NATO()
        {
            penetration = 2f;
            bulletSpeed = 29f;
            deadly = true;
            bulletThickness = 1.5f;
            bulletLength = 40f;
            immediatelyDeadly = true;
            BulletDamage = 40f;
            DeltaDamage = 0.2f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT762NATOShell(x, y)
            {
                hSpeed = (3f + Rando.Float(-0.1f, 0.1f)) * dir,
                vSpeed = 2.25f + Rando.Float(-0.4f, 0.4f)
            };
            Level.Add(shell);
        }
    }
}