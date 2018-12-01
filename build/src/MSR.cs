﻿using DuckGame;


namespace TMGmod.src
{
    [EditorGroup("TMG|Sniper")]
    public class MSR : Sniper
    {
		
        public MSR(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("MSR"));
            this.center = new Vec2(28.5f, 6f);
            this.collisionOffset = new Vec2(-28.5f, -6f);
            this.collisionSize = new Vec2(47f, 12f);
            this._barrelOffsetTL = new Vec2(48f, 5f);
            this.ammo = 5;
            this._ammoType = new ATSniper();
            this._ammoType.bulletSpeed = 85f;
            this._fireSound = GetPath("sounds/RifleOrMG.wav");
            this._fullAuto = false;
            this._kickForce = 1.85f;
            this.laserSight = false;
            this._laserOffsetTL = new Vec2(31f, 9f);
            this._holdOffset = new Vec2(14f, -1f);
            this._editorName = "MSR";
			this.weight = 4.65f;
			

        }

        public override void Draw()
        {
            float ang = this.angle;
            if (this.offDir <= 0)
            {
                this.angle = this.angle + this._angleOffset;
            }
            else
            {
                this.angle = this.angle - this._angleOffset;
            }
            base.Draw();
            this.angle = ang;
            this.laserSight = false;
        }

        public override void OnPressAction()
        {
            if (this.loaded)
            {
                base.OnPressAction();
                return;
            }
            if (this.ammo > 0 && this._loadState == -1)
            {
                this._loadState = 0;
                this._loadAnimation = 0;
            }
        }

        public override void Update()
        {
            base.Update();
            if (this._loadState > -1)
            {
                if (this.owner == null)
                {
                    if (this._loadState == 3)
                    {
                        this.loaded = true;
                    }
                    this._loadState = -1;
                    this._angleOffset = 0f;
                    this.handOffset = Vec2.Zero;
                }
                if (this._loadState == 0)
                {
                    if (!Network.isActive)
                    {
                        SFX.Play("loadSniper", 1f, 0f, 0f, false);
                    }
                    else if (base.isServerForObject)
                    {
                        this._netLoad.Play(1f, 0f);
                    }
                    Sniper sniper = this;
                    sniper._loadState = sniper._loadState + 1;
                }
                else if (this._loadState == 1)
                {
                    if (this._angleOffset >= 0.1f)
                    {
                        Sniper sniper1 = this;
                        sniper1._loadState = sniper1._loadState + 1;
                    }
                    else
                    {
                        this._angleOffset = this._angleOffset + 0.003f;
                    }
                }
                else if (this._loadState == 2)
                {
                    this.handOffset.x = this.handOffset.x - 0.2f;
                    if (this.handOffset.x > 4f)
                    {
                        Sniper sniper2 = this;
                        sniper2._loadState = sniper2._loadState + 1;
                        this.Reload(true);
                        this.loaded = false;
                    }
                }
                else if (this._loadState == 3)
                {
                    this.handOffset.x = this.handOffset.x + 0.2f;
                    if (this.handOffset.x <= 0f)
                    {
                        Sniper sniper3 = this;
                        sniper3._loadState = sniper3._loadState + 1;
                        this.handOffset.x = 0f;
                    }
                }
                else if (this._loadState == 4)
                {
                    if (this._angleOffset <= 0.03f)
                    {
                        this._loadState = -1;
                        this.loaded = true;
                        this._angleOffset = 0f;
                    }
                    else
                    {
                        this._angleOffset = MathHelper.Lerp(this._angleOffset, 0f, 0.15f);
                    }
                }
            }
            this.laserSight = false;
        }
	}
}