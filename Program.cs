using System;
using System.Threading.Tasks;
using System.Linq;
using XCommas.Net.Objects;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ConsoleTableExt;
using System.Reflection;

namespace BlockPartySdca
{
    class Program
    {
        //static List<(int SO, decimal Tp)> tpTargets = new List<(int SO, decimal Tp)>
        //{
        //    (0, 0.42m),
        //    (1, 0.42m),
        //    (2, 0.42m),
        //    (3, 1.0m),
        //    (4, 2.0m),
        //    (5, 3.0m),
        //    (6, 4.0m),
        //    (7, 5.0m),
        //    (8, 6.0m)
        //};

        static async Task Main(string[] args)
        {
            Console.WriteLine("BlockParty SDCA tool " + typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, false);

            var config = builder.Build();
            var c = config.GetSection("tp");
            List<SOTP> tpTargets = new List<SOTP>();
            c.Bind(tpTargets);
            //var list = c.GetChildren().ToList();

            var tableData = new List<List<object>>();
            foreach (var item in tpTargets)
            {
                tableData.Add(new List<object> { item.SO, item.TP + "%" });
            }

            ConsoleTableBuilder
                .From(tableData)
                .WithMinLength(new Dictionary<int, int> {
                { 0, 15 },
                { 1, 15 }
                })
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center }
                })
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("TP targets", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("SO", "TP")
                .ExportAndWriteLine();



            if (args.Length < 3)
            {
                Console.WriteLine($"Please specify bot name and API key");
                return;
            }

            var botName = args[0];
            var apiKey = args[1];
            var secret = args[2];

            var client = new XCommas.Net.XCommasApi(apiKey, secret);

            Console.WriteLine($"Bot name {botName}");

            var response = await client.GetDealsAsync(limit: 1000, dealScope: DealScope.Active, dealOrder: DealOrder.CreatedAt);

            if (response.IsSuccess)
            {

                var deals = response.Data.Where(deal => deal.BotName == botName);
                Console.WriteLine($"{deals.Count()} deals.");
                foreach (var deal in deals)
                {

                    Console.WriteLine($"{deal.BotName} {deal.Pair} SO {deal.CompletedSafetyOrdersCount} TP {deal.TakeProfit}");
                    
                    var tpTarget = tpTargets.SingleOrDefault(m => m.SO == deal.CompletedSafetyOrdersCount);
                    if (tpTarget != default)
                    {
                        if (deal.TakeProfit != tpTarget.TP)
                        {
                            decimal takeProfit = tpTarget.TP;
                            DealUpdateData dealUpdateData = new DealUpdateData(deal.Id)
                            {
                                TakeProfit = takeProfit
                            };
                            Console.WriteLine($"Set take profit {takeProfit}%");
                            await client.UpdateDealAsync(deal.Id, dealUpdateData);
                        }
                    }


                }
            }
            else
            {
                Console.WriteLine(response.Error);
            }
        }
    }

    class SOTP
    {
        public int SO { get; set; }
        public decimal TP { get; set; }
    }
}
