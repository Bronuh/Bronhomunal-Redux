using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bronuh.Modules
{
	class AbroServer
	{
		public static string Request(string ip, int port, string request)
		{
			Console.WriteLine("Starting connection");
			TcpClient tcpClient = new TcpClient();
			tcpClient.Connect(ip, port);
			Console.WriteLine("Connected");

			NetworkStream stream = tcpClient.GetStream();
			Console.WriteLine("Sending: " + request);
			string encodedRequest = Encode(request);
			byte[] requestData = System.Text.Encoding.UTF8.GetBytes(encodedRequest);
			stream.Write(requestData, 0, requestData.Length);
			stream.Flush();

			byte[] respondData = new byte[1024 * 1024];
			StringBuilder response = new StringBuilder();
			do
			{
				int bytes = stream.Read(respondData, 0, respondData.Length);
				response.Append(Encoding.UTF8.GetString(respondData, 0, bytes));
			}
			while (stream.DataAvailable);
			Console.WriteLine("Raw response: " + response);
			return Decode(response.ToString());
		}

		private static string Encode(string text)
		{
			return text.Replace("\n", "<nl>") + "\n";
		}

		private static string Decode(string text)
		{
			return text.Replace("<nl>", "\n");
		}
	}
}
