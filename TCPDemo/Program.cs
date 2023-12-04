// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;

// server
Task.Run(() => 
{ 
	TcpListener listener = null;
	try
	{
		int port = 5000;
		IPAddress localAddress = new IPAddress(new byte[] {127,0,0,1 });
		listener = new TcpListener(localAddress, port);
		listener.Start();
		var buffer = new byte[1024];
		string data = string.Empty;
		while (true)
		{
			Console.WriteLine("Чекаємо на зєднання...");
		    TcpClient client = listener.AcceptTcpClient();
			Console.WriteLine("Client connected ...");
			data = null;
			NetworkStream stream = client.GetStream();
			int i;
			while ((i=stream.Read(buffer,0,buffer.Length)) != 0)
			{
				data = System.Text.Encoding.UTF8.GetString(buffer,0,i);
				Console.WriteLine("Server Resived: {0}", data);
				data = data.ToUpper();
				var msg = System.Text.Encoding.UTF8.GetBytes(data);
				stream.Write(msg,0,msg.Length);
				Console.WriteLine("Server Sent: {0}", data);

			}
			client.Close();
		}
	}
	catch { }
	finally { listener?.Stop();
	}

});

Task.Run(() => 
{
	try
	{
		var client = new TcpClient("127.0.0.1", 5000);
		string message = "test message";
		var data = System.Text.Encoding.UTF8.GetBytes(message);
		var stream = client.GetStream();
		stream.Write(data, 0, data.Length);
		Console.WriteLine("Client Send: {0}", message);
		data = new byte[1024];
		var responceData = string.Empty;
		int bytes = stream.Read(data, 0, data.Length);
		responceData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
		Console.WriteLine("Client Received: {0}", responceData);
		stream.Close();
		client.Close();
	}
	catch { }
});

Console.ReadLine();