using System;

namespace Utils
{
    public static class ObjectUtils
    {
        public static T AssureNotNull<T>(T t) where T : class => t ?? throw new ArgumentNullException(nameof(t));
    }
}
