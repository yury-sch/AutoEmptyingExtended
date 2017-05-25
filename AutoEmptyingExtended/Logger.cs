using System;
using UnityEngine;
using ColossalFramework.Plugins;

namespace AutoEmptyingExtended
{
    internal static class Logger
    {
        private const string Prefix = "AutoEmptying Extended: ";

        public static void LogDebug(Func<string> acquire)
        {
        #if DEBUG
            var message = acquire();
            Debug.Log(Prefix + "(DEBUG) " + message);
        #endif
        }

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

        public static void LogInGame(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, msg);
        }
    }
}
