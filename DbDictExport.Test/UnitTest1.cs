using System;
using System.Text;
using DbDictExport.Core.Codes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbDictExport.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var camel = AbstractKdCodeFactory.ToCamelCase("HelloWorld");
            var pascal = AbstractKdCodeFactory.ToPascalCase("helloWorld");

            var idCamel = AbstractKdCodeFactory.ToCamelCase("ID");
            var idPascal = AbstractKdCodeFactory.ToPascalCase("id");
            

            Assert.AreEqual("helloWorld", camel);
            Assert.AreEqual("id", idCamel);

            Assert.AreEqual("HelloWorld", pascal);
            Assert.AreEqual("Id",idPascal);

        }
    }

}
