using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

public class Block : AnimationSprite
{
    private static int index = 0;

    public Block(string path, int col, int row, TiledObject obj = null) : base(path, col, row)
    {
        //collider.isTrigger = true;
        index++;
    }

    public int GetIndex()
    {
        return index;
    }
}

public class Platform : Block
{
    public Platform(string path, int col, int row, TiledObject obj = null) : base(path, col, row)
    {
        collider.isTrigger = true;
    }
}

public class BreakSide : Block
{
    public BreakSide(string path, int col, int row, TiledObject obj = null) : base(path, col, row)
    {
        collider.isTrigger = false;
    }
}

public class BreakDown : Block
{
    public BreakDown(string path, int col, int row, TiledObject obj = null) : base(path, col, row)
    {
        collider.isTrigger = false;
    }
}
