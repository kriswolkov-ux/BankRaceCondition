using System;
using System.Threading;

namespace BankRaceCondition.Models;

public sealed class BankAccount
{
    private decimal _balance;
    private readonly object _sync = new();

    public decimal Balance
    {
        get
        {
            lock (_sync)
            {
                return _balance;
            }
        }
    }

    public void DepositUnsafe(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        decimal currentBalance = _balance;
        Thread.SpinWait(1000); // Искусственная задержка для провоцирования гонки
        _balance = currentBalance + amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        lock (_sync)
        {
            _balance += amount;
        }
    }
}