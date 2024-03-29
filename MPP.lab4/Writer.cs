﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorImpl
{
    public class Writer
    {
        private string directoryPath;

        public Writer(string direcroryPath)
        {
            this.directoryPath = direcroryPath;
            if (!Directory.Exists(direcroryPath))
                Directory.CreateDirectory(direcroryPath);
        }

        public async Task WriteTestsToFiles(List<TestClassDetails> tests)
        {
            if (tests != null)
            {
                foreach (var test in tests)
                {
                    byte[] encodedText = Encoding.UTF8.GetBytes(test.Code);
                    string pathToFile = directoryPath + @"\" + test.TestClassName;
                    using (FileStream sourceStream = new FileStream(pathToFile,
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true))
                    {
                        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    };
                }
            }

        }
    }
}
