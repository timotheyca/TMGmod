﻿using DuckGame;

// ReSharper disable once CheckNamespace
namespace TMGmod.src
{
    [EditorGroup("TMG|Machinegun")]
    public class M4A1 : Gun
    {
		
		public M4A1 (float xval, float yval)
          : base(xval, yval)
        {
            ammo = 30;
            _ammoType = new ATMagnum {range = 300f, accuracy = 0.8f, penetration = 1.5f};
            _type = "gun";
            _graphic = new Sprite(GetPath("M4A1"));
            _center = new Vec2(15f, 6f);
            _collisionOffset = new Vec2(-15f, -6f);
            _collisionSize = new Vec2(30f, 11f);
            _barrelOffsetTL = new Vec2(31f, 4f);
            _fireSound = "deepMachineGun";
            _fullAuto = true;
            _fireWait = 0.745f;
            _kickForce = 0f;
            loseAccuracy = 0.01f;
            maxAccuracyLost = 0.12f;
            _holdOffset = new Vec2(3f, 1f);
            _editorName = "M4A1";
			_weight = 4.5f;
        }
	}
}