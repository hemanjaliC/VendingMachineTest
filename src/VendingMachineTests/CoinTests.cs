using NUnit.Framework;
using VendingMachine;
using VendingMachine.Coins;

namespace VendingMachineTests
{
    [TestFixture]
    public class CoinTests
    {
        [Test]
        public void CreateWithValidDescriptionReturnsCoinWithAppropriateValue()
        {
            TestValidCoinCreation("quarter", 0.25m);
            TestValidCoinCreation("dime", 0.10m);
            TestValidCoinCreation("nickel", 0.05m);
        }

        [Test]
        public void CreateWithInvalidDescriptionReturnsFalse()
        {
            TestCoinCreationWithInvalidDescription("dollar");
            TestCoinCreationWithInvalidDescription("sfdeljknesv");
        }

        private static void TestCoinCreationWithInvalidDescription(string description)
        {
            ValidCoin validCoin;
            var success = ValidCoin.TryCreate(description, out validCoin);
            Assert.That(success, Is.EqualTo(false));
            Assert.That(validCoin, Is.EqualTo(null));
        }

        private static void TestValidCoinCreation(string description, decimal expectedValue)
        {
            ValidCoin validCoin;
            var success = ValidCoin.TryCreate(description, out validCoin);
            Assert.That(success, Is.EqualTo(true));
            Assert.That(validCoin.Value, Is.EqualTo(expectedValue));
        }
    }
}
