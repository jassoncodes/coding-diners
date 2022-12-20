using SMDataParser.Models;
using models = SMDataParser.Models;

namespace SMDataParser
{
    internal class SMDataParser
    {


        static void Main(string[] args)
        {
            try
            {
                DataValidator dataValidator = new DataValidator();

                dataValidator.inputPath= args[0];
            }
            catch
            {

            }
        }
    }
}