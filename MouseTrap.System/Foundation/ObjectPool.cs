using System;
using System.Collections.Concurrent;

namespace MouseTrap.Foundation
{
	/// <summary>
	/// Object pool
	/// </summary>
	/// <remarks>https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool</remarks>
	/// <typeparam name="T"></typeparam>
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
