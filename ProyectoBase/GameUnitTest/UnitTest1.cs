using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Game;

namespace GameUnitTest
{
    [TestClass]
    public class TestCollider
    {
        [TestMethod]
        public void TestCollition()
        {
            Vector2 PositionA = new Vector2(0, 0);
            Vector2 SizeA = new Vector2(1, 1);
            Vector2 PositionB = new Vector2(5, 5);
            Vector2 SizeB = new Vector2(1, 1);
            
            bool result = CollisionsUtilities.isBoxingColliding(PositionA, SizeA, PositionB, SizeB);

            Assert.IsTrue(!result);
        }
    }
}
