using SMDataParser.Config;
using SMDataParser.Models;
using models = SMDataParser.Models;

namespace SMDataParser
{
    internal class SMDataParser
    {


        static void Main(string[] args)
        {

            AppConfig appConfig = new AppConfig();
            
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