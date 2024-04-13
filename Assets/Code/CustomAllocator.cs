using System;
using Unity.Burst;
using Unity.Collections;
using static Unity.Collections.AllocatorManager;

namespace AllocatorDemo
{
	[BurstCompile]
	public struct CustomAllocator : IAllocator
	{
		private AllocatorHandle _handle;

		public AllocatorHandle Handle
		{
			get => _handle;
			set => _handle = value;
		}
		
		public bool IsCustomAllocator => _handle.IsCustomAllocator;
		public Allocator ToAllocator => _handle.ToAllocator;
		public bool IsAutoDispose => false;

		public TryFunction Function => AllocatorFunction;

		private int AllocatorFunction(IntPtr allocatorState, ref Block block)
		{
			return -1;
		}

		public void Dispose()
		{
			// TODO: Ensure no memory leaks?
			
			_handle.Dispose();
		}

		// Could be used?
		private int _allocationCount;

		// This method allocates or deallocates (why are they together?)
		public int Try(ref Block block)
		{
			int error = 0;
			
			// If Pointer is zero, need to Allocate
			if (block.Range.Pointer == IntPtr.Zero)
			{
				// This Example uses Allocator.Persistent for allocation
				var originalAllocator = block.Range.Allocator;
				block.Range.Allocator = Persistent;
				error = AllocatorManager.Try(ref block);
				block.Range.Allocator = originalAllocator;

				if (error != 0)
					return error;

				if (block.Range.Pointer != IntPtr.Zero)
				{
					_allocationCount++;
				}

				// Successful
				return 0;
			}
			// Deallocate
			else
			{
				var originalAllocator = block.Range.Allocator;
				block.Range.Allocator = Persistent;
				error = AllocatorManager.Try(ref block);
				block.Range.Allocator = originalAllocator;

				if (error != 0)
					return error;

				if (block.Range.Pointer == IntPtr.Zero)
				{
					_allocationCount--;
				}

				return 0;
			}
		}
	}
}