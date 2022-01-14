using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

public class Block : Sprite
{
    public Block(Texture2D texture, TiledObject obj = null) : base(texture)
    {
            
    }
}

public class Platform : Block
{
    public Platform(Texture2D texture, TiledObject obj = null) : base(texture)
    {
        //collider.isTrigger = true;
    }
}
