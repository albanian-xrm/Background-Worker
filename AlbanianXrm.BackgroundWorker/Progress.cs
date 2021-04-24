namespace AlbanianXrm.BackgroundWorker
{
    public class Progress<T>
    {
        public Progress(int percentage)
        {
            this.Percentage = percentage;
        }

        public Progress(int percentage, T userState) : this(percentage)
        {
            this.UserState = userState;
        }

        public int Percentage { get; private set; }
        public T UserState { get; private set; }
    }
}
