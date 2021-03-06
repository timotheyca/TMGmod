using System.Linq;
using DuckGame;
using JetBrains.Annotations;

namespace TMGmod.Stuff
{
    [EditorGroup("TMG|Misc")]
    [BaggedProperty("canSpawn", false)]
    [UsedImplicitly]
    public class Wire : PhysicsObject
    {

        private readonly SpriteMap _sprite;
        [UsedImplicitly]
        public int Teksturka;
        [UsedImplicitly]
        public StateBinding TexBinding = new StateBinding(nameof(Teksturka));
        [UsedImplicitly]
        public float Hp;
        [UsedImplicitly]
        public StateBinding HpBinding = new StateBinding(nameof(Hp));

        public Wire(float xpos, float ypos) : base(xpos, ypos)
        {
            Hp = 25f;
            _sprite = new SpriteMap(GetPath("Holiday/WireYesNY"), 48, 6); //if ny then NY else _
            _graphic = _sprite;
            Teksturka = Rando.Int(0, 3);
            _center = new Vec2(24f, 3f);
            _collisionOffset = new Vec2(-24f, -3f);
            _collisionSize = new Vec2(48f, 6f);
            thickness = 3f;
            _weight = 40f;
            throwSpeedMultiplier = 0f;
        }

        public override void Update()
        {
            if (Hp <= 0f) Hp = 0f;
            else
            {
                var probablyduck =
                    Level.CheckRectAll<IAmADuck>(position + new Vec2(-24f, -3f), position + new Vec2(24f, 3f));
                foreach (var realyduck in probablyduck)
                {
                    if (!(realyduck is Thing r1)) continue;
                    //else
                    r1.hSpeed *= 1f / (Hp / 26f + 1f);
                    r1.vSpeed *= 1f / (Hp / 10f + 1f);
                }

                var wirelist = Level.CheckRectAll<Wire>(position + new Vec2(-16f, -3f), position + new Vec2(16f, 3f));
                if (wirelist.Any(wire => wire != this && wire.sleeping && wire.Hp >= 1f && wire.position.y > position.y))
                {
                    sleeping = true;
                    vSpeed = 0f;
                    hSpeed *= 0.3f;
                }
                else
                {
                    sleeping = false;
                }
            }
            base.Update();
        }

        public override bool DoHit(Bullet bullet, Vec2 hitPos)
        {
            if (bullet.ammo.penetration < 10f) Damage(bullet.ammo);
            return Hit(bullet, hitPos);
        }
        private void Damage(AmmoType at)
        {
            if (at.penetration < 1.1f) return;
            Hp -= at.penetration;
            if (!(Hp < 1f)) return;
            //else
            thickness = 0.1f;
            graphic = new Sprite(GetPath("Holiday/WireNotNY"));
            collisionSize = new Vec2(48f, 4f);
        }

        public override void Draw()
        {
            _sprite.frame = Teksturka;
            base.Draw();
        }
    }
}