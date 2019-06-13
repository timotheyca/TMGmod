﻿using System.Collections.Generic;
using DuckGame;
using TMGmod.Core;
using TMGmod.Core.WClasses;


namespace TMGmod
{
    [EditorGroup("TMG|Rifle|Fully-Automatic")]
    // ReSharper disable once InconsistentNaming
    public class AUGA3 : BaseAr, IHaveSkin
    {

        private readonly SpriteMap _sprite;
        private const int NonSkinFrames = 1;
        public StateBinding FrameIdBinding { get; } = new StateBinding(nameof(FrameId));
        public EditorProperty<int> Skin { get; }
        private static readonly List<int> Allowedlst = new List<int>(new[] { 0, 1, 5 });

        public AUGA3(float xval, float yval)
          : base(xval, yval)
        {
            Skin = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 30;
            _ammoType = new ATMagnum
            {
                range = 376f,
                accuracy = 0.93f,
                penetration = 1f
            };
            _type = "gun";
            _sprite = new SpriteMap(GetPath("AUGA3"), 30, 12);
            _graphic = _sprite;
            _sprite.frame = 0;
            _center = new Vec2(15f, 6f);
            _collisionOffset = new Vec2(-15f, -6f);
            _collisionSize = new Vec2(30f, 12f);
            _barrelOffsetTL = new Vec2(30f, 5f);
            _holdOffset = new Vec2(-3f, 0f);
            _fireSound = GetPath("sounds/scar.wav");
            _fullAuto = true;
            _fireWait = 0.8f;
            _kickForce = 1.7f;
            loseAccuracy = 0.08f;
            maxAccuracyLost = 0.2f;
            _editorName = "AUG A3";
			_weight = 5.5f;
            Kforce2Ar = 0.7f;
        }
        private void UpdateSkin()
        {
            var bublic = Skin.value;
            while (!Allowedlst.Contains(bublic))
            {
                bublic = Rando.Int(0, 9);
            }
            _sprite.frame = bublic;
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