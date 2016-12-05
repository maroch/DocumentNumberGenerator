using System.ComponentModel;

namespace DocumentNumberGenerator.ViewModel
{
    /// <summary>
    /// Base View Model for all ViewModels
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Whether the view model should ignore property-change events.
        /// </summary>
        public virtual bool IgnorePropertyChangeEvents { get; set; }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        public virtual void RaisePropertyChangedEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanged == null) return;

            // Raise event
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">The name of the changing property.</param>
        public virtual void RaisePropertyChangingEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanging == null) return;

            // Raise event
            var e = new PropertyChangingEventArgs(propertyName);
            PropertyChanging(this, e);
        }
    }
}