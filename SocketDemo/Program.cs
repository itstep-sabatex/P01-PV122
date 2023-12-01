// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
Task.Run(() =>
{
	// Server
	var ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
	IPEndPoint localEndPoint = new IPEndPoint(ip, 5000);

	Socket listner = new Socket(localEndPoint.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
	try
	{
		listner.Bind(localEndPoint);
		listner.Listen(1000);
		var bytes = new byte[1024];
		while(true)
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
	}catch(Exception ex)
	{
		Console.WriteLine(ex.ToString());
	}

});

Task.Run(() => 
{
	// Client
	var bytes = new byte[1024];
	IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
	var remoteEndPoint = new IPEndPoint(ip, 5000);
	Socket listner = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
	try
	{
		listner.Connect(remoteEndPoint);
		Console.WriteLine($"Client connected to {listner.RemoteEndPoint.ToString()} ");
		var msg = Encoding.ASCII.GetBytes("Test message from Client ... \r\n");
		listner.Send(msg);
		listner.Shutdown(SocketShutdown.Both);
		listner.Close();
	}
	catch (Exception ex)
	{
		
		Console.WriteLine(ex.ToString());

	}


});


Console.ReadLine();