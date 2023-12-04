// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

UdpClient udpClient = new UdpClient();
udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 5000));
Task.Run(() => {
	var from = new IPEndPoint(0, 5000);
	while (true)
	{
		Task.Delay(1000).Wait();
		var recBuffer = udpClient.Receive(ref from);
		Console.WriteLine($"Recesive: {Encoding.UTF8.GetString(recBuffer,0,recBuffer.Length)}");

	}
});


for (int i = 0; i < 100000; i++)
{
	var data = Encoding.UTF8.GetBytes($"Hello World {i}");
	udpClient.Send(data, data.Length, "255.255.255.255", 5000);
}
Console.WriteLine("All packet sending");
Console.ReadLine();

