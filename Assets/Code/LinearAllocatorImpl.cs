using System;
using Unity.Burst;
using Unity.Collections;
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

		public AllocatorHandle Handle
		{
			get => _handle;
			set => _handle = value;
		}

		public void Dispose()
		{
			// No-op
		}

		public int Try(ref Block block)
		{
			// TODO:
			return -1;
		}

		[BurstCompile]
		private static int AllocatorFunction(IntPtr allocatorState, ref Block block)
		{
			return ((LinearAllocatorImpl*)allocatorState)->Try(ref block);
		}
	}
}