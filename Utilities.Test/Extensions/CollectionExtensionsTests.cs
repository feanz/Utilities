using System.Linq;
using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Utilities.Test.Extensions
{
	public class CollectionExtensionsTests
	{
		 [Fact]
		 public void Batch_Should_split_into_corret_number_of_batches()
		 {
			 var list = Enumerable.Range(1, 99);

			 var batches = list.Batch(10).ToList();
			 
			 batches.Count().Should().Be(10);
			 batches.Count(ints => ints.Count() == 10).Should().Be(9);
			 batches.Last().Count().Should().Be(9);
		 }
	}
}