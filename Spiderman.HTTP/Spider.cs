using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Spiderman.HTTP.Enums;
using System.Net;

namespace Spiderman.HTTP
{
    public class Spider
    {
        public async Task<string> Request(HttpOptionBuilder option)
        {
            switch (option.RequestMode)
            {
                case RequestMode.API:
                    return await APIRequest(option);
                case RequestMode.Selenium:
                    return await SeleniumRequest(option);
            }
            return null;
        }
        public async Task<string> APIRequest(HttpOptionBuilder option)
        {
            var httpClientFactory = new HttpClientFactory();
            using var httpClient = httpClientFactory.Create(option);
            var response = await httpClient.SendAsync(option.HttpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        public async Task<string> SeleniumRequest(HttpOptionBuilder option)
        {
            // 设置ChromeDriver可执行文件的路径
            ChromeOptions options = new ChromeOptions();
            //options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            ChromeDriverService service = ChromeDriverService.CreateDefaultService("Drivers","chromedriver112.exe");
            //service.SuppressInitialDiagnosticInformation = true;
            //service.HideCommandPromptWindow = true;
            //servic

            // 创建Chrome浏览器实例
            IWebDriver driver = new ChromeDriver(service);
            driver.Navigate().GoToUrl(option.Url);
            string html = driver.PageSource;
            return html;
        }
    }
}