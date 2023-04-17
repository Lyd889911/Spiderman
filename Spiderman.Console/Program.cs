// See https://aka.ms/new-console-template for more information
using Spiderman.HTTP;

Console.WriteLine("Hello, World!");
HttpOptionBuilder option = new HttpOptionBuilder("https://danjuanfunds.com/djapi/fundx/base/fund/record/asset/percent?fund_code=001490");
Spider spider = new Spider();
string r =await spider.Get(option);
Console.WriteLine(r);