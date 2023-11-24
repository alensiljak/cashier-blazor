using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cashier.Lib
{
    /// <summary>
    /// Prints the given object into the Console.
    /// </summary>
    public class DebugPrinter
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public static void Print(object obj)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object? value = descriptor.GetValue(obj);
                Console.WriteLine("{0} = {1}", name, value);
            }
        }

        public static void PrintJson(object? obj)
        {
            var output = JsonConvert.SerializeObject(obj);

            Console.WriteLine(output);
        }
    }
}
