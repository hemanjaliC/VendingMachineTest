﻿namespace VendingMachine.Coins
{
    public class InvalidCoin : ICoin
    {
        public InvalidCoin(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}