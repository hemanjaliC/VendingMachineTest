﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Coins;
using VendingMachine.enums;

namespace VendingMachine
{
    public class VendingService
    {
        private string oneTimeDisplay = null;
        private readonly List<ICoin> coinReturnContents = new List<ICoin>();
        private readonly List<Item> stock = new List<Item>();
        private readonly List<ValidCoin> currentSessionCoins = new List<ValidCoin>();

        public string GetDisplay()
        {
            if (oneTimeDisplay != null)
            {
                var temp = oneTimeDisplay;
                oneTimeDisplay = null;
                return temp;
            }

            var currentSessionValue = GetCurrentSessionCoinValue();
            if (currentSessionValue == 0)
            {
                return "INSERT COIN";
            }

            return currentSessionValue.ToString("C");
        }

        public void AcceptCoin(string coinPhysicalDescription)
        {
            ValidCoin validCoin;
            if (ValidCoin.TryCreate(coinPhysicalDescription, out validCoin))
            {
                this.currentSessionCoins.Add(validCoin);
            }
            else
            {
                var invalidCoin = new InvalidCoin(coinPhysicalDescription);
                coinReturnContents.Add(invalidCoin);
            }
        }

        public void SelectProduct(Item productName)
        {
            decimal price = GetProductPrice(productName);
            if (!stock.Contains(productName))
            {
                oneTimeDisplay = "SOLD OUT";
                return;
            }

            var currentSessionValue = GetCurrentSessionCoinValue();
            var difference = currentSessionValue - price;
            if (difference >= 0)
            {
                stock.Remove(productName);
                oneTimeDisplay = "THANK YOU";
                
                // TODO: This "throws away" the inserted coins. We cannot determine the total coins in the machine.
                currentSessionCoins.Clear();
                var changeCoins = MakeChange(difference);
                coinReturnContents.AddRange(changeCoins);
                return;
            }

            this.oneTimeDisplay = $"PRICE {price:C}";
        }

        public IEnumerable<string> EmptyCoinReturn()
        {
            var contents = coinReturnContents.Select(x => x.Description).ToList();
            coinReturnContents.Clear();
            return contents;
        }

        private IEnumerable<ValidCoin> MakeChange(decimal difference)
        {
            var pennyCount = (int)((difference % 0.05m)*100);
            var nickelCount = (int) (difference/0.05m);
            for (int i = 0; i < nickelCount; i++)
            {
                yield return new ValidCoin(ValidCoinType.Nickel, "nickel");
            }
            for (int i = 0; i < pennyCount; i++)
            {
                yield return new ValidCoin(ValidCoinType.Penny, "penny");
            }

        }

        public void ReturnCoins()
        {
            coinReturnContents.AddRange(this.currentSessionCoins);
            this.currentSessionCoins.Clear();
        }

        public void AddStock(Item product)
        {
            this.stock.Add(product);
        }

        private decimal GetCurrentSessionCoinValue()
        {
            return this.currentSessionCoins.Sum(x => x.Value);
        }

        private decimal GetProductPrice(Item productName)
        {
            switch (productName)
            {
                case Item.Coke:
                    return .25m;
                case Item.Pepsi:
                    return .35m;
                case Item.Soda:
                    return .45m;
                default:
                    throw new ArgumentOutOfRangeException(nameof(productName), productName, $"That product is not supported");
            }
        }
    }
}