using System;
using System.Linq;
using System.IO;
using NGameAsset;
using MiniJSON;
using LitJson;
using System.Globalization;
namespace KrTableDataDecryptor
{
    class Program
    {   
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string path= args.FirstOrDefault() ?? ".";

            foreach (string file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == ".bvo" || Path.GetExtension(file) == ".bjs")
                {
                    Console.WriteLine(file);
                    string json = null;
                    try { 
                        json = Json.Serialize(PlainAssetHelper.LoadJsonObjectFromJsonBin(File.ReadAllBytes(file), file, Path.GetExtension(file) == ".bvo"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error while decrypting");
                    }
                    if (json == "null")
                    {
                        Console.WriteLine("using LitJson");
                        JsonData data = PlainAssetHelper.LoadLitJsonFromJsonBin(File.ReadAllBytes(file), file, Path.GetExtension(file) == ".bvo");
                        json = LitJson_Extension_Method.ToJson(data, false);
                    }
                    File.WriteAllText(file + ".json", json);
                }
                if (Path.GetExtension(file) == ".vs")
                {
                    Console.WriteLine(file);
                    string text = PlainAssetHelper.DecryptText(file, File.ReadAllText(file));

                    File.WriteAllText(file + ".csv", text);
                }
            }
            Console.WriteLine("done");
        }
    }
}
