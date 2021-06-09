using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bronuh.Modules
{
	public class RSS
	{
		public static string GetRandomQuote()
		{
			var client = new WebClient();
			var shit = client.DownloadString("https://bash.im/rss/");

			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(shit);
			XmlElement xRoot = xDoc.DocumentElement;
			var channel = xRoot.ChildNodes[0];
			var items = new List<XmlElement>();

			foreach (XmlElement item in channel.ChildNodes)
			{
				if (item.Name == "item")
				{
					items.Add(item);
				}
			}
			client.Dispose();
			return GetContent(items[new Random().Next(0, items.Count)]).Replace("&quot","\"");
		}

		public static string GetRandomJoke()
		{
			var client = new WebClient();
			var shit = client.DownloadString("https://www.anekdot.ru/rss/export_top.xml");

			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(shit);
			XmlElement xRoot = xDoc.DocumentElement;
			var channel = xRoot.ChildNodes[0];
			var items = new List<XmlElement>();

			foreach (XmlElement item in channel.ChildNodes)
			{
				if (item.Name == "item")
				{
					items.Add(item);
				}
			}
			client.Dispose();
			return GetContent(items[new Random().Next(0, items.Count)],3).Replace("&quot", "\"");
		}

		public static string[] GetRandomAnecdote()
		{
			var client = new WebClient();
			var rnd = new Random();
			int anecId = rnd.Next(1,1143);
			int imgId = rnd.Next(1,32);
			var shit = client.DownloadString("https://baneks.ru/"+anecId);
			client.Dispose();

			var context = BrowsingContext.New(Configuration.Default);
			var document = context.OpenAsync(req => req.Content(shit)).GetAwaiter().GetResult();

			var article = document.QuerySelector("article");
			var num = article.FirstChild.TextContent;
			var anek = article.TextContent;

			return new string[] {num+"\n"+anek , "https://baneks.ru/img/bgs/bg-"+imgId+".jpg" };
		}

		public static string GetRandomComicsLink()
		{
			var client = new WebClient();
			var shit = client.DownloadString("https://bash.im/rss/comics.xml");

			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(shit);
			XmlElement xRoot = xDoc.DocumentElement;
			var channel = xRoot.ChildNodes[0];
			var items = new List<XmlElement>();

			foreach (XmlElement item in channel.ChildNodes)
			{
				if (item.Name == "item")
				{
					items.Add(item);
				}
			}
			client.Dispose();

			return GetContent(items[new Random().Next(0, items.Count)]).Replace("<img src=\"", "").Replace("\">", "");
		}

		private static string GetContent(XmlElement item)
		{
			string text = "";

			try
			{
				var desc = item.ChildNodes.Item(4);
				text = desc.InnerText.Replace("<br>", "\r\n");

			}
			catch (Exception e)
			{

			}

			return text;
		}

		private static string GetContent(XmlElement item,int id)
		{
			string text = "";

			try
			{
				var desc = item.ChildNodes.Item(id);
				text = desc.InnerText.Replace("<br>", "\r\n");

			}
			catch (Exception e)
			{

			}

			return text;
		}
	}
}
