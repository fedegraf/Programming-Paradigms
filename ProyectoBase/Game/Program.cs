namespace Game
{
    public class Program
    {
        public static bool isPlaying = true;
        public static int SCREEN_WIDHT = 980;
        public static int SCREEN_HEIGHT = 760;
        public static float GRAVITY = 10f;

        static void Main(string[] args)
        {
            Inicialization();

            while (isPlaying)
            {
                Time.CalculateDeltaTime();
                Update();
                Render();
            }
        }
        private static void Inicialization()
        {
            Time.Inicialization();
            Engine.Initialize();
            GameManager.Instance.Initialize();
        }
        private static void Update()
        {
            GameManager.Instance.Update();
        }
        private static void Render()
        {
            Engine.Clear();
            GameManager.Instance.Render();
            Engine.Show();
        }

    }
}