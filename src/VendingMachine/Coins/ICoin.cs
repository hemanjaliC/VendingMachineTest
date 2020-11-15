using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VendingMachine.Coins
{
    public interface ICoin
    {
        string Description { get; }
    }
}