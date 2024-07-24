using System.Windows;
using System.Windows.Controls;

namespace UI.Controls
{
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
            PopulateHours();
            PopulateMinutes();
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(TimeSpan?), typeof(TimePicker), new PropertyMetadata(null, OnSelectedTimeChanged));

        public TimeSpan? SelectedTime
        {
            get { return (TimeSpan?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TimePicker)d;
            control.UpdateListBoxSelection();
        }

        private void PopulateHours()
        {
            for (int i = 0; i < 24; i++)
            {
                HoursListBox.Items.Add(i.ToString("D2"));
            }
        }

        private void PopulateMinutes()
        {
            for (int i = 0; i < 60; i += 15)
            {
                MinutesListBox.Items.Add(i.ToString("D2"));
            }
        }

        private void UpdateListBoxSelection()
        {
            if (SelectedTime.HasValue)
            {
                HoursListBox.SelectedItem = SelectedTime.Value.Hours.ToString("D2");
                MinutesListBox.SelectedItem = SelectedTime.Value.Minutes.ToString("D2");
            }
        }

        private void HoursListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HoursListBox.SelectedItem != null && MinutesListBox.SelectedItem != null)
            {
                int hours = int.Parse(HoursListBox.SelectedItem.ToString());
                int minutes = int.Parse(MinutesListBox.SelectedItem.ToString());
                SelectedTime = new TimeSpan(hours, minutes, 0);
            }
        }

        private void MinutesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HoursListBox.SelectedItem != null && MinutesListBox.SelectedItem != null)
            {
                int hours = int.Parse(HoursListBox.SelectedItem.ToString());
                int minutes = int.Parse(MinutesListBox.SelectedItem.ToString());
                SelectedTime = new TimeSpan(hours, minutes, 0);
            }
        }
    }
}