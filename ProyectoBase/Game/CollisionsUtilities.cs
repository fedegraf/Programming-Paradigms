using System;

namespace Game
{
    public class CollisionsUtilities
    {
        //Check if two objects, giving position and size of both, are colliding
        public static bool isBoxingColliding (Vector2 PositionA, Vector2 SizeA, Vector2 PositionB, Vector2 SizeB)
        {
            float DistanceX = Math.Abs(PositionA.X - PositionB.X);
            float DistanceY = Math.Abs(PositionA.Y - PositionB.Y);
            float SumHalfWidhts = SizeA.X / 2 + SizeB.X / 2;
            float SumHalfHeights = SizeA.Y / 2 + SizeB.Y / 2;

            return DistanceX <= SumHalfWidhts && DistanceY <= SumHalfHeights;
        }
    }
}
