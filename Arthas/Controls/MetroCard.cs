using System.Windows;
using System.Windows.Controls;

namespace Arthas.Controls
{
    public class MetroCard : GroupBox
    {
        static MetroCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroCard), new FrameworkPropertyMetadata(typeof(MetroCard)));
        }
    }
}