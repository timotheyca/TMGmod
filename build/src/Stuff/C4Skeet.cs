﻿using DuckGame;
using JetBrains.Annotations;

namespace TMGmod.Stuff
{
    [EditorGroup("TMG|Misc")]
    [BaggedProperty("canSpawn", false)]
    [PublicAPI]
    public sealed class C4Skeet : Holdable
    {
        public C4Skeet(float xpos, float ypos) : base(xpos, ypos)
        {
            weight = 8f;
            graphic = new Sprite(GetPath("c4skeet"));
            center = new Vec2(4f, 2f);
            collisionOffset = new Vec2(-4f, -2f);
            collisionSize = new Vec2(8f, 4f);
        }

        public override void Update()
        {
            base.Update();
            if (Rando.Float(0f, 1f) > 0.01) return;
            //else
            var sCfour = new Cfour(x, y)
            {
                hSpeed = Rando.Float(-7f, 7f),
                vSpeed = Rando.Float(-5f, 5f) - 15f,
                Weak = true
            };
            Level.Add(sCfour);
        }
    }
}