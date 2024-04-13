using Unity.Collections;

namespace AllocatorDemo
{
	public struct PersistentAllocator
	{
		public bool IsCreated;

		private AllocatorHelper<PersistentAllocatorImpl> _helper;
		private ref PersistentAllocatorImpl Impl => ref _helper.Allocator;

		// For ease-of-use
		public static implicit operator Allocator(PersistentAllocator val) => val.Impl.ToAllocator;

		// For ease-of-use
		public static implicit operator AllocatorManager.AllocatorHandle(PersistentAllocator val) =>
			val.Impl.Handle;

		public PersistentAllocator(int dummy)
		{
			// This Allocator.Persistent parameter is used to create the Heap Memory for the Allocator function itself,
			// it's not related to the 'PersistentAllocatorImpl'
			_helper = new AllocatorHelper<PersistentAllocatorImpl>(Allocator.Persistent);
			IsCreated = true;
		}

		public void Dispose()
		{
			if (!IsCreated)
			{
				// throw new InvalidOperationException($"Attempting to Dispose already Destroyed or Uninitialized {nameof(CustomAllocatorStruct)}");
				return;
			}

			Impl.Dispose();
			_helper.Dispose();
			IsCreated = false;
		}
	}
}