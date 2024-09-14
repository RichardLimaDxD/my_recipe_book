using AutoMapper;
using Sqids;

namespace CommonTestUtilities.IdEncryption
{
    public class IdEncripterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "ilLCq1KH6G9U8kAd4jXWIcvTDso7bS2pYfPgmVrJOyRwEaMuQZ5txNnBzeh3F"
            });
        }
    }
}
