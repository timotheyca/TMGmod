﻿using System;
using System.Collections.Generic;
using DuckGame;
using JetBrains.Annotations;

namespace TMGmod.Stuff
{
    /// <summary>
    /// Stickable quack-explosives
    /// </summary>
    [EditorGroup("TMG|Misc")]
    [BaggedProperty("canSpawn", false)]
    [PublicAPI]
    public class Cfour : Holdable
    {
        /// <summary>
        /// Time to explode
        /// </summary>
        public float ToExplode = -1f;
        /// <summary>
        /// <see cref="ToExplode"/> syncing
        /// </summary>
        public StateBinding ToExplodeBinding = new StateBinding(nameof(ToExplode));
        /// <summary>
        /// Whether is was activated by <see cref="Duck"/>
        /// </summary>
        public bool Activated;
        /// <summary>
        /// <see cref="Activated"/> syncing
        /// </summary>
        public StateBinding ActivatedBinding = new StateBinding(nameof(Activated));
        /// <summary>
        /// Who/what activated it
        /// </summary>
        public MaterialThing Activator;
        /// <summary>
        /// <see cref="Activator"/> syncing
        /// </summary>
        public StateBinding ActivatorBinding = new StateBinding(nameof(Activator));
        /// <summary>
        /// what it's stick to
        /// </summary>
        public MaterialThing StickThing;
        //public StateBinding StickBinding = new StateBinding(nameof(StickThing));
        private Vec2 _stickyVec2;
        /// <summary>
        /// Whether it was thrown by <see cref="Duck"/>
        /// </summary>
        public bool WasThrown;
        /// <summary>
        /// <see cref="WasThrown"/> syncing
        /// </summary>
        public StateBinding WasThrownBinding = new StateBinding(nameof(WasThrown));
        /// <summary>
        /// Whether it doesn't explode properly
        /// </summary>
        public bool Weak;
        private bool _didboom;

        /// <inheritdoc />
        public Cfour(float xpos, float ypos) : base(xpos, ypos)
        {
            _weight = 3f;
            _graphic = new Sprite(GetPath("cfour"));
            _center = new Vec2(3f, 1.5f);
            _collisionOffset = new Vec2(-1.5f, -1.5f);
            _collisionSize = new Vec2(3f, 3f);
            flammable = 0.9f;
            thickness = 1f;
            throwSpeedMultiplier = 1.5f;
            airFrictionMult = 0.05f;
        }

        private void Explode()
        {
            if (_didboom) return;
            _didboom = true;
            Activator?.Fondle(this);
            Graphics.FlashScreen();
            for (var index = 0; index < 1; ++index)
            {
                var explosionPart = new ExplosionPart(x - 8f + Rando.Float(16f), y - 8f + Rando.Float(16f));
                explosionPart.xscale *= 0.7f;
                explosionPart.yscale *= 0.7f;
                Level.Add(explosionPart);
            }
            SFX.Play("explode");
            if (!isServerForObject || Weak) return;


            var varBullets = new List<Bullet>();
            for (var index = 0; index < 150; ++index)
            {
                var num = (float)(index * 30.0 - 10.0) + Rando.Float(20f);
                var atShrapnel = new ATShrapnel { range = 30f + Rando.Float(0f, Rando.Float(70f)) };
                var bullet = new Bullet(x + (float)(Math.Cos(Maths.DegToRad(num)) * 8.0),
                        y - (float)(Math.Sin(Maths.DegToRad(num)) * 8.0), atShrapnel, num)
                    { firedFrom = this };
                varBullets.Add(bullet);
                Level.Add(bullet);
            }

            if (Network.isActive)
            {
                Send.Message(new NMExplodingProp(varBullets), NetMessagePriority.ReliableOrdered);
                varBullets.Clear();
            }
            foreach (var window in Level.CheckCircleAll<Window>(position, 40f))
                if (Level.CheckLine<Block>(position, window.position, window) == null)
                    window.Destroy(new DTImpact(this));
            foreach (var thing in Level.CheckCircleAll<Thing>(position, 500f))
            {
                if (Level.CheckLine<Block>(position, thing.position, thing) != null) continue;
                //else
                var dVec2 = thing.position - position + new Vec2(Rando.Float(-6f, 6f), Rando.Float(-6f, 6f));
                var l = dVec2.length + 0.1f;
                var force = dVec2 * (1000f / (l * l * l));
                //force.y *= 0.8f;
                //force.x *= 1.1f;
                thing.ApplyForce(force);
            }
            AddFire();
            Level.Remove(this);
        }

        /// <inheritdoc />
        public override void UpdateOnFire()
        {
            ToExplode = Rando.Float(0f, 1.5f);
            base.UpdateOnFire();
        }

        /// <inheritdoc />
        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            ToExplode = Rando.Float(0f, 0.7f);
            return base.Hit(bullet, hitPos);
        }

        /// <inheritdoc />
        public override void OnImpact(MaterialThing with, ImpactedFrom from)
        {
            base.OnImpact(with, from);
            if (StickThing != null) return;
            if (!Activated) return;
            StickThing = with;
            StickThing.Fondle(this);
            _stickyVec2 = position - with.position;
            //enablePhysics = false;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (from)
            {
                case ImpactedFrom.Right:
                    angleDegrees = -90f;
                    break;
                case ImpactedFrom.Bottom:
                    angleDegrees = 0f;
                    break;
                case ImpactedFrom.Left:
                    angleDegrees = 90f;
                    break;
                case ImpactedFrom.Top:
                    angleDegrees = 180f;
                    break;
            }
        }

        /// <inheritdoc />
        public override void Thrown()
        {
            WasThrown = true;
            base.Thrown();
        }

        /// <inheritdoc />
        public override void Update()
        {
            if (_destroyed) return;
            if (StickThing != null && StickThing._destroyed)
            {
                StickThing = null;
            }

            ToExplode -= 0.1f;
            if (duck != null && duck.holdObject == this)
            {
                StickThing = null;
                if (duck != Activator)
                {
                    Activated = false;
                }
            }

            if (StickThing != null)
            {
                sleeping = true;
                position = StickThing.position + _stickyVec2;
                hSpeed = StickThing.hSpeed;
                vSpeed = StickThing.vSpeed;
            }
            else
            {
                sleeping = false;
            }

            if (Activator is Duck duck1 && !duck1.dead && duck1.IsQuacking()) ToExplode = Rando.Float(0f, 0.5f);

            if (grounded) angle = 0f;
            else if ((duck == null || duck.holdObject != this) && WasThrown && StickThing == null)
            {
                angle += 0.3f * offDir;
            }

            if (-0.5 < ToExplode && ToExplode < 0f && !_destroyed) Destroy();

            base.Update();
        }

        /// <inheritdoc />
        public override void OnPressAction()
        {
            if (duck == null) return;
            //else
            Activated = true;
            Activator = duck;
        }

        /// <inheritdoc />
        protected override bool OnDestroy(DestroyType _ = null)
        {
            if (_destroyed) return true;
            Explode();
            return true;
        }
    }
}