﻿using System;
using System.Collections.Generic;
using DuckGame;
using TMGmod.Core;
using TMGmod.Cases.Color;
using TMGmod.Cases;

namespace TMGmod.NY
{
    [EditorGroup("TMG|Misc|Holiday")]
    public class NewYearCase : BaseCase
    {
        public NewYearCase(float xval, float yval) : base(xval, yval)
        {
            var sprite = new SpriteMap(GetPath("Holiday/HolydayCase"), 14, 8);
            _graphic = sprite;
            sprite.frame = Rando.Int(0, 31);
            _center = new Vec2(7f, 4f);
            _collisionOffset = new Vec2(-7f, -4f);
            _collisionSize = new Vec2(14f, 8f);
            depth = -0.5f;
            thickness = 0.0f;
            _weight = 4f;
            collideSounds.Add("presentLand");
            _editorName = "Holiday Case";
            Things = new List<Type>
            {
                //x4
                typeof(CandyCane),
                typeof(CandyCane),
                typeof(CandyCane),
                typeof(CandyCane),
                 //x3
                 typeof(GarlandGun),
                 typeof(GarlandGun),
                 typeof(GarlandGun),
                 typeof(Popcal),
                 typeof(Popcal),
                 typeof(Popcal),
                 typeof(Icer),
                 typeof(Icer),
                 typeof(Icer),
                 typeof(SnowMgun),
                 typeof(SnowMgun),  
                 typeof(SnowMgun),   
                  //x2
                  typeof(CandyCaneLime),
                  typeof(CandyCaneLime),
                  typeof(SkeetGun),  
                  typeof(SkeetGun),  
                  typeof(Helmet),
                  typeof(Helmet),  
                  typeof(ChestPlate),
                  typeof(ChestPlate), 
                   //x1
                   typeof(SpruceGun),
                   typeof(CandyCaneOrange),
                   typeof(PPLMG),
                   typeof(PodarokColorB),  
                   typeof(PodarokColorC),  
                   typeof(PodarokColorY),  
                   typeof(PodarokColorR),  
                   typeof(PodarokColorG),  
                   typeof(PodarokMillitary)  
            };
            CaseId = 6;
        }
    }
}