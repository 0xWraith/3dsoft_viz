using System;

namespace Utils
{
    /// <summary>
    /// Extension for classes that sends data on change event
    /// </summary>
    public class OnPropertyChanged<T> : EventArgs
    {
        public T Value { get; set; }
        public OnPropertyChanged(T value)
        {
            this.Value = value;
        }
    }
}
