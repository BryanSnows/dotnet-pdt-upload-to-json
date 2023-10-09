using System;
using System.IO;
using System.Linq;
using ACadSharp;
using ACadSharp.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Lendo o arquivo DWG e convertendo todas as informações em JSON...");

        try
        {
            string dwgFilePath = "C:/Users/bryan.pinto/Desktop/dwg/dwg/2.dwg";

            if (File.Exists(dwgFilePath))
            {
                CadDocument doc = DwgReader.Read(dwgFilePath);

               
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };

                
                var allData = new
                {
                    Header = doc.Header,
                    Blocks = doc.BlockRecords.Select(blockRecord => new
                    {
                        Name = blockRecord.Name,
                       
                    }),
                    // Layers = doc.Layers,
                    
                };

                
                string json = JsonConvert.SerializeObject(allData, Formatting.Indented, settings);

              
                File.WriteAllText("saida.json", json);

                Console.WriteLine("Conversão concluída. O JSON foi salvo em 'saida.json'");
            }
            else
            {
                Console.WriteLine("O arquivo DWG não foi encontrado.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro: " + ex.Message);
        }
    }
}