using Unity.Collections;

namespace AllocatorDemo
{
	public struct CustomAllocatorStruct
	{
		public AllocatorHelper<CustomAllocator> Helper;

		public ref CustomAllocator RefAllocator => ref Helper.Allocator;

		public bool IsCreated;

		// For ease-of-use
		public static implicit operator Allocator(CustomAllocatorStruct str) => str.RefAllocator.ToAllocator;

		// For ease-of-use
		public static implicit operator AllocatorManager.AllocatorHandle(CustomAllocatorStruct str) =>
			str.RefAllocator.Handle;

		public CustomAllocatorStruct(int dummy)
		{
			Helper = new AllocatorHelper<CustomAllocator>(Allocator.Persistent);
			IsCreated = true;
		}

		public void Dispose()
		{
			if (!IsCreated)
			{
				// throw new InvalidOperationException($"Attempting to Dispose already Destroyed or Uninitialized {nameof(CustomAllocatorStruct)}");
				return;
			}

			RefAllocator.Dispose();
			Helper.Dispose();
			IsCreated = false;
		}
	}
}