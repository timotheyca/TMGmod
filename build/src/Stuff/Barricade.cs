﻿using System;
using System.Linq;
using DuckGame;
using TMGmod.Core.Particles;

// ReSharper disable VirtualMemberCallInConstructor

namespace TMGmod.Stuff
{
    [EditorGroup("TMG|Misc")]
    public class Barricade:Block
    {
        private bool _anchored;
        public float Hp;
        public StateBinding HpBinding = new StateBinding(nameof(Hp));
        public float ImpactSpeed;
        public StateBinding ImpactSpeedBinding = new StateBinding(nameof(ImpactSpeed));
        private float _duckcooldown;
        public Barricade(float x, float y) : base(x, y)
        {
            _anchored = true;
            Hp = 10f;
            thickness = 2f;
            physicsMaterial = PhysicsMaterial.Wood;
            center = new Vec2(1f, 2f);
            collisionOffset = new Vec2(-1f, -2f);
            collisionSize = new Vec2(2f, 4f);
            graphic = new Sprite(GetPath("barr"));
            flammable = 0.6f;
            _isStateObject = true;
            //if (!(owner is Duck duck)) return;
            //duck.clip.Add(this);
            //clip.Add(duck);
        }

        private bool CheckBlocks()
        {
            var blocks = Level.CheckLineAll<Block>(new Vec2(x, y + 2), new Vec2(x, y + 4));
            _anchored = false;
            foreach (var block in blocks)
            {
                if (block is Barricade && !(block as Barricade)._anchored) continue;
                //else
                _anchored = true;
                
            }
            blocks = Level.CheckLineAll<Block>(new Vec2(x, y), new Vec2(x, y - 4));
            return _anchored || blocks.Any(block => block != this);
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            SFX.Play("woodHit");
            Damage(bullet.ammo.penetration * (bullet.ammo is ATShrapnel ? 2 : 1));
            ImpactSpeed = bullet.hSpeed;
            return base.Hit(bullet, hitPos);
        }

        private void Damage(float dValue)
        {
            var barricades = Level.CheckCircleAll<Barricade>(position, 10f);
            foreach (var barricade in barricades)
            {
                if (barricade != this && barricade.Hp >= 1f)
                {
                    barricade.Hp -= dValue;
                }
            }

            Hp -= dValue;
        }

        public override void OnImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with is Duck duck && duck.inputProfile.Down("SHOOT") && _duckcooldown < 0)
            {
                SFX.Play("woodHit");
                _duckcooldown = 2.0f;
                Hp -= 4f;
            }
            else if (Math.Abs(with.hSpeed) > 5f)
            {
                SFX.Play("woodHit");
                Hp -= Math.Abs(with.hSpeed) * 0.2f - 1f;
                Damage(Math.Abs(with.hSpeed) * 0.2f);
            }

            ImpactSpeed = with.hSpeed * 2f;
            base.OnImpact(with, from);
        }

        public override void Update()
        {
            _duckcooldown -= 0.1f;
            if (Hp < 0f || !CheckBlocks())
            {
                SFX.Play("woodHit");
                var bbp = new BarrBetaPar(x, y)
                {
                    hSpeed = ImpactSpeed + Rando.Float(-1f, 1f),
                    vSpeed = Rando.Float(-1.5f, 1.5f)
                };
                Level.Add(bbp);
                Destroy();
                Level.Remove(this);
            }
            base.Update();
            thickness = 0.2f * Hp;
        }
    }
}