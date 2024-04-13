using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace AllocatorDemo
{
	/// <summary>
	/// No-Dispose allocator that works withs stack memory
	/// </summary>
	[BurstCompile]
	public unsafe struct LinearAllocatorImpl : IAllocator
	{
		private AllocatorHandle _handle;

		public Allocator ToAllocator => _handle.ToAllocator;
		public bool IsCustomAllocator => _handle.IsCustomAllocator;
		public bool IsAutoDispose => true;
		public TryFunction Function => AllocatorFunction;

		private void* _ptr;
		private int _length;
		private int _allocated;
		
		public AllocatorHandle Handle
		{
			get => _handle;
			set => _handle = value;
		}
		
		public void Init(Span<byte> memory)
		{
			_ptr = UnsafeUtility.AddressOf(ref memory[0]);
			_length = memory.Length;
			_allocated = 0;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public int Try(ref Block block)
		{
			// Allocate
			if (block.Range.Pointer == IntPtr.Zero)
			{
				// Debug.Log($"Requested {block.Bytes}");
				
				if (_allocated + block.Bytes > _length)
				{
					return -1;
				}

				block.Range.Pointer = (IntPtr)((byte*)_ptr + _allocated);
				block.AllocatedItems = block.Range.Items;
				_allocated += (int)block.Bytes;
				return 0;
			}

			// Deallocate or Reallocate not supported
			return -1;
		}

		[BurstCompile]
		private static int AllocatorFunction(IntPtr allocatorState, ref Block block)
		{
			return ((LinearAllocatorImpl*)allocatorState)->Try(ref block);
		}
	}
}