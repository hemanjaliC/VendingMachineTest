using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine;
using VendingMachine.enums;

namespace VendingMachineTests
{
    [TestFixture]
    public class InStockTests
    {
        private VendingService vendingService;

        [SetUp]
        public void CreateVendingMachine()
        {
            this.vendingService = new VendingService();
        }

        [Test]
        public void WhenSelectedProductIsOutOfStockThenDisplaySoldOut()
        {
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
        }

        [Test]
        public void WhenSelectedProductIsOutOfStockAndSufficientValueInsertedThenDisplaySoldOut()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
        }

        [Test]
        public void WhenSelectedProductIsNotOutOfStockAndSufficientValueInsertedThenProductIsDispensedAndRemovedFromStock()
        {
            vendingService.AddStock(Item.Soda);
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("THANK YOU"));

            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
        }

        [Test]
        public void WhenOtherProductIsInStockThenDisplaySoldOut()
        {
            vendingService.AddStock(Item.Coke);
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
        }

        [Test]
        public void WhenSoldOutProductIsSelectedThenDisplayReturnsToDefaultAfterDisplayingSoldOut()
        {
            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
        }

        [Test]
        public void WhenSoldOutProductIsSelectedThenDisplayReturnsToDefaultAfterDisplayingSoldOut2()
        {
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("nickel");

            vendingService.SelectProduct(Item.Soda);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.55"));
        }
    }
}
