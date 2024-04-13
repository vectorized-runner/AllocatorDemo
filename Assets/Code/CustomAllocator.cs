using System;
using Unity.Burst;
using Unity.Collections;
using static Unity.Collections.AllocatorManager;

namespace AllocatorDemo
{
	/// <summary>
	/// Block: Range + BytesPerItem + AllocatedItems + Alignment + Padding
	/// BytesPerItem: This is the sizeof the struct used for allocation?
	/// AllocatedItems: The items this Block has allocated
	/// (if Range.Items == AllocatedItems, then Block allocation is successful, otherwise allocation request isn't completed yet)
	/// Bytes: BytesPerItem * Range.Items. Total user-requested bytes
	/// Range: Ptr + Items + AllocatorHandle
	///	Ptr: Pointer to the allocated heap memory
	/// Items: Number of items allocated in the range
	/// AllocatorHandle: Allocator Function used to create the Range (self-referencing)
	/// </summary>
	[BurstCompile]
	public unsafe struct CustomAllocator : IAllocator
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

		private static int AllocatorFunction(IntPtr allocatorState, ref Block block)
		{
			return ((CustomAllocator*)allocatorState)->Try(ref block);
		}

		public void Dispose()
		{
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