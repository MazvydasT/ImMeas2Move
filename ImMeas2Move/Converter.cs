using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImMeas2Move
{
    public static class Converter
    {
        private static readonly Regex regEx = new Regex(@"^(?<PreImMeas>\s*?)ImMeas(?<PostImMeas>.+)\\n_znPosID:=\d+\\Tool:=(?<PostPosID>.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static async Task Convert(string inputPath, string outputPath, IProgress<ProgressUpdate> progress = default)
        {
            await Task.Factory.StartNew(() =>
            {
                //var encoding = Encoding.GetEncoding("Windows-1252");

                using (var fileStreamRead = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStreamRead))
                using (var fileStreamWrite = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                using (var streamWriter = new StreamWriter(fileStreamWrite))
                {
                    while(!streamReader.EndOfStream)
                    {
                        streamWriter.WriteLine(regEx.Replace(streamReader.ReadLine(), "${PreImMeas}Move${PostImMeas}, fine, ${PostPosID}"));

                        progress?.Report(new ProgressUpdate { Max = fileStreamRead.Length, Value = fileStreamRead.Position });
                    }

                    fileStreamWrite.SetLength(fileStreamWrite.Position);

                    progress?.Report(new ProgressUpdate { Max = fileStreamRead.Length, Value = fileStreamRead.Length });
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}