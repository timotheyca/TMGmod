﻿using System.Collections.Generic;
using DuckGame;
using JetBrains.Annotations;
using TMGmod.Core;
using TMGmod.Core.AmmoTypes;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [EditorGroup("TMG|Shotgun|Semi-Automatic")]
    // ReSharper disable once InconsistentNaming
    public class SIX12S : BaseGun, IHaveSkin, IAmSg, I5
    {
        private readonly SpriteMap _sprite;
        [UsedImplicitly]
        public StateBinding LaserBinding = new StateBinding(nameof(laserSight));
        private const int NonSkinFrames = 2;
        public StateBinding FrameIdBinding { get; } = new StateBinding(nameof(FrameId));
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private readonly EditorProperty<int> skin;
        // ReSharper disable once ConvertToAutoProperty
        public EditorProperty<int> Skin => skin;
        private static readonly List<int> Allowedlst = new List<int>(new[]{ 0, 1, 2, 3, 4, 5, 6, 7, 9 });
        public SIX12S (float xval, float yval)
          : base(xval, yval)
        {
            skin = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 6;
            _ammoType = new AT12GaugeS
            {
                range = 180f,
                accuracy = 0.9f
            };
            BaseAccuracy = 0.9f;
            _numBulletsPerFire = 14;
            _flare = new SpriteMap(GetPath("takezis"), 4, 4);
            _type = "gun";
            _sprite = new SpriteMap(GetPath("SIX12S"), 29, 10);
            _graphic = _sprite;
            _sprite.frame = 0;
            _center = new Vec2(19f, 5f);
            _collisionOffset = new Vec2(-19f, -5f);
            _collisionSize = new Vec2(29f, 10f);
            _barrelOffsetTL = new Vec2(29f, 4f);
            _fireSound = "shotgunFire";
            _fullAuto = false;
            _fireWait = 2f;
            _kickForce = 5.5f;
            loseAccuracy = 0.3f;
            maxAccuracyLost = 0.5f;
            laserSight = false;
            _laserOffsetTL = new Vec2(24f, 7.5f);
            _holdOffset = new Vec2(2f, 0f);
            _editorName = "SIX12 Silenced";
			_weight = 4f;
        }
        public override void Update()
        {
            if (duck?.inputProfile.Pressed("QUACK") == true)
            {
                if (laserSight)
                {
                    FrameId -= 10;
                    loseAccuracy = 0.3f;
                    maxAccuracyLost = 0.5f;
                    laserSight = false;
                }
                else
                {
                    FrameId += 10;
                    loseAccuracy = 0.45f;
                    maxAccuracyLost = 0.5f;
                    laserSight = true;
                }
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
        public override void Reload(bool shell = true)
        {
            if (ammo != 0)
            {
                --ammo;
            }
            loaded = true;
        }
    }
}