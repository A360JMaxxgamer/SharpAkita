using SharpAkita.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharpAkitaTest.Data
{
    public class ObservableDictionaryTest
    {
        [Fact]
        public void OnPropertyChangingTest()
        {
            var propertyName = string.Empty;
            var dic = new ObservableDictionary<string, int>();
            dic.Add("1", 1);
            dic.PropertyChanging += (o, p) =>
            {
                propertyName = p.PropertyName;
            };

            dic["1"] = 2;

            Assert.Equal("Item[1]", propertyName);
        }

        [Fact]
        public void OnPropertyChangedTest()
        {
            var propertyName = string.Empty;
            var dic = new ObservableDictionary<string, int>();
            dic.Add("1", 1);
            dic.Add("2", 2);
            dic.PropertyChanged += (o, p) =>
            {
                propertyName = p.PropertyName;
            };

            dic["2"] = 3;

            Assert.Equal("Item[2]", propertyName);
        }
    }
}
