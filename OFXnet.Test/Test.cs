using OFXnet.Core;
using OFXnet.Domain;

namespace OFXnet.Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMethod1()
        {
             var testFilePath = @"C:\Users\sPPeecT\Desktop\Extrato28169591254.ofx";

            Extract extract = OFXnet.Core.Parser.GenerateExtract(testFilePath, new ParserSettings());
        }
    }
}