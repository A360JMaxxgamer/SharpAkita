using SharpAkita.Api.Query;
using SharpAkita.Api.Store;
using System;
using Xunit;

namespace SharpAkitaTest.Query
{
    public class BaseQueryTest
    {
        [Fact]
        public void SelectTest()
        {
            var store = new Store<int>(() => 0);
            var query = new BaseQuery<int>(store);
            var result = false;
            query.Select(s => s).Subscribe(i =>
            {
                if (i == 5)
                {
                    result = true;
                }
            });

            store.SetState(5);

            Assert.True(result);
        }
    }
}
