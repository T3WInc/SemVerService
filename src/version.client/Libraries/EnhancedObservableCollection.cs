using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace version.client.Libraries
{
    public class EnhancedObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
    {
        private bool disableNotifications = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!disableNotifications)
                base.OnCollectionChanged(e);
        }

        public void DisableNotifications()
        {
            disableNotifications = true;
        }

        public void EnableNotifications(bool sendNotification = false)
        {
            disableNotifications = false;
            if (sendNotification)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<T> collection)
        {
            var temporarilyDisableNotifications = !disableNotifications;
            if (temporarilyDisableNotifications)
                disableNotifications = true;

            foreach (T item in collection)
                Add(item);

            if (temporarilyDisableNotifications)
                disableNotifications = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public EnhancedObservableCollection() : base() { }
        public EnhancedObservableCollection(IEnumerable<T> collection) : base(collection) { }
    }
}
