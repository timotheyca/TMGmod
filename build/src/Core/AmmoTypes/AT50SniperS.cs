using DuckGame;
using TMGmod.Core.Shells;

namespace TMGmod.Core.AmmoTypes
{
    // ReSharper disable once InconsistentNaming
    public class AT50SniperS : AmmoType, IDamage
    {
        public AT50SniperS()
        {
            penetration = 1f;
            bulletSpeed = 37f;
            deadly = true;
            bulletThickness = 0f;
            bulletLength = 40f;
            immediatelyDeadly = true;
            Bulletdamage = 106f;
            Deltadamage = 0.07f;
        }
        public override void PopShell(float x, float y, int dir)
        {
            var shell = new AT762NATOShell(x, y) //spinershell
            {
                hSpeed = (3f + Rando.Float(-0.1f, 0.1f)) * dir,
                vSpeed = (2.25f + Rando.Float(-0.4f, 0.4f)) * dir
            };
            Level.Add(shell);
        }
        public float Bulletdamage { get; }
        public float Deltadamage { get; }
    }
}