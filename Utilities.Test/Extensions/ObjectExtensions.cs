using Utilities.Extensions;
using Xunit;

namespace Utilities.Test.Extensions
{
	public class ObjectExtensions
	{
		[Fact]
		public void As_should_cast_empty_string_to_nullable_int()
		{
			var source = "";

			var actual = source.As<int?>();

			Assert.False(actual.HasValue);
		}
	}
}