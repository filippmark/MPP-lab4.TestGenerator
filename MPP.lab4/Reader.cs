using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorImpl
{
    public class Reader
    {
        public Reader()
        {

        }

        public async Task<string> ReadCodeFromFile(string pathToFile)
        {
            string code = null;
            using (var reader = File.OpenText(pathToFile))
            {
                code = await reader.ReadToEndAsync();
                return code;
            }
        }
    }
}
