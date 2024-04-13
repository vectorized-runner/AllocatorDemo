using System;
using NUnit.Framework;
using Unity.Collections;

namespace AllocatorDemo
{
	public class PersistentAllocatorTests
	{
		[Test]
		public void CanNotAccessArrayAfterDispose()
		{
			Assert.Throws<ObjectDisposedException>(() =>
			{
				var allocator = new PersistentAllocator(0);
				var arr = CollectionHelper.CreateNativeArray<int>(10, allocator);
				allocator.Dispose();
				var value = arr[0];
			});
		}
		
		[Test]
		public void CanNotCreateArrayAfterDispose()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var allocator = new PersistentAllocator(0);
				allocator.Dispose();
				var arr = CollectionHelper.CreateNativeArray<int>(10, allocator);
			});
		}
		
		[Test]
		public void CanDisposeAfterCreateArray()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new PersistentAllocator(0);
				var arr = CollectionHelper.CreateNativeArray<int>(10, allocator);
				allocator.Dispose();
			});
		}
		
		[Test]
		public void CanDoubleDispose()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new PersistentAllocator(0);
				allocator.Dispose();
				allocator.Dispose();
			});
		}
		
		[Test]
		public void CanDispose()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new PersistentAllocator(0);
				allocator.Dispose();
			});
		}
		
		[Test]
		public void CanCreateNativeArray()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new PersistentAllocator(0);
				CollectionHelper.CreateNativeArray<int>(10, allocator);
			});
		}

		[Test]
		public void IsZeroInitialized()
		{
			var allocator = new PersistentAllocator(0);
			var arr = CollectionHelper.CreateNativeArray<int>(10, allocator);
			Assert.Zero(arr[0]);
			allocator.Dispose();
		}
	}
}