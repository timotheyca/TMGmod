using DuckGame;
using TMGmod.Core.Bullets;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class ATFOUCUS : BaseAmmoType
    {
        public ATFOUCUS()
        {
            range = 405f;
            accuracy = 0.85f;
            penetration = 2.1f;
            bulletSpeed = 38f;
            deadly = true;
            bulletThickness = 1f;
            bulletLength = 0f;
            immediatelyDeadly = true;
            bulletType = typeof(Bullet556);
            BulletDamage = 52f;
            DeltaDamage = 0.05f;
            AlphaDamage = 0.9f;
            DistanceConvexity = 0f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT762NATOShell(x, y)
            {
                hSpeed = (4f + Rando.Float(-0.2f, 0.2f)) * dir,
                vSpeed = 3f + Rando.Float(-0.3f, 0.3f)
            };
            Level.Add(shell);
        }

        public override void WriteAdditionalData(BitBuffer b)
        {
            b.Write(penetration);
        }

        public override void ReadAdditionalData(BitBuffer b)
        {
            penetration = b.ReadFloat();
        }
    }
}