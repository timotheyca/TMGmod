﻿using DuckGame;

// ReSharper disable once CheckNamespace
namespace TMGmod.src
{
    [EditorGroup("TMG|Sniper")]
    // ReSharper disable once InconsistentNaming
    public class SNR22 : Gun
    {
        public SNR22 (float xval, float yval)
          : base(xval, yval)
        {
            ammo = 5;
            _ammoType = new ATSniper {range = 1200f, accuracy = 1f, penetration = 1f};
            _type = "gun";
            _graphic = new Sprite(GetPath("SNR22"));
            _center = new Vec2(14f, 6f);
            _collisionOffset = new Vec2(-14.5f, -5f);
            _collisionSize = new Vec2(33f, 10f);
            _barrelOffsetTL = new Vec2(33f, 4f);
            _fireSound = GetPath("sounds/HeavySniper.wav");
            _fullAuto = false;
            _fireWait = 5f;
            _kickForce = 0.8f;
            loseAccuracy = 0.1f;
            maxAccuracyLost = 0.3f;
            _holdOffset = new Vec2(0f, 1f);
            laserSight = true;
            _laserOffsetTL = new Vec2(22f, 3.5f);
            _editorName = "Gepard Lynx";
			_weight = 6f;
        }
        public override void Update()
        {
            if (_owner != null && _owner.height < 17f)
            {
                _kickForce = 0f;
				loseAccuracy = 0f;
                maxAccuracyLost = 0f;
				_graphic = new Sprite(GetPath("SNR22bipods"));
            }
            else
            {
                _kickForce = 0.8f;
                loseAccuracy = 0.1f;
                maxAccuracyLost = 0.3f;
				_graphic = new Sprite(GetPath("SNR22"));
            }
            base.Update();
        }
	}
}