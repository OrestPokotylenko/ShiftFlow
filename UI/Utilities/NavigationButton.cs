using System.Windows;
using System.Windows.Controls;

namespace UI.Utilities
{
    public class NavigationButton : RadioButton
    {
        static NavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationButton), new FrameworkPropertyMetadata(typeof(NavigationButton)));
        }
    }
}
