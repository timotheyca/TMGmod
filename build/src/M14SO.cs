﻿#if DEBUG
using System;
using System.Collections.Generic;
using DuckGame;
using JetBrains.Annotations;
using TMGmod.Core;
using TMGmod.Core.AmmoTypes;
using TMGmod.Core.WClasses;

namespace TMGmod
{
    [EditorGroup("TMG|Sniper|Fully-Automatic")]
    // ReSharper disable once InconsistentNaming
    public class M14SO : BaseGun, IHaveSkin, IFirstPrecise, IHaveStock
    {
        private bool _stock = true;
        [UsedImplicitly]
        public bool Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                var stockstate = StockState;
                if (isServerForObject)
                    StockState += 1f / 17 * (value ? 1 : -1);
                var nostock = StockState < 0.01f;
                var stock = StockState > 0.99f;
                _fireWait = stock ? 1.25f : 1f;
                loseAccuracy = stock ? 0.15f : 0.2f;
                maxAccuracyLost = stock ? 0.5f : 0.7f;
                weight = stock ? 2.5f : 2f;
                FrameId = FrameId % 10 + 10 * (stock ? 0 : nostock ? 2 : 1);
                if (isServerForObject && stock && stockstate <= 0.99f)
                    SFX.Play(GetPath("sounds/tuduc"));
                if (isServerForObject && nostock && stockstate >= 0.01f)
                    SFX.Play(GetPath("sounds/tuduc"));
            }
        }

        private float _stockstate = 1f;
        public float StockState
        {
            get => _stockstate;
            set
            {
                value = Math.Max(value, 0f);
                value = Math.Min(value, 1f);
                _stockstate = value;
            }
        }
        public StateBinding StockStateBinding { get; } = new StateBinding(nameof(StockState));

        [UsedImplicitly]
        public StateBinding StockBinding { get; } = new StateBinding(nameof(StockBuffer));

        public BitBuffer StockBuffer
        {
            get
            {
                var b = new BitBuffer();
                b.Write(Stock);
                return b;
            }
            set => Stock = value.ReadBool();
        }
        private readonly SpriteMap _sprite;
        private const int NonSkinFrames = 3;
        public StateBinding FrameIdBinding { get; } = new StateBinding(nameof(FrameId));
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private readonly EditorProperty<int> skin;
        // ReSharper disable once ConvertToAutoProperty
        public EditorProperty<int> Skin => skin;
        private static readonly List<int> Allowedlst = new List<int>(new[] { 0 });
        public M14SO(float xval, float yval)
          : base(xval, yval)
        {
            skin = new EditorProperty<int>(0, this, -1f, 9f, 0.5f);
            ammo = 18;
            _ammoType = new AT762NATO
            {
                range = 333f,
                accuracy = 0.8f
            };
            BaseAccuracy = 0.8f;
            _type = "gun";
            _sprite = new SpriteMap(GetPath("Sawed-Off M14"), 31, 11);
            _graphic = _sprite;
            _sprite.frame = 0;
            _center = new Vec2(16f, 6f);
            _collisionOffset = new Vec2(-16f, -6f);
            _collisionSize = new Vec2(31f, 11f);
            _barrelOffsetTL = new Vec2(31f, 3f);
            _flare = new SpriteMap(GetPath("FlareOnePixel2"), 13, 10)
            {
                center = new Vec2(0.0f, 5f)
            };
            _fireSound = GetPath("sounds/scar.wav");
            _fullAuto = true;
            _fireWait = 1.25f;
            _kickForce = 2.5f;
            loseAccuracy = 0.15f;
            maxAccuracyLost = 0.5f;
            _holdOffset = new Vec2(0f, 1f);
            ShellOffset = new Vec2(-9f, -4f);
            _editorName = "M14 Sawed-Off";
            _weight = 2.5f;
            MaxAccuracy = 1f;
            MaxDelayFp = 12;
        }
        public override void Update()
        {
            base.Update();
            if (SwitchStockQ() && (Stock || duck.grounded) && duck.inputProfile.Pressed("QUACK"))
            {
                Stock = !Stock;
                SFX.Play("quack", -1);
            }
            else if (duck != null)
                Stock = Stock;
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
        public override void Fire()
        {
            if (FrameId / 10 == 1) return;
            base.Fire();
        }
        public int CurrDelay { get; set; }
        public int MaxDelayFp { get; }
        public float MaxAccuracy { get; }
    }
}
#endif