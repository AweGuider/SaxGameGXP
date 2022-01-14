using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

public class DeadZone : Sprite
{


    public DeadZone(TiledObject obj = null) : base("checkers.png")
    {
        collider.isTrigger = true;
        alpha = 0;
    }
}
