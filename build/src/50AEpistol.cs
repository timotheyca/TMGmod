﻿using System.Collections.Generic;
using DuckGame;
using TMGmod.Core;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [EditorGroup("TMG|Pistol")]
    public class BigShot : BaseGun, IAmHg, IHaveSkin
    {
        private readonly SpriteMap _sprite;
        private const int NonSkinFrames = 1;
        public StateBinding FrameIdBinding = new StateBinding(nameof(FrameId));
        public readonly EditorProperty<int> Fid;
        private static readonly List<int> Allowedlst = new List<int>(new[] { 1, 2, 7 });
        public BigShot (float xval, float yval)
          : base(xval, yval)
        {
            Fid = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 7;
            _ammoType = new AT50C();
            _type = "gun";
            _sprite = new SpriteMap(GetPath("50AEPistolpattern"), 26, 10);
            _graphic = _sprite;
            _sprite.frame = 1;
            _center = new Vec2(13f, 5f);
            _collisionOffset = new Vec2(-13f, -5f);
            _collisionSize = new Vec2(26f, 10f);
            _barrelOffsetTL = new Vec2(26f, 2f);
            _fireSound = "magnum";
            _fullAuto = false;
            _fireWait = 1.6f;
            _kickForce = 3.76f;
            loseAccuracy = 0.5f;
            maxAccuracyLost = 1f;
            _holdOffset = new Vec2(0f, 2f);
            _editorName = "50AE Pistol";
			_weight = 2.5f;
        }
        private void UpdateSkin()
        {
            var fid = Fid.value;
            while (!Allowedlst.Contains(fid))
            {
                fid = Rando.Int(0, 9);
            }
            _sprite.frame = fid;
        }
        public int FrameId
        {
            get => _sprite.frame;
            set => _sprite.frame = value % (10 * NonSkinFrames);
        }
        public override void EditorPropertyChanged(object property)
        {
            UpdateSkin();
            base.EditorPropertyChanged(property);
        }
    }
}