﻿using DuckGame;

// ReSharper disable once CheckNamespace
namespace TMGmod.src
{
    [EditorGroup("TMG|AutoPistol")]
    public class Glock18 : Gun
    {
		
        public Glock18(float xval, float yval)
          : base(xval, yval)
        {
            ammo = 17;
            _ammoType = new AT9mm {range = 100f, accuracy = 1f, penetration = 1f};
            _type = "gun";
            _graphic = new Sprite(GetPath("Glock17"));
            _center = new Vec2(8f, 3f);
            _collisionOffset = new Vec2(-7.5f, -3.5f);
            _collisionSize = new Vec2(16f, 11f);
            _barrelOffsetTL = new Vec2(16f, 1f);
            _fireSound = GetPath("sounds/2.wav");
            _fullAuto = true;
            _fireWait = 0.4f;
            _kickForce = 0.1f;
            loseAccuracy = 0.1f;
            maxAccuracyLost = 0.2f;
            _editorName = "Glock 18";
			_weight = 2f;
        }
	}
}