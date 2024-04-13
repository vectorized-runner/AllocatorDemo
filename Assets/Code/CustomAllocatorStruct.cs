using Unity.Collections;

namespace AllocatorDemo
{
	public class CustomAllocatorStruct
	{
		public AllocatorHelper<CustomAllocator> Helper;

		public ref CustomAllocator RefAllocator => ref Helper.Allocator;

		public CustomAllocatorStruct()
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