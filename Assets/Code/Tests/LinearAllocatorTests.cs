using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace AllocatorDemo
{
	public class LinearAllocatorTests
	{
		[Test]
		public void CanCreate()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new LinearAllocator(stackalloc byte[1]);
			});
		}

		[Test]
		public void CanDispose()
		{
			Assert.DoesNotThrow(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[1]);
			});
		}

		[Test]
		public void CanCreateNativeArray()
		{
			Assert.DoesNotThrow(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[100]);
				var array = CollectionHelper.CreateNativeArray<int>(5, allocator);
			});
		}
		
		[Test]
		public void CanAccessNativeArray()
		{
			Assert.DoesNotThrow(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[100]);
				var array = CollectionHelper.CreateNativeArray<int>(5, allocator);
				
				Assert.Zero(array[0]);
				Assert.Zero(array[1]);
				Assert.Zero(array[2]);
				Assert.Zero(array[3]);
				Assert.Zero(array[4]);
			});
		}
		
		[Test]
		public void CanNotAllocateOutOfBounds()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[100]);
				var array = CollectionHelper.CreateNativeArray<byte>(101, allocator);
			});
		}
		
		[Test]
		public void CanAllocateUnsafeList()
		{
			Assert.DoesNotThrow(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[100]);
				var list = new UnsafeList<int>(10, allocator);
			});
		}

		[Test]
		public void CanAllocateMultiple()
		{
			Assert.DoesNotThrow(() =>
			{
				using var allocator = new LinearAllocator(stackalloc byte[256]);
				var list1 = new UnsafeList<byte>(64, allocator);
				var list2 = new UnsafeList<byte>(64, allocator);
				var list3 = new UnsafeList<byte>(64, allocator);
				var list4 = new UnsafeList<byte>(64, allocator);
			});
		}
	}
}