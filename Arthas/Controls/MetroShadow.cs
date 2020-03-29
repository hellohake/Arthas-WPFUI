using System.Windows;
using System.Windows.Controls;

namespace Arthas.Controls
{
    public class MetroShadow : Border
    {
        static MetroShadow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroShadow), new FrameworkPropertyMetadata(typeof(MetroShadow)));
        }
    }
}