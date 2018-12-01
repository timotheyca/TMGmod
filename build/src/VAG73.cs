﻿using DuckGame;
// ReSharper disable VirtualMemberCallInConstructor

namespace TMGmod
{
    [EditorGroup("TMG|AutoPistol")]
    public class Vag : Gun
    {
        private float _mode = 1f;
		
		public Vag(float xval, float yval)
          : base(xval, yval)
        {
            ammo = 48;
            _ammoType = new AT9mm
            {
                range = 175f,
                accuracy = 0.81f,
                penetration = 1f,
                bulletSpeed = 12f
            };
            _type = "gun";
            graphic = new Sprite(GetPath("VAG731"));
            center = new Vec2(8f, 3f);
            collisionOffset = new Vec2(-7.5f, -3.5f);
            collisionSize = new Vec2(16f, 11f);
            _barrelOffsetTL = new Vec2(16f, 1f);
            _holdOffset = new Vec2(-2f, -3.5f);
            _fireSound = GetPath("sounds/2.wav");
            _fullAuto = true;
            _fireWait = 0.3f;
            _kickForce = 0.1f;
            loseAccuracy = 0.075f;
            maxAccuracyLost = 0.225f;
            _editorName = "Dominator";
			weight = 2f;
        }
        public override void Update()
        {
            if (owner != null)
            {
                if (isServerForObject)
                {
                    if (duck.inputProfile.Pressed("QUACK"))
                    {
					  if (_mode > 0f && _mode < 2f)
					    {
				         graphic = new Sprite(GetPath("VAG732"));
						 _fireWait = 0.6f;
						 _mode = 2f;
					    }
                      else if (_mode > 1f && _mode < 3f)
					    {
				         graphic = new Sprite(GetPath("VAG733"));
						 _fireWait = 0.9f;
						 _mode = 3f;
					    }
                      else if (_mode > 2f && _mode < 4f)
					    {
				         graphic = new Sprite(GetPath("VAG731"));
						 _fireWait = 0.3f;
						 _mode = 1f;
					    }
					}
				}
			}
		    base.Update();
		}			
	}
}