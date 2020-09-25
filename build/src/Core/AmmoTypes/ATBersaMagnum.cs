using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATBersaMagnum : BaseAmmoType
    {
        public ATBersaMagnum()
        {
            range = 190f;
            accuracy = 0.76f;
            penetration = 2.1f;
            bulletSpeed = 48f;
            deadly = true;
            bulletThickness = 2f;
            bulletLength = 64f;
            immediatelyDeadly = true;
            BulletDamage = 59f;
            DeltaDamage = 0.55f;
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