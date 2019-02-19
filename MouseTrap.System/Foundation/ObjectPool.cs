using System;
using System.Collections.Concurrent;

namespace MouseTrap.Foundation
{
	internal class ObjectPool<T>
	{
		private Func<T> ObjectGenerator { get; }
		private ConcurrentBag<T> Objects { get; }

		public ObjectPool(Func<T> objectGenerator)
		{
			ObjectGenerator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");
			Objects = new ConcurrentBag<T>();
		}

		public T GetObject()
		{
			if (Objects.TryTake(out T item)) return item;
			return ObjectGenerator();
		}

		public void PutObject(T item)
		{
			Objects.Add(item);
		}
	}
}
