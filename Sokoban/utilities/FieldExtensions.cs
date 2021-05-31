#nullable enable
using System;

namespace Sokoban.utilities
{
    internal static class FieldExtensions
    {
        public static void SetAndOperation(Action setter, Action? operation)
        {
            setter.Invoke();
            operation?.Invoke();
        }

        public static T OperationIfConditionAndGet<T>(T field, bool condition, Action operation)
        {
            if (condition) operation();
            return field;
        }
    }
}