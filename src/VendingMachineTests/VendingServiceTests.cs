using System;
using System.Linq;
using NUnit.Framework;
using VendingMachine;
using VendingMachine.enums;

namespace VendingMachineTests
{
   [TestFixture]
    public class VendingServiceTests
    {
        private VendingService vendingService;

        [SetUp]
        public void CreateAndStockVendingMachine()
        {
            var vm = new VendingService();
            vm.AddStock(Item.Pepsi);
            vm.AddStock(Item.Soda);
            vm.AddStock(Item.Coke);
            this.vendingService = vm;
        }

        [Test]
        public void WhenVendingMachineIsCreatedItDisplaysInsertCoin()
        {
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenQuarterIsInsertedItDisplays25Cents()
        {
            vendingService.AcceptCoin("quarter");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.25"));
        }

        [Test]
        public void WhenDimeIsInsertedItDisplays10Cents()
        {
            vendingService.AcceptCoin("dime");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.10"));
        }

        [Test]
        public void WhenCoinsAreInsertedItDisplaysTotalValue()
        {
            vendingService.AcceptCoin("dime");
            vendingService.AcceptCoin("quarter");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.35"));
        }

        [Test]
        public void WhenCoinsAreInsertedItDisplaysTotalValue2()
        {
            vendingService.AcceptCoin("dime");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("nickel");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.40"));
        }

        [Test]
        public void WhenInvalidCoinIsInsertedItIsPlacedInTheCoinReturn()
        {
            vendingService.AcceptCoin("dollar");
            Assert.That(vendingService.EmptyCoinReturn(), Is.EquivalentTo(new[] { "dollar" }));

            vendingService.AcceptCoin("sfdeljknesv");
            vendingService.AcceptCoin("dollar");
            vendingService.AcceptCoin("dollar");
            Assert.That(vendingService.EmptyCoinReturn(), Is.EquivalentTo(new[] { "dollar", "dollar", "sfdeljknesv" }));
        }

        [Test]
        public void WhenProductIsSelectedMachineDisplaysProductPriceThenStandardDisplay()
        {
            vendingService.SelectProduct(Item.Coke);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("PRICE £0.25"));

            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenProductIsSelectedMachineDisplaysProductPriceThenStandardDisplay2()
        {
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("PRICE £0.45"));

            vendingService.SelectProduct(Item.Pepsi);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("PRICE £0.35"));

            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenProductIsSelectedAndInsufficientValueMachineDisplaysProductPriceThenValueInserted()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("PRICE £0.45"));

            vendingService.SelectProduct(Item.Pepsi);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("PRICE £0.35"));

            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.25"));
        }

        [Test]
        public void WhenProductIsSelectedAndEqualValueInsertedThenDisplayThankYouAndSetValueToZero()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("THANK YOU"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenProductIsSelectedAndGreaterValueInsertedThenDisplayThankYouAndSetValueToZero()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("nickel");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.55"));
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("THANK YOU"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenProductIsSelectedAndGreaterValueInsertedThenExcessGoesToCoinReturn()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("nickel");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.55"));
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("THANK YOU"));
            var returnedCoins = vendingService.EmptyCoinReturn().ToList();

            // I just need to confirm that the correct value was dispensed
            foreach (var returnedCoin in returnedCoins)
            {
                vendingService.AcceptCoin(returnedCoin);
            }
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.10"));
        }

        [Test]
        public void WhenCoinsAreInsertedAndReturnCoinsIsSelectedThenCoinValueGoesToCoinReturn()
        {
            vendingService.AcceptCoin("nickel");
            vendingService.AcceptCoin("dime");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.15"));

            vendingService.ReturnCoins();
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
            var expectedCoins = new[] { "nickel", "dime" };
            Assert.That(vendingService.EmptyCoinReturn(), Is.EquivalentTo(expectedCoins));
        }
    }
}
