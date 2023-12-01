// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

Console.WriteLine("Hello, World!");

	// Client
	var bytes = new byte[1024];
	IPAddress ip = new IPAddress(new byte[] { 172, 25, 219, 197 });
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



