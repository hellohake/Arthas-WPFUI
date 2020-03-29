using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using Arthas.Extensions;
using Arthas.Interop;

namespace Arthas.Controls
{
    public class MetroWindow : Window
    {
        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
        }

        public static readonly DependencyProperty CaptionContentProperty =
            DependencyProperty.Register(nameof(CaptionContent), typeof(object), typeof(MetroWindow));

        public object CaptionContent
        {
            get => GetValue(CaptionContentProperty);
            set => SetValue(CaptionContentProperty, value);
        }

        public static readonly DependencyProperty CaptionBackgroundProperty =
            DependencyProperty.Register(nameof(CaptionBackground), typeof(Brush), typeof(MetroWindow));

        public Brush CaptionBackground
        {
            get => (Brush)GetValue(CaptionBackgroundProperty);
            set => SetValue(CaptionBackgroundProperty, value);
        }

        public static readonly DependencyProperty CaptionForegroundProperty =
            DependencyProperty.Register(nameof(CaptionForeground), typeof(Brush), typeof(MetroWindow));

        public Brush CaptionForeground
        {
            get => (Brush)GetValue(CaptionForegroundProperty);
            set => SetValue(CaptionForegroundProperty, value);
        }

        public MetroWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, delegate
            {
                SystemCommands.MinimizeWindow(this);
            }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, delegate
            {
                SystemCommands.MaximizeWindow(this);
            }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, delegate
            {
                SystemCommands.RestoreWindow(this);
            }));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, delegate
            {
                SystemCommands.CloseWindow(this);
            }));

            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0, 0, 0, 1),
                NonClientFrameEdges = NonClientFrameEdges.None,
                ResizeBorderThickness = new Thickness(6),
                UseAeroCaptionButtons = false
            });

            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            RefreshMaxSize();
        }

        FrameworkElement PART_Content;
        FrameworkElement PART_Caption;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Content = GetTemplateChild(nameof(PART_Content)) as FrameworkElement;
            PART_Caption = GetTemplateChild(nameof(PART_Caption)) as FrameworkElement;

            PART_Caption.SetCaption();
        }

        IntPtr handle;
        HwndSource hwndSource;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            handle = new WindowInteropHelper(this).Handle;
            hwndSource = HwndSource.FromHwnd(handle);
            hwndSource?.AddHook(WndProc);

            RefreshMaxSize();
        }

        static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((WindowsMessages)msg)
            {
                case WindowsMessages.NCCALCSIZE:
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            RefreshMaxSize();
        }

        void RefreshMaxSize()
        {
            if (PART_Content == null)
                return;

            if (handle == IntPtr.Zero)
                return;

            if (WindowState == WindowState.Maximized)
            {
                var screen = Screen.FromHandle(handle);
                var matrix = hwndSource?.CompositionTarget?.TransformToDevice;
                if (!matrix.HasValue)
                    return;

                PART_Content.MaxWidth = Math.Max(screen.WorkingArea.Width / matrix.Value.M11, MinWidth);
                PART_Content.MaxHeight = Math.Max(screen.WorkingArea.Height / matrix.Value.M22, MinHeight);
            }
            else
            {
                PART_Content.MaxWidth = double.PositiveInfinity;
                PART_Content.MaxHeight = double.PositiveInfinity;
            }
        }
    }
}