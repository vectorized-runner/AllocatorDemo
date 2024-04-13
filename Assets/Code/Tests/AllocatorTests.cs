using NUnit.Framework;
using Unity.Collections;

namespace AllocatorDemo
{
	public class AllocatorTests
	{
		[Test]
		public void CanCreateNativeArray()
		{
			Assert.DoesNotThrow(() =>
			{
				var allocator = new CustomAllocatorStruct(0);
				var arr = new NativeArray<int>(10, allocator);
				allocator.Dispose();
			});
		}

		[Test]
		public void ZeroInitialized()
		{
			var allocator = new CustomAllocatorStruct(0);
			var arr = new NativeArray<int>(10, allocator);

			Assert.Zero(arr[0]);

			allocator.Dispose();
		}
	}
}