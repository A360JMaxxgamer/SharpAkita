using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharpAkitaTest.Extensions
{
    public class ObjectExtensionsTest
    {
        [Fact]
        public void CopyTest()
        {
            var toCopy = new A
            {
                AString = "Test",
                B = new B
                {
                    BString = "Blub",
                    Cs = new List<C>
                    {
                        new C { CInt =1 },
                        new C { CInt = 5}
                    }
                }
            };
            toCopy.Self = toCopy;

            var copy = toCopy.Copy();
            toCopy.AString = "Not same reference";

            Assert.NotEqual("Not same reference", copy.AString);
            Assert.Equal("Test", copy.AString);
            Assert.Equal("Blub", copy.B.BString);
            Assert.Equal(1, copy.B.Cs[0].CInt);
            Assert.Equal(5, copy.B.Cs[1].CInt);
            Assert.Equal(2, copy.B.Cs.Count);
        }
    }

    internal class A
    {
        public string AString { get; set; }

        public B B { get; set; }

        public A Self { get; set; }
    }

    internal class B
    {
        public string BString { get; set; }

        public List<C> Cs { get; set; }
    }

    internal class C
    {
        public int CInt { get; set; }
    }
}
