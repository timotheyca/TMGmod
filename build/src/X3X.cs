﻿using DuckGame;
// ReSharper disable VirtualMemberCallInConstructor

namespace TMGmod
{
    [EditorGroup("TMG|Pistol")]
    [BaggedProperty("isSuperWeapon", true)]
    public class X3X : Gun
    {
        private readonly SpriteMap _sprite;
        public X3X (float xval, float yval)
          : base(xval, yval)
        {
            ammo = 8;
            _ammoType = new ATMagnum
            {
                range = 500f,
                accuracy = 1f,
                penetration = 100f,
                bulletThickness = 5f
            };
            _type = "gun";
            //this.graphic = new Sprite(GetPath("X3X"));
            _sprite = new SpriteMap(GetPath("X3Xa"), 28, 15);
            graphic = _sprite;
            center = new Vec2(14f, 9f);
            collisionOffset = new Vec2(-11.5f, -9f);
            collisionSize = new Vec2(23f, 15f);
            _barrelOffsetTL = new Vec2(28f, 5f);
            _fireSound = "deepMachineGun2";
            _fullAuto = false;
            _fireWait = 2.5f;
            _kickForce = 10f;
            loseAccuracy = 1.9f;
            maxAccuracyLost = 0.5f;
            _holdOffset = new Vec2(0f, 2f);
            _editorName = "EXsess's X3X";
			weight = 5.5f;
            _manualLoad = true;
            _sprite.AddAnimation("idle", 0.3f, false, new int[]
            {
				0,
			});
            _sprite.AddAnimation("fire", 0.3f, false, new int[]
            {
				1,
			});
            _sprite.AddAnimation("empty", 1f, true, new int[]
            {
                1
            });
        }

        public override void Fire()
        {
            //base.Fire();
        }
        public override void Update()
        {
            if (_sprite.currentAnimation == "fire" && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            base.Update();
        }

        public override void OnPressAction()
        {
            if (ammo == 0)
            {
                _sprite.SetAnimation("empty");
            }
            if (loaded)
            {
                base.Fire();
            }
            else
            {
                SFX.Play("Click");
                _sprite.SetAnimation("fire");
                Reload();
            }
            base.OnPressAction();
        }
        
    }
}
