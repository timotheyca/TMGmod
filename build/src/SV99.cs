﻿using DuckGame;
// ReSharper disable VirtualMemberCallInConstructor

namespace TMGmod
{
    [EditorGroup("TMG|Sniper")]
    // ReSharper disable once InconsistentNaming
    public class SV99 : Sniper
    {
        public SV99(float xval, float yval) : base(xval, yval)
        {
            graphic = new Sprite(GetPath("SV99"));
            center = new Vec2(13.5f, 4.5f);
            collisionOffset = new Vec2(-13.5f, -4.5f);
            collisionSize = new Vec2(27f, 9f);
            _barrelOffsetTL = new Vec2(28f, 5f);
            ammo = 6;
            _ammoType = new AT9mm
            {
                penetration = 1f,
                range = 1000f
            };
            _fireSound = GetPath("sounds/Silenced3.wav");
            _fullAuto = false;
            _kickForce = 1.25f;
            loseAccuracy = 0.5f;
            maxAccuracyLost = 1.5f;
            _holdOffset = new Vec2(1f, 0f);
			_manualLoad = true;
            _editorName = "SV-99";
			weight = 2f;
		}
        
        public override void Update()
		{
			base.Update();
			if (_loadState > -1)
			{
				if (owner == null)
				{
					if (_loadState == 2)
					{
						loaded = true;
					}
					_loadState = -1;
					_angleOffset = 0f;
					handOffset = Vec2.Zero;
				}
				if (_loadState == 0)
				{
					if (Network.isActive)
					{
						if (isServerForObject)
						{
							_netLoad.Play();
						}
					}
					else
					{
						SFX.Play("loadSniper");
					}
					_loadState++;
				}
				else if (_loadState == 1)
				{
					if (_angleOffset < 0.16f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0.25f, 0.25f);
					}
					else
					{
						_loadState++;
					}
				}
				else if (_loadState == -1)
				{
					handOffset.x = handOffset.x + 0.8f;
					if (handOffset.x > 4f)
					{
						_loadState++;
						Reload();
						loaded = false;
					}
				}
				else if (_loadState == 3)
				{
					handOffset.x = handOffset.x - 0.8f;
					if (handOffset.x <= 0f)
					{
						_loadState++;
						handOffset.x = 0f;
					}
				}
				else if (_loadState == 4)
				{
					if (_angleOffset > 0.04f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0f, 0.25f);
					}
					else
					{
						_loadState = -1;
						loaded = true;
						_angleOffset = 0f;
					}
				}
			}
			if (loaded && owner != null && _loadState == -1)
			{
				laserSight = false;
				return;
			}
			laserSight = false;
		}

		public override void OnPressAction()
		{
            _ammoType.accuracy = _owner == null || _owner.velocity != new Vec2(0f, 0f) ? 0f : 1f;
			if (loaded)
			{
				base.OnPressAction();
				return;
			}

		    if (ammo <= 0 || _loadState != -1) return;
            //else
		    _loadState = 0;
		    _loadAnimation = 0;
		}

		public override void Draw()
		{
			var ang = angle;
			if (offDir > 0)
			{
				angle -= _angleOffset;
			}
			else
			{
				angle += _angleOffset;
			}
			base.Draw();
			angle = ang;
		}

        
	}
}