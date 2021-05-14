using System;
using System.Threading.Tasks;
using System.Linq;
using XCommas.Net.Objects;

namespace BlockPartySdca
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("BlockParty SDCA tool");

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
                    if (deal.CompletedSafetyOrdersCount >= 5 && deal.TakeProfit == 0.42m)
                    {
                        decimal takeProfit = 3;
                        DealUpdateData dealUpdateData = new DealUpdateData(deal.Id)
                        {
                            TakeProfit = takeProfit
                        };
                        Console.WriteLine($"Set take profit {takeProfit}%");
                        await client.UpdateDealAsync(deal.Id, dealUpdateData);
                    }

                }
            } else
            {
                Console.WriteLine(response.Error);
            }
        }
    }
}
