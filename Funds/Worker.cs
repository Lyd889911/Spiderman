using Spiderman.HTTP;
using Newtonsoft.Json;
using Funds.Dtos;
using HtmlAgilityPack;

namespace Funds
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await AllFunds();
            //await LSJZ("000001");
        }
        //获取全部基金代码
        private async Task AllFunds()
        {
            HttpOptionBuilder option = new HttpOptionBuilder("https://fund.eastmoney.com/js/fundcode_search.js");
            Spider spider = new Spider();
            string r = await spider.Request(option);

            r = r.Replace("var r = ", "").Replace(";", "");

            var list = JsonConvert.DeserializeObject<List<List<string>>>(r);
            int i = 1;
            foreach (var item in list)
            {
                //基金代码，基金名字，基金类型
                Console.WriteLine($"{i++}————{item[0]}————{item[2]}————{item[3]}");

                if (i > 20)
                    break;

                if (item[3].Contains("货币"))
                    continue;
                await LSJZ(item[0]);
            }
        }

        //获取某一只基金的历史净值数据
        private async Task LSJZ(string code)
        {
            int total = 0;
            int index = 1;
            object loc = new object();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            while (true)
            {
                if (total!=0&&index > total)
                    break;
                if (tokenSource.IsCancellationRequested)
                    break;

                if(index == 1)
                {
                    int pages = await LsjzTask(code, index, tokenSource);
                    total = pages;
                    Console.WriteLine($"总页数：{total}");
                    if (total == 0)
                        break;

                    Parallel.For(2, total, async (i) =>
                    {
                        await LsjzTask(code, i, tokenSource);
                    });
                }
                else
                {
                    //Task.Run(async () => {
                    //    int pages = await LsjzTask(code, index, tokenSource);
                    //}, tokenSource.Token);
                }
                index++;
            }
        }

        private async Task<int> LsjzTask(string code,int index, CancellationTokenSource tokenSource)
        {
            string url = $"https://fundf10.eastmoney.com/F10DataApi.aspx?type=lsjz&code={code}&page={index}&per=49";
            HttpOptionBuilder option = new HttpOptionBuilder(url);
            //Spider spider = new Spider();
            //string r = await spider.Request(option);

            var httpclient = new HttpClient(option.HttpClientHandler);
            var response = httpclient.Send(new HttpRequestMessage(HttpMethod.Get,url));
            var r = await response.Content.ReadAsStringAsync();
            await Task.Delay(200);
            if (string.IsNullOrEmpty(r))
            {
                Console.WriteLine(url);
                Console.WriteLine("空了");
                return 0;
            }
            r = r.Replace("var apidata=", "").Replace(";", "");
            var lsjz = JsonConvert.DeserializeObject<LsjzDto>(r);
            //Console.WriteLine(r);

            var doc = new HtmlDocument();
            doc.LoadHtml(r);

            var rows = doc.DocumentNode.Descendants("tr").ToList();
            for (int i = 0; i < rows.Count; i++)
            {
                var cols = rows[i].Descendants("td").ToList();
                if (cols.Count == 0)
                    continue;
                if (cols[0].InnerText.Contains("暂无"))
                    break;

                JzinfoDto jz = new JzinfoDto();
                jz.Date = DateTime.Parse(cols[0].InnerText);

                string dwjz = string.IsNullOrEmpty(cols[1].InnerText) ? "0" : cols[1].InnerText;
                jz.Dwjz = Convert.ToDecimal(dwjz);

                string ljjz = string.IsNullOrEmpty(cols[2].InnerText) ? "0" : cols[2].InnerText;
                jz.Ljjz = Convert.ToDecimal(ljjz);

                string rate = string.IsNullOrEmpty(cols[3].InnerText) ? "0" : cols[3].InnerText.Replace("%", "");
                jz.RiseRate = Convert.ToDecimal(rate);

                //Console.WriteLine(JsonConvert.SerializeObject(jz));
            }

            Console.WriteLine($"{code}第{index}页");
            if (lsjz.Curpage >= lsjz.Pages)
                tokenSource.Cancel();

            return lsjz.Pages;
        }
    }
}