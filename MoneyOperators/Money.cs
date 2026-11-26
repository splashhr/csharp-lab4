using System;
using System.Globalization;

namespace MoneyOperators
{
    public class Money
    {
        private const int KopeksInRuble = 100;

        private readonly uint _rubles;
        private readonly byte _kopeks;

        public Money()
            : this(0, 0)
        {
        }

        public Money(uint rubles, byte kopeks)
        {
            if (kopeks >= KopeksInRuble)
            {
                throw new ArgumentOutOfRangeException(nameof(kopeks), "Копеек должно быть от 0 до 99.");
            }

            _rubles = rubles;
            _kopeks = kopeks;
        }

        public Money(Money other)
            : this(other.Rubles, other.Kopeks)
        {
        }

        public uint Rubles
        {
            get
            {
                return _rubles;
            }
        }

        public byte Kopeks
        {
            get
            {
                return _kopeks;
            }
        }

        public long TotalKopeks
        {
            get
            {
                return (long)_rubles * KopeksInRuble + _kopeks;
            }
        }

        public static Money operator ++(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);
            return FromKopeks(money.TotalKopeks + 1);
        }

        public static Money operator --(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);
            return FromKopeks(money.TotalKopeks - 1);
        }

        public static implicit operator uint(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);
            return money._rubles;
        }

        public static explicit operator double(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);
            return money._kopeks / (double)KopeksInRuble;
        }

        public static Money operator -(Money money, int kopeks)
        {
            ArgumentNullException.ThrowIfNull(money);
            return FromKopeks(money.TotalKopeks - kopeks);
        }

        public static Money operator -(int kopeks, Money money)
        {
            return money - kopeks;
        }

        public bool IsEqualTo(Money other)
        {
            ArgumentNullException.ThrowIfNull(other);
            return TotalKopeks == other.TotalKopeks;
        }

        public bool IsGreaterThan(Money other)
        {
            ArgumentNullException.ThrowIfNull(other);
            return TotalKopeks > other.TotalKopeks;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} руб. {1:00} коп.", _rubles, _kopeks);
        }

        private static Money FromKopeks(long totalKopeks)
        {
            if (totalKopeks < 0)
            {
                throw new InvalidOperationException("Денежная сумма не может быть меньше 0.");
            }

            var rubles = checked((uint)(totalKopeks / KopeksInRuble));
            return new Money(rubles, (byte)(totalKopeks % KopeksInRuble));
        }
    }
}
