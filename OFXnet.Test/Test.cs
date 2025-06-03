using Newtonsoft.Json;
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
             var testFilePath = @"C:\Users\sPPeecT\Desktop\Nova pasta\4.ofx";

            Extract extract = OFXnet.Core.Parser.GenerateExtract(testFilePath, new ParserSettings());
            var resultInJson = JsonConvert.SerializeObject(extract);
        }
    }
}