using Linkeeper.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace Linkeeper.IntegrationTests
{
    public class LinkeeperTestsData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { new Link { Address = "https://www.test.com", Representation = "Test" } };
        }
    }
}
