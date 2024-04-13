using Unity.Collections;

namespace AllocatorDemo
{
	public struct CustomAllocatorStruct
	{
		public AllocatorHelper<CustomAllocator> Helper;

		public ref CustomAllocator RefAllocator => ref Helper.Allocator;

		// For ease-of-use
		public static implicit operator Allocator(CustomAllocatorStruct str) => str.RefAllocator.ToAllocator;

		public CustomAllocatorStruct(int dummy)
		{
			Helper = new AllocatorHelper<CustomAllocator>(Allocator.Persistent);
		}

		public void Dispose()
		{
			RefAllocator.Dispose();
			Helper.Dispose();
		}
	}
}