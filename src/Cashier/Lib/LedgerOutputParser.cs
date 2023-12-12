using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cashier.Lib
{
    public class LedgerOutputParser
    {
        public LedgerOutputParser() { }

        /// <summary>
        /// Returns the array of lines containing totals from the ledger output.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<string> getTotalLines(List<string> lines)
        {
            // Extract the total lines from the output,
            // unless there is only one account, in which case use the complete output

            var result = new List<string>();
            string totalLine = string.Empty;
            var nextLineIsTotal = false;

            if (lines.Count == 1)
            {
                // No income is an array with an empty string ['']
                if (lines[0] == string.Empty)
                {
                    totalLine = "0";
                } else
                {
                    // One-line results don't have totals
                    totalLine = lines[0];
                }

                result.Add(totalLine);
            } else
            {
                foreach (var line in lines)
                {
                    if(nextLineIsTotal)
                    {
                        result.Add(line);
                    } else
                    {
                        if(line.IndexOf("------") >= 0)
                        {
                            nextLineIsTotal = true;
                        }
                    }
                }
            }

            if(string.IsNullOrEmpty( totalLine))
            {
                throw new Exception("No total received!");
            }

            return result;
        }
    }
}
