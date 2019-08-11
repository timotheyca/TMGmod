﻿using DuckGame;

// ReSharper disable once CheckNamespace
namespace TMGmod.src
{
    [EditorGroup("TMG|Pistol")]
    public class BigShot : Gun
    {
        public BigShot (float xval, float yval)
          : base(xval, yval)
        {
            ammo = 7;
            _ammoType = new ATMagnum {range = 250f, accuracy = 1f, penetration = 2f, bulletThickness = 3f};
            _numBulletsPerFire = 4;
            _type = "gun";
            _graphic = new Sprite(GetPath("pistol50"));
            _center = new Vec2(10f, 5f);
            _collisionOffset = new Vec2(-11.5f, -5f);
            _collisionSize = new Vec2(23f, 11f);
            _barrelOffsetTL = new Vec2(25f, 2f);
            _fireSound = "magnum";
            _fullAuto = false;
            _fireWait = 1.6f;
            _kickForce = 1f;
            loseAccuracy = 0f;
            maxAccuracyLost = 0f;
            _holdOffset = new Vec2(2f, 0f);
            _editorName = "Pistol .50";
			_weight = 2.5f;
        }


    }
}
