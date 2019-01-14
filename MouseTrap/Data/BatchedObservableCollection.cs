using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MouseTrap.Data
{
	public class BatchedObservableCollection<T> : ObservableCollection<T>
	{
		// Clears the entire list and adds new items
		// This avoids updating the control observing the collection multiple times per operation
		public virtual void SetItems(ICollection<T> collection)
		{
			if (collection == null) return;
			if (collection.Count < 1) return;

			Items.Clear();

			foreach (var item in collection)
			{
				Items.Add(item);
			}

			OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
