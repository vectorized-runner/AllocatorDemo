using System;
using Unity.Collections;

namespace AllocatorDemo
{
	public struct LinearAllocator : IDisposable
	{
		private AllocatorHelper<LinearAllocatorImpl> _helper;
		
		// For ease-of-use
		public static implicit operator Allocator(LinearAllocator val) => val._helper.Allocator.ToAllocator;

		// For ease-of-use
		public static implicit operator AllocatorManager.AllocatorHandle(LinearAllocator val) =>
			val._helper.Allocator.Handle;

		public LinearAllocator(Span<byte> memory)
		{
			// Temp backing Allocator as LinearAllocator lives in stack anyway
			_helper = new AllocatorHelper<LinearAllocatorImpl>(Allocator.Temp);
			_helper.Allocator.Init(memory);
		}

		public void Dispose()
		{
			_helper.Allocator.Dispose();
			_helper.Dispose();
		}
	}
}