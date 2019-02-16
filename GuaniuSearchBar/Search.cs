using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public class Search
    {
        static int startIndex = 0;
        static string[] hotKeywords = new string[50];
        static Label[] labels;
        static string defaultExplorer;

        static string[] fix_sitenames = { "百度", "淘宝", "天猫", "hao123", "test", "百度", "淘宝", "天猫", "hao123", "test" };
        static string[] fix_urls = {

        };
        public static string[] sitenames = { "百度", "淘宝", "天猫", "hao123", "test", "百度", "淘宝", "天猫", "hao123", "test" };
        public static string[] urls;

        public static void GetDefaultExplorer()
        {
            //return "chrome.exe";
        }

        public static void GetIconLinks()
        {
            Action action = () =>
            {
                try
                {
                    while (true)
                    {
                        string s = HttpHelper.HttpGet(HttpHelper.baseUrl + "/static/links.json");
                        JToken jsonResults = (JToken)JsonConvert.DeserializeObject(s);

                        // var data = jsonResults.SelectToken("data");
                        var root = jsonResults.SelectToken("links");
                        sitenames = root.Select(ss =>
                        {
                            return ss["name"].ToString();
                        }
                        ).ToArray();
                        urls = root.Select(ss =>
                        {
                            return ss["url"].ToString();
                        }
                        ).ToArray();

                        //if (labels != null)
                        //{
                        //    GetBaiduHotKeywords(labels);
                        //}
                        Thread.Sleep(10000);//10s
                    }



                }
                catch (Exception e)
                {
                }
            };


            new Thread(new ThreadStart(action)).Start();
        }

        public static void GetBaiduHotKeywords()
        {
            hotKeywords = new string[50];
            for (int i = 0; i < hotKeywords.Length; i++)
            {
                hotKeywords[i] = string.Empty;
            }
            Action UpdateAction = () =>
            {
                try
                {
                    while (true)
                    {
                        string s = HttpHelper.HttpGet("https://api.shenjian.io/?appid=ba446d36cd13dc91a69baf3b2ca7e338");
                        JToken jsonResults = (JToken)JsonConvert.DeserializeObject(s);

                        // on error stop.
                        if (jsonResults.SelectToken("error_code").ToString() != "0")
                        {
                            return;
                        }
                        // var data = jsonResults.SelectToken("data");
                        hotKeywords = jsonResults.SelectToken("data")
                                                                    .Select(ss => { return ss["keyword"].ToString(); }).ToArray();
                        for (int i = 0; i < hotKeywords.Length; i++)
                        {
                            hotKeywords[i] = (i + 1).ToString() + "." + hotKeywords[i];
                        }

                        if (labels != null)
                        {
                            GetBaiduHotKeywords(labels);
                        }
                        Thread.Sleep(10000);//10s
                    }



                }
                catch (Exception e)
                {
                }
            };


            new Thread(new ThreadStart(UpdateAction)).Start();
        }

        public static void GetBaiduHotKeywords(Label[] linkLabels, bool changeKeywordFlag = false)
        {
            labels = linkLabels;
            Action UpdateNews = () =>
            {
                try
                {
                    int labelCnt = linkLabels.Count();
                    if (changeKeywordFlag)
                    {
                        startIndex += labelCnt;
                        if (startIndex + labelCnt > hotKeywords.Count())
                        {
                            startIndex = 0;
                        }
                        Debug.Print("startindex{0}", startIndex);
                    }



                    linkLabels[0].Invoke(new MethodInvoker(() =>
                    {
                        for (int i = 0; i < labelCnt; i++)
                        {
                            linkLabels[i].Text = " " + hotKeywords[i + startIndex].ToString();
                            linkLabels[i].Tag = "https://www.baidu.com/baidu?word=" + linkLabels[i].Text;
                        }
                    }));




                }
                catch (Exception e)
                {
                }
            };


            new Thread(new ThreadStart(UpdateNews)).Start();
        }
    }
}
