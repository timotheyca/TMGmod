﻿using System.Collections.Generic;
using DuckGame;
using TMGmod.Core;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [EditorGroup("TMG|Shotgun")]
    // ReSharper disable once InconsistentNaming
    public class SIX12S : Gun, IHaveSkin, IAmSg
    {

        private readonly SpriteMap _sprite;
        private const int NonSkinFrames = 1;
        public StateBinding FrameIdBinding = new StateBinding(nameof(FrameId));
        public readonly EditorProperty<int> Fid;
        private static readonly List<int> Allowedlst = new List<int>(new[]{ 0, 1, 2, 3, 4 });
        public SIX12S (float xval, float yval)
          : base(xval, yval)
        {
            Fid = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 6;
            _ammoType = new AT9mmS
            {
                range = 225f,
                accuracy = 0.9f,
                penetration = 1f
            };
            _numBulletsPerFire = 14;
            _type = "gun";
            _sprite = new SpriteMap(GetPath("SIX12Spattern"), 29, 10);
            _graphic = _sprite;
            _sprite.frame = 0;
            _center = new Vec2(19.5f, 5f);
            _collisionOffset = new Vec2(-19.5f, -5f);
            _collisionSize = new Vec2(29f, 10f);
            _barrelOffsetTL = new Vec2(30f, 4.5f);
            _fireSound = "shotgunFire";
            _fullAuto = false;
            _fireWait = 1.7f;
            _kickForce = 5f;
            loseAccuracy = 0.3f;
            maxAccuracyLost = 0.5f;
            laserSight = false;
            _laserOffsetTL = new Vec2(24f, 7f);
            _holdOffset = new Vec2(2f, 0f);
            _editorName = "SIX12 Silenced";
			_weight = 4f;
        }

        /*public override void Initialize()
        {
            var fid = Fid.value;
            if (fid == -1) fid = Rando.Int(0, 9);
            _sprite.frame = fid;
            base.Initialize();
        }*/

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