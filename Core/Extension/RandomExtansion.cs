using System;

namespace Core.Extension {
    public static class RandomExtansion {
        public static DateTime NextDate(this Random random, DateTime from, DateTime to) {
            var range = to - from;
            var randTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return from + randTimeSpan;
        }

        public static long NextLong(this Random random, long min, long max) {
            if(max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while(ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        public static decimal NextDecimal(this Random random, decimal min, decimal max) {
            byte fromScale = new System.Data.SqlTypes.SqlDecimal(min).Scale;
            byte toScale = new System.Data.SqlTypes.SqlDecimal(max).Scale;

            byte scale = (byte)(fromScale + toScale);
            if(scale > 28)
                scale = 28;

            decimal r = new decimal(random.Next(), random.Next(), random.Next(), false, scale);
            if(Math.Sign(min) == Math.Sign(max) || min == 0 || max == 0)
                return decimal.Remainder(r, max - min) + min;

            bool getFromNegativeRange = (double)min + random.NextDouble() * ((double)max - (double)min) < 0;
            return getFromNegativeRange ? decimal.Remainder(r, -min) + min : decimal.Remainder(r, max);
        }
    }
}
