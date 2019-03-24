using DuckGame;
using ogtdglib;
using TMGmod.Core.AmmoTypes;

namespace TMGmod.NY
{
    // ReSharper disable once InconsistentNaming
    public class ATSneg : BaseAmmoTypeT
    {
        public readonly SpriteMap SpriteY;
        public ATSneg()
        {
            bulletType = typeof(SnowBullet);
            SpriteY = new SpriteMap(new Nothing(Core.TMGmod.LastInstance).GetPath("Holiday/snow"), 6, 5);
            SpriteY.CenterOrigin();
            sprite = SpriteY;
            bulletSpeed = 5f;
            range = 500f;
            accuracy = 0.95f;
            bulletLength = 3f;
            affectedByGravity = true;
            barrelAngleDegrees = -13.5f;
        }
    }
}
