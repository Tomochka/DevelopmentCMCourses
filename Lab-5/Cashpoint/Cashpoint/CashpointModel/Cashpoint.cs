using System;
using System.Collections.Generic;
using System.Text;

namespace CashpointModel
{
    using System.Collections.Generic;
    delegate void CalculateGrants(uint banknote, byte count);

    public sealed class Cashpoint
    {
        private readonly Dictionary<uint, byte> _banknotes = new Dictionary<uint, byte>();
        private uint[] _granted = {1};
        private uint countBanknotes = 0;
        private uint _total = 0;
        private CalculateGrants calcGrants;

        public void AddBanknote(uint banknote)
        {
            AddBanknote(banknote, 1);
        }

        public void AddBanknote(uint banknote, byte count)
        {
            if (_banknotes.TryGetValue(banknote, out byte value))
            {
                _banknotes[banknote] = (byte)(count + value);
            }
            else
            {
                _banknotes.Add(banknote, count);
            }

            //Sieve
            calcGrants = SieveForAdd;
            calcGrants(banknote, count);

            _total += banknote * count;
            countBanknotes += count;
        }

        public void RemoveBanknote(uint banknote)
        {
            RemoveBanknote(banknote, 1);
        }

        public void RemoveBanknote(uint banknote, byte count)
        {
            if (_banknotes.TryGetValue(banknote, out byte value))
            {
                if (value >= count)
                {
                    _banknotes[banknote] = (byte)(value - count);

                    if ( _banknotes[banknote] == 0)
                    {
                        _banknotes.Remove(banknote);
                    }

                    //Sieve
                    calcGrants = SieveForRemove;
                    calcGrants(banknote, count);

                    _total -= banknote * count;
                    countBanknotes -= count;
                }
                else
                {
                    Console.WriteLine("Нет стольких купюр такого номинала");
                }
            }
            else 
            {
                Console.WriteLine("Нет купюры такого номинала");
            }
        }

        private void SieveForAdd(uint banknote, byte count)
        {
            for (var j = 1; j <= count; j++)
            {

                uint[] _grantedNext = new uint[_total + banknote * j + 1];

                Array.Resize<uint>(ref _granted, _grantedNext.Length);
                Array.Copy(_granted, _grantedNext, _granted.Length);


                for (var i = 0; i < _grantedNext.Length; i++)
                {
                    if (_granted[i] > 0)
                    {
                        _grantedNext[i + banknote] = _granted[i] + _granted[i + banknote];
                    }

                }

                Array.Copy(_grantedNext, _granted, _grantedNext.Length);
            }
        }

        private void SieveForRemove(uint banknote, byte count)
        {
            for (var j = 1; j <= count; j++)
            {
                Array.Resize<uint>(ref _granted, (int)(_granted.Length - banknote));

                for (var i = banknote; i < _granted.Length; i++)
                {
                    if (_granted[i] > 0)
                    {
                        var difference = i - banknote;

                        if (_granted[difference] > 0) _granted[i] -= _granted[difference];
                    }
                }
            }
        }

        public bool CanGrant(uint sum)
        {
            return sum > _total ? false : _granted[sum] > 0 ? true : false;
        }

        public uint CountBanknotes
        {
            get
            {
                return countBanknotes;
            }
        }

        public uint[] Sieve
        {
            get
            {
                return _granted;
            }
        }

        public uint Total
        {
            get
            {
                return _total;
            }
        }
    }
}

