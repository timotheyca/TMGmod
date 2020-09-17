﻿#if DEBUG
using System.Collections.Generic;
using DuckGame;
using JetBrains.Annotations;
using TMGmod.Core;
using TMGmod.Core.WClasses;
using TMGmod.Core.AmmoTypes;

namespace TMGmod
{
    [EditorGroup("TMG|Rifle|Combined")]
    public class ValmetM76 : BaseBurst, IAmDmr, IHaveSkin
    {
        private readonly SpriteMap _sprite;

        [UsedImplicitly]
        public bool NonAuto
        {
            get => BurstNum == 1;
            set
            {
                BurstNum = value ? 1 : 2;
                _fireWait = value ? 0.7f : 1.4f;
                FrameId = FrameId % 10 + (value ? 0 : 10);
                loseAccuracy = value ? 0.2f : 0f;
                _kickForce = value ? 3f : 6.5f;
                _ammoType.accuracy = value ? 0.89f : 1f;
            }
        }
        private const int NonSkinFrames = 2;
        public StateBinding FrameIdBinding { get; } = new StateBinding(nameof(FrameId));
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private readonly EditorProperty<int> skin;
        // ReSharper disable once ConvertToAutoProperty
        public EditorProperty<int> Skin => skin;
        private static readonly List<int> Allowedlst = new List<int>(new[] { 0 });
        public ValmetM76(float xval, float yval)
          : base(xval, yval)
        {
            skin = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 20;
            _ammoType = new AT545NATO
            {
                range = 410f,
                accuracy = 0.89f
            };
            _type = "gun";
            _sprite = new SpriteMap(GetPath("Valmet M76"), 33, 10);
            _graphic = _sprite;
            _sprite.frame = 0;
            _center = new Vec2(17f, 5f);
            _collisionOffset = new Vec2(-17f, -5f);
            _collisionSize = new Vec2(33f, 10f);
            _barrelOffsetTL = new Vec2(33f, 3f);
            _flare = new SpriteMap(GetPath("FlareOnePixel2"), 13, 10)
            {
                center = new Vec2(0.0f, 5f)
            };
            _holdOffset = new Vec2(4f, 1f);
            ShellOffset = new Vec2(-2f, -2f);
            _fireSound = GetPath("sounds/HeavyRifle.wav");
            _fullAuto = false;
            _fireWait = 0.7f;
            _kickForce = 3f;
            loseAccuracy = 0.2f;
            maxAccuracyLost = 0.4f;
            _editorName = "Valmet M76";
			_weight = 4f;
            DeltaWait = 0.15f;
            BurstNum = 1;
        }
        public override void Update()
        {
            if (duck?.inputProfile.Pressed("QUACK") == true)
            {
                NonAuto = !NonAuto;
                SFX.Play(GetPath("sounds/tuduc.wav"));
            }
            base.Update();
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
        [UsedImplicitly]
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
#endif