﻿using UnityEngine;

namespace AutoEmptyingExtended
{
    internal static class Logger
    {
        private const string Prefix = "AEE: ";

        public static void Log(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            Debug.Log(msg);
        }

        public static void LogWarning(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            Debug.LogWarning(msg);
        }

        public static void LogError(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            Debug.LogError(msg);
        }
    }
}