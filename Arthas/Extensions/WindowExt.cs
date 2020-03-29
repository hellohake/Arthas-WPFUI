using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Arthas.Interop;

namespace Arthas.Extensions
{
    public static class WindowExt
    {
        public static void SetCaption(this FrameworkElement element)
        {
            if (element == null)
                return;

            element.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                var window = Window.GetWindow(element);
                if (window == null)
                    return;

                var handle = new WindowInteropHelper(window).Handle;
                if (handle == IntPtr.Zero)
                    return;

                if (e.ClickCount == 2)
                    switch (window.ResizeMode)
                    {
                        case ResizeMode.CanResize:
                        case ResizeMode.CanResizeWithGrip:

                            switch (window.WindowState)
                            {
                                case WindowState.Normal:
                                    SystemCommands.MaximizeWindow(window);
                                    break;

                                case WindowState.Maximized:
                                    SystemCommands.RestoreWindow(window);
                                    break;
                            }

                            break;
                    }
                else
                    NativeMethods.SendMessage(handle, WindowsMessages.NCLBUTTONDOWN, (IntPtr)HitTest.CAPTION, IntPtr.Zero);
            };
        }
    }
}