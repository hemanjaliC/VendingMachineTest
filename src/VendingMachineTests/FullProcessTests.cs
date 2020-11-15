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
    public class FullProcessTests
    {
        [Test]
        public void CustomerIsAbleToReturnCoinsAfterSelectingSoldOutProduct()
        {
            var vendingService = new VendingService();
            vendingService.AddStock(Item.Pepsi);
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("quarter");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£1.00"));

            vendingService.SelectProduct(Item.Coke);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£1.00"));

            vendingService.ReturnCoins();
            Assert.That(vendingService.EmptyCoinReturn(), Is.EquivalentTo(new[] { "quarter", "quarter", "quarter", "quarter" }));
        }

        [Test]
        public void CustomerIsAbleToSelectProductAndReceiveChange()
        {
            var vendingService = new VendingService();
            vendingService.AddStock(Item.Pepsi);
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("penny");
            vendingService.AcceptCoin("quarter");
            vendingService.AcceptCoin("dime");
            vendingService.AcceptCoin("dime");
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.71"));

            vendingService.SelectProduct(Item.Pepsi);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("THANK YOU"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("INSERT COIN"));
            var change = vendingService.EmptyCoinReturn();
            foreach (var coin in change)
            {
                vendingService.AcceptCoin(coin);
            }

            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.36"));
            vendingService.SelectProduct(Item.Pepsi);
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("SOLD OUT"));
            Assert.That(vendingService.GetDisplay(), Is.EqualTo("£0.36"));
        }
    }
}
