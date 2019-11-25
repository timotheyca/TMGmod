﻿#if DEBUG
using System.Linq;
using JetBrains.Annotations;
using DuckGame;
using System;
using System.Collections.Generic;
using MathNet.Numerics;

namespace TMGmod.Buddies
{
    [PublicAPI]
    [EditorGroup("TMG|DEBUG")]
    public class StingerMissile:PhysicsObject
    {
        private readonly List<Vec2> _pList = new List<Vec2>();
        private const float A = 0.22f;
        private MaterialThing _target;
        private Vec2 _tlv;
        private Vec2 _ta;
        private bool _activated;
        private int _ticks;
        public StingerMissile(float xval, float yval) : base(xval, yval)
        {
            var sprite = new SpriteMap(GetPath("ColoredCases"), 14, 8);
            _graphic = sprite;
            sprite.frame = 3;
            _center = new Vec2(7f, 4f);
            _collisionOffset = new Vec2(-7f, -4f);
            _collisionSize = new Vec2(14f, 8f);
            depth = -0.5f;
            thickness = 0.0f;
            _weight = 3f;
            airFrictionMult = 0f;
            throwSpeedMultiplier = 5f;
            friction = 0;
        }

        public override void Update()
        {
            UpdateTarget();
            UpdateFlight();
            base.Update();
            _pList.Add(position);
            if (_ticks > 10)
                _activated = true;
            else
                _ticks += 1;
        }

        private void UpdateTarget()
        {
            Level.Add(SmallSmoke.New(x, y));
            if (_target != null)
                if (Level.CheckLine<IPlatform>(position, _target.position) != null || _target is Duck duck0 && duck0.dead || _target == owner)
                    _target = null;
            if (_target != null) return;
            //else
            var ducks = Level.CheckCircleAll<Duck>(position, 1000);
            foreach (var d in ducks)
            {
                if (Level.CheckLine<IPlatform>(position, d.position) == null && !d.dead && (owner == null || owner.owner != d))
                    _target = d;
            }

            var stgs = Level.CheckCircleAll<StingerMissile>(position, 1000);
            foreach (var d in stgs)
            {
                if (Level.CheckLine<IPlatform>(position, d.position) == null && d._activated && d != this && !d._destroyed && (owner == null || d._target == owner.owner))
                    _target = d;
            }
        }

        protected override bool OnDestroy(DestroyType dtype = null)
        {
            if (_destroyed) return true;
            if (!_activated) return false;
            _destroyed = true;
            new ATMissileShrapnel().MakeNetEffect(position);
            Random random = null;
            if (Network.isActive && isLocal)
            {
                random = Rando.generator;
                Rando.generator = new Random(NetRand.currentSeed);
            }
            var varBullets = new List<Bullet>();
            for (var index = 0; index < 12; ++index)
            {
                var num = (float)(index * 30.0 - 10.0) + Rando.Float(20f);
                var atMissileShrapnel = new ATMissileShrapnel { range = 15f + Rando.Float(5f) };
                var vec2 = new Vec2((float)Math.Cos(Maths.DegToRad(num)), (float)Math.Sin(Maths.DegToRad(num)));
                var bullet = new Bullet(x + vec2.x * 8f, y - vec2.y * 8f, atMissileShrapnel, num) { firedFrom = this };
                varBullets.Add(bullet);
                Level.Add(bullet);
                Level.Add(Spark.New(x + Rando.Float(-8f, 8f), y + Rando.Float(-8f, 8f), vec2 + new Vec2(Rando.Float(-0.1f, 0.1f), Rando.Float(-0.1f, 0.1f))));
                Level.Add(SmallSmoke.New(x + vec2.x * 8f + Rando.Float(-8f, 8f), y + vec2.y * 8f + Rando.Float(-8f, 8f)));
            }
            if (Network.isActive && isLocal)
            {
                Send.Message(new NMFireGun(null, varBullets, 0, false), NetMessagePriority.ReliableOrdered);
                varBullets.Clear();
            }
            if (Network.isActive && isLocal)
                Rando.generator = random;
            foreach (var window in Level.CheckCircleAll<DuckGame.Window>(position, 30f))
            {
                if (isLocal)
                    Fondle(window, DuckNetwork.localConnection);
                if (Level.CheckLine<Block>(position, window.position, window) == null)
                    window.Destroy(new DTImpact(this));
            }
            foreach (var physicsObject in Level.CheckCircleAll<PhysicsObject>(position, 70f))
            {
                if (isLocal && owner == null)
                    Fondle(physicsObject, DuckNetwork.localConnection);
                if ((physicsObject.position - position).length < 30.0)
                    physicsObject.Destroy(new DTImpact(this));
                physicsObject.sleeping = false;
                physicsObject.vSpeed = -2f;
            }
            var varBlocks = new HashSet<ushort>();
            foreach (var blockGroup1 in Level.CheckCircleAll<BlockGroup>(position, 50f))
            {
                if (blockGroup1 == null) continue;
                var blockGroup2 = blockGroup1;
                foreach (var block in blockGroup2.blocks.Where(block => Collision.Circle(position, 28f, block.rectangle)))
                {
                    block.shouldWreck = true;
                    if (block is AutoBlock autoBlock)
                        varBlocks.Add(autoBlock.blockIndex);
                }
                blockGroup2.Wreck();
            }
            foreach (var block in Level.CheckCircleAll<Block>(position, 28f))
            {
                switch (block)
                {
                    case AutoBlock autoBlock:
                        autoBlock.skipWreck = true;
                        autoBlock.shouldWreck = true;
                        varBlocks.Add(autoBlock.blockIndex);
                        break;
                    case Door _:
                    case VerticalDoor _:
                        Level.Remove(block);
                        block.Destroy(new DTRocketExplosion(null));
                        break;
                }
            }
            if (Network.isActive && isLocal && varBlocks.Count > 0)
                Send.Message(new NMDestroyBlocks(varBlocks));
            Level.Remove(this);
            _pList.Clear();
            return true;
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            base.OnSolidImpact(with, from);
            Destroy(new DTImpact(with));
        }

        private void UpdateFlight()
        {
            if (!_activated) return;
            //velocity += OffsetLocal(new Vec2(A, 0));
            if (_target is null) return;
            _ta = _target.velocity - _tlv;
            _tlv = _target.velocity;
            var p = _target.position - position;
            var v = _target.velocity - velocity;
            var g = new Vec2(0, gravity) - _ta;
            var pa = Delta(-v, p, g);
            var maybeangle = Math.Acos(pa.x / pa.length);
            if (pa.y < 0) maybeangle = -maybeangle;
            if (offDir < 0) maybeangle += Math.PI;
            angle = (float) maybeangle;
            velocity += pa;
        }

        public override void Draw()
        {
            if (_target != null)
                Graphics.DrawCircle(_target.position, 16, Color.Red, depth: _target.depth.value + 3f);
            base.Draw();
            var p0 = position;
            _pList.Reverse();
            foreach (var pi in _pList)
            {
                Graphics.DrawLine(p0, pi, Color.Crimson);
                p0 = pi;
            }
            _pList.Reverse();
            if (_target is null) return;
            var p = _target.position - position;
            var v = _target.velocity - velocity;
            var g = new Vec2(0, gravity);
            var t = Time4(A * A, -v, p, g);
            var pa = Delta(-v, p, g);
            p0 = position;
            var v0 = velocity;
            for (var i = 0; i < 60; i++)
            {
                var pi = p0 + v0;
                Graphics.DrawLine(p0, pi, Color.Green);
                v0 += pa + g;
                p0 = pi;
            }
            p0 = _target.position;
            v0 = _target.velocity;
            for (var i = 0; i < 60; i++)
            {
                var pi = p0 + v0;
                Graphics.DrawLine(p0, pi, Color.Blue);
                v0 += _ta;
                p0 = pi;
            }

            var ipos = position + (pa + g) * t * t / 2 + velocity * t;
            Graphics.DrawCircle(ipos, 24, Color.Yellow, depth: _target.depth.value + 3f);
        }

        public override void Touch(MaterialThing with)
        {
            switch (with)
            {
                case Duck duck0:
                    Destroy(new DTImpact(duck0));
                    break;
                case StingerMissile stg:
                    Destroy(new DTImpact(stg));
                    stg.Destroy(new DTImpact(this));
                    break;
            }

            base.Touch(with);
        }


        /*private float Time5(float aa, Vec2 v, Vec2 p, Vec2 g, float t)
        {
            var res = 0f;
            var gg = Vec2.Dot(g, g);
            var vg = Vec2.Dot(v, g);
            var vv = Vec2.Dot(v, v);
            var pg = Vec2.Dot(p, g);
            var pv = Vec2.Dot(p, v);
            var pp = Vec2.Dot(p, p);
            res += gg - aa;
            res *= t;
            res += 4 * vg;
            res *= t;
            res += 4 * (vv - pg);
            res *= t;
            res += -8 * pv;
            res *= t;
            res += 4 * pp;
            return res;
        }*/

        private double Time4(double aa, Vec2 v, Vec2 p, Vec2 g)
        {
            double gg = Vec2.Dot(g, g);
            double vg = Vec2.Dot(v, g);
            double vv = Vec2.Dot(v, v);
            double pg = Vec2.Dot(p, g);
            double pv = Vec2.Dot(p, v);
            double pp = Vec2.Dot(p, p);
            var rar = FindRoots.Polynomial(new []
            {
                4 * pp,
                -8 * pv,
                4 * (vv - pg),
                4 * vg,
                gg - aa
            });
            var t = (from r in rar where r.IsRealNonNegative() select r.Real).Concat(new[] { 1e+10 }).Min();
            return t;
        }

        private Vec2 Delta(Vec2 v, Vec2 p, Vec2 g)
        {
            const double aa = A * A;
            var t = Time4(aa, v, p, g);
            const float e = 1e-6f;
            if (t < e)
            {
                var av0 = -g;
                av0 /= g.length;
                av0 *= A;
                return av0;
            }

            var av = 2 * p - 2 * v * (float) t - g * (float) (t * t);
            av /= (float) (t * t);
            return av;
        }
    }
}

#endif