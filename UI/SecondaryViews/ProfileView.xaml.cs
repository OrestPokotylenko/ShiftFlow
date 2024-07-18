using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace UI.SecondaryViews
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void DropDown_Click(object sender, EventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            DockPanel dockPanel = (DockPanel)button.Tag;

            if (dockPanel.Visibility == Visibility.Collapsed)
            {
                Storyboard showStoryboard = (Storyboard)Application.Current.Resources["ShowDockPanelStoryboard"];
                showStoryboard.Begin(dockPanel);
            }
            else
            {
                Storyboard hideStoryboard = (Storyboard)Application.Current.Resources["HideDockPanelStoryboard"];
                hideStoryboard.Begin(dockPanel);
            }
        }
    }
}