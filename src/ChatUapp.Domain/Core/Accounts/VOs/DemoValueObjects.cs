using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.Accounts.VOs
{
    internal class DemoValueObjects : ValueObject
    {
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
