using System;

namespace SecretNest.RemoteAgency
{
    static class RandomizedName
    {
        private const string RandomizedNameFormat = "{0}_{1:N}";

        internal static string GetRandomizedName(string prefix)
        {
            return string.Format(RandomizedNameFormat, prefix, Guid.NewGuid());
        }
    }
}
