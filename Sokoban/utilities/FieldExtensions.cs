#nullable enable
using System;

namespace Sokoban.utilities
{
    internal static class FieldExtensions
    {
        public static void SetAndOperation<T>(ref T field, T value, Action operation)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            field = value;
            operation();
        }

        public static T OperationIfConditionAndGet<T>(T field, bool condition, Action operation)
        {
            if (condition) operation();
            return field;
        }
    }
}
