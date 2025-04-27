global using static AnalyzerUI.Util;

namespace AnalyzerUI
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Static class to expose global utility functions
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// Allows UI events to be processed before the "next" Action is performed.
        /// </summary>
        /// <param name="next">The action to perform after the events are processed.</param>
        public static void AllowEvents(Action next)
        {
            DispatcherFrame frame = new();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(_ =>
            {
                next();
                return null;
            }),
            frame);

            Dispatcher.PushFrame(frame);
        }
    }
}
