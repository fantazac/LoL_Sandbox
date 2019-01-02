using NUnit.Framework;

namespace Tests
{
    public class ResourceTest
    {
        private static readonly float RESOURCE_INITIAL_BASE_VALUE = 100;
        private static readonly float RESOURCE_VALUE = 10;

        private Resource resource;

        [SetUp]
        public void SetUp()
        {
            resource = new Resource(RESOURCE_INITIAL_BASE_VALUE);
        }

        [Test]
        public void GivenAnAmountGreaterThanCurrentValue_whenReduce_thenCurrentValueBecomesZero()
        {
            float amount = 2 * RESOURCE_INITIAL_BASE_VALUE;

            resource.Reduce(amount);

            float currentValue = resource.GetCurrentValue();
            Assert.AreEqual(0, currentValue);
        }

        [Test]
        public void GivenAnAmountLowerThanCurrentValue_whenReduce_thenCurrentValueIsUpdated()
        {
            float amount = RESOURCE_VALUE;

            resource.Reduce(amount);

            float currentValue = resource.GetCurrentValue();
            Assert.AreEqual(RESOURCE_INITIAL_BASE_VALUE - RESOURCE_VALUE, currentValue);
        }
    }
}
