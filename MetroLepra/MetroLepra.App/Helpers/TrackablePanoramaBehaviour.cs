using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.Helpers
{
    public class TrackablePanoramaBehavior : Behavior<Panorama>
    {
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof (int), typeof (TrackablePanoramaBehavior),
                                        new PropertyMetadata(0, SelectedIndexPropertyChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (PanoramaItem), typeof (TrackablePanoramaBehavior),
                                        new PropertyMetadata(null, SelectedItemPropertyChanged));

        private Panorama _panorama;
        private bool _updatedFromUI;

        public PanoramaItem SelectedItem
        {
            get { return (PanoramaItem) GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // DP for binding index

        public int SelectedIndex
        {
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void SelectedItemPropertyChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            if (change.NewValue.GetType() != typeof (int) || dpObj.GetType() != typeof (TrackablePanoramaBehavior))
                return;

            var track = (TrackablePanoramaBehavior) dpObj;

            // If this flag is not checked, the panorama smooth transition is overridden
            if (!track._updatedFromUI)
            {
                var pan = track._panorama;

                pan.DefaultItem = change.NewValue;
            }

            track._updatedFromUI = false;
        }

        private static void SelectedIndexPropertyChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            if (change.NewValue.GetType() != typeof (int) || dpObj.GetType() != typeof (TrackablePanoramaBehavior))
                return;

            var track = (TrackablePanoramaBehavior) dpObj;

            // If this flag is not checked, the panorama smooth transition is overridden
            if (!track._updatedFromUI)
            {
                var pan = track._panorama;

                var index = (int) change.NewValue;

                if (pan.Items.Count > index)
                {
                    pan.DefaultItem = pan.Items[(int) change.NewValue];
                }
            }

            track._updatedFromUI = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _panorama = base.AssociatedObject;
            _panorama.SelectionChanged += PanoramaSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_panorama != null)
                _panorama.SelectionChanged += PanoramaSelectionChanged;
        }

        // Index changed by UI
        private void PanoramaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _updatedFromUI = true;
            SelectedIndex = _panorama.SelectedIndex;
            SelectedItem = (PanoramaItem) _panorama.SelectedItem;
        }
    }
}