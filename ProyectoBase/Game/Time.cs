using System;

namespace Game
{
    public static class Time
    {
        private static DateTime startTime;
        private static float lastFrameTime;
        public static float deltaTime;
        public static float DeltaTime { get => deltaTime; }

        public static void Inicialization()
        {

            startTime = DateTime.Now;

        }
        public static void CalculateDeltaTime()
        {
            TimeSpan CurrentTime = DateTime.Now - startTime;
            float CurrentSeconds = (float)CurrentTime.TotalSeconds;
            deltaTime = CurrentSeconds - lastFrameTime;
            lastFrameTime = CurrentSeconds;
        }
    }
}
