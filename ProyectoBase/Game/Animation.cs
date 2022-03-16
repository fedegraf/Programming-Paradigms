using System.Collections.Generic;

namespace Game
{
    public class Animation
    {
        private string id;
        private List<Texture> frames;
        private float speed;
        private bool isLoopingEnable;
        private float currentAnimationTime = 0;
        private int currentFrameIndex = 0;

        public Texture CurrentFrame => frames[currentFrameIndex];
        public string Id => id;
        public Animation (string id, List<Texture> frames, float speed, bool canLoop)
        {
            this.id = id;
            this.speed = speed;
            this.frames = frames;
            isLoopingEnable = canLoop;
        }
        public void Update()
        {
            currentAnimationTime += Time.DeltaTime;
            if (currentAnimationTime >= speed)
            {
                currentFrameIndex++;
                currentAnimationTime = 0;
                if (currentFrameIndex >= frames.Count)
                {
                    //if loop is enable, start animation again. If not, get stuck in the last frame
                    currentFrameIndex = isLoopingEnable ? 0 : frames.Count - 1;
                }
            }
        }
        public void Play()
        {
            currentFrameIndex = 0;
            currentAnimationTime = 0;
        }
    }
}
