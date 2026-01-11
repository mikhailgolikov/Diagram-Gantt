using Xunit;

namespace Gantt_Chart_Backend.Tests
{
    public class unitTest
    {
        [Fact] 
        public void Simple_Test()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Simple2_Test()
        {
            var result = 2 + 2;
            Assert.Equal(4, result);
        }
    }
}
