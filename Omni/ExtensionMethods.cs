﻿using Microsoft.Xna.Framework;

namespace Omni
{
    public static class ExtensionMethods
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
