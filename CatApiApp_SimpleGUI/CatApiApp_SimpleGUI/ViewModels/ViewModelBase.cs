// ViewModels/ViewModelBase.cs

// This file defines the ViewModelBase class, which provides base functionality 
// for all view models in the CatApiApp. It implements the INotifyPropertyChanged 
// interface to support property change notifications and data binding.

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace CatApiApp_SimpleGUI.ViewModels
{
    // A base class for view models that implements the INotifyPropertyChanged interface
    // to provide property change notifications for data binding in the UI.
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event triggered when a property value changes, notifying the UI of updates.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise the PropertyChanged event for a given property.
        // This is used to notify the UI that a property value has been updated.
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to update a property value and notify the UI if the value has changed.
        // This helps in reducing boilerplate code when setting properties in view models.
        protected bool RaiseAndSetIfChanged<T>(ref T field, T newValue, string propertyName)
        {
            // Check if the new value is different from the current field value.
            if (!Equals(field, newValue))
            {
                field = newValue; // Update the field with the new value.
                OnPropertyChanged(propertyName); // Notify the UI that the property has changed.
                return true; // Return true to indicate that the value was updated.
            }
            return false; // Return false if the value did not change.
        }
    }
}
