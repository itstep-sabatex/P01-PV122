// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

Console.WriteLine("Hello, World!");

	// Server
	var ip = new IPAddress(new byte[] { 172, 25, 219, 197 });
	IPEndPoint localEndPoint = new IPEndPoint(ip, 5000);

	Socket listner = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
	try
	{
		listner.Bind(localEndPoint);
		listner.Listen(1000);
		var bytes = new byte[1024];
		while (true)
		{
			Console.WriteLine("Чекаємо на зєднання");
			Socket handle = listner.Accept();
			Console.WriteLine("Server: Підєднався клієнт");
			string data = "";
			while (true)
			{
				int bytesRec = handle.Receive(bytes);
				data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
				if (data.IndexOf("\r\n") > -1)
				{
					break;
				}
			}
			Console.WriteLine($"Server:Отримані дані: {data}");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.ToString());
	}


