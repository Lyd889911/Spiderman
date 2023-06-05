// See https://aka.ms/new-console-template for more information
using Spiderman.HTTP;
using Spiderman.HTTP.Enums;

Console.WriteLine("Hello, World!");
//https://danjuanfunds.com/djapi/fundx/base/fund/record/asset/percent?fund_code=001490
HttpOptionBuilder option = new HttpOptionBuilder("https://www.bilibili.com/");
Spider spider = new Spider();
string r =await spider.Request(option);
//string r =await spider.SeleniumRequest(option);
Console.WriteLine(r);