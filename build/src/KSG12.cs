using DuckGame;
using JetBrains.Annotations;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [UsedImplicitly]
    [EditorGroup("TMG|Shotgun|Pump-Action")]
    public class Ksg12 : BasePumpAction
    {
	    public Ksg12(float xval, float yval)
		    : base(xval, yval)
	    {
		    ammo = 15;
	        _ammoType = new AT9mm
	        {
	            range = 185f,
	            accuracy = 0.4f,
	            penetration = 1f,
	            bulletSpeed = 40f,
	            bulletThickness = 0.25f
	        };
            _numBulletsPerFire = 8;
            _type = "gun";
		    _graphic = new Sprite(GetPath("KSG12"));
		    _center = new Vec2(18f, 5.5f);
		    _collisionOffset = new Vec2(-18f, -5.5f);
		    _collisionSize = new Vec2(36f, 11f);
		    _barrelOffsetTL = new Vec2(36f, 3f);
            _holdOffset = new Vec2(-1f, 0f);
            ShellOffset = new Vec2(0f, 0f);
            _fireSound = "shotgunFire2";
		    _kickForce = 3.75f;
		    _manualLoad = true;
            _fireWait = 2.5f;
            LoaderSprite = new SpriteMap(GetPath("KSG12Pimp"), 19, 9)
            {
                center = new Vec2(3f, 4f)
            };
            _editorName = "KSG-12";
            LoaderVec2 = new Vec2(2f, 1f);
	        EpsilonA = 50;
	        EpsilonB = 100;
	        Loaddx = 2f;
	    }
    }
}