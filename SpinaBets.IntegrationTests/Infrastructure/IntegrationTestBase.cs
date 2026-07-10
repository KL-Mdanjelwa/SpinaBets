using System;
using System.Collections.Generic;
using System.Text;

namespace SpinaBets.IntegrationTests.Infrastructure
{
    public abstract class IntegrationTestBase
    {
        protected readonly TestWebApplicationFactory Factory;
        protected readonly HttpClient Client;

        protected IntegrationTestBase()
        {
            Factory = new TestWebApplicationFactory();
            Client = Factory.CreateClient();
        }
    }
}
