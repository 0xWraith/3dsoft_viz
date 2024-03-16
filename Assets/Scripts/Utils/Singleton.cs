namespace Utils
{
    public abstract class SingletonBase<T> : BaseScript where T : BaseScript
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                return instance;
            }
        }
    }
    public abstract class Singleton<T> where T : new()
    {
        public static T Instance { get; } = new T();
    }
}
