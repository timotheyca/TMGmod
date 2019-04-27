﻿using DuckGame;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [EditorGroup("TMG|Sniper")]
    // ReSharper disable once InconsistentNaming
    public class SVU : BaseGun, IAmDmr
    {
        public SVU (float xval, float yval)
          : base(xval, yval)
        {
            ammo = 10;
            _ammoType = new ATMagnum
            {
                range = 580f,
                accuracy = 0.91f,
                penetration = 1.5f
            };
            _type = "gun";
            _graphic = new Sprite(GetPath("SVU"));
            _center = new Vec2(20f, 8f);
            _collisionOffset = new Vec2(-14.5f, -8f);
            _collisionSize = new Vec2(31f, 11f);
            _barrelOffsetTL = new Vec2(31f, 5f);
            _fireSound = GetPath("sounds/HeavyRifle.wav");
            _fullAuto = true;
            _fireWait = 0.75f;
            _kickForce = 1.8f;
            loseAccuracy = 0.05f;
            maxAccuracyLost = 0.45f;
            _holdOffset = new Vec2(1f, 2f);
            _editorName = "SVU";
			_weight = 5f;
        }
    }
}