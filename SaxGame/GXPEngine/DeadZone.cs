using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class DeadZone : GameObject
    {
        private float width;
        private float height;

        public DeadZone(float x, float y, float width, float height) : base(true)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
