using System.Net.Sockets;
using System.Net;
using System.Text;
namespace LocalNetworkMessager
{
	internal class Controller
	{
		TcpListener server = new TcpListener(IPAddress.Any, 6666);
		TcpClient client = new TcpClient(AddressFamily.InterNetwork);
		public event Action<string> MessageReceived = (X) => { };
		bool isListening = false;
		Task Receiver = Task.Run(() => { });
		public void ConnectTo(string way)     //public event Action<string> Connect;
		{
			if (isListening)
			{
				MessageReceived?.Invoke("Disconnect or close server firstly!");
			}
			isListening = true;
			client.Connect(way, 6666);
			Receiver = Task.Run(ReceiveAsClient);
		}
		public void Disconnect()     //public event Action Disconnect;
		{
			isListening = false;
			Receiver.Wait();
			client.Dispose();
		}
		public void CreateServer()     //public event Func<string> CreateServer;
		{
			if (isListening)
			{
				MessageReceived?.Invoke("Disconnect or close server firstly!");
			}
			isListening = true;
			server.Start();
			MessageReceived?.Invoke("Server Started");
			Receiver = Task.Run(ReceiveAsServer);
		}
		public void CloseServer()     //public event Action CloseServer;
		{
			isListening = false;
			Receiver.Wait();
			server.Stop();
		}
		void ReceiveAsServer()
		{
			List<TcpClient> clients = new List<TcpClient>();
			List<NetworkStream> clientsInWork = new List<NetworkStream>();
			List<Task> tasks = new List<Task>();
			TcpClient client;
			NetworkStream stream;
			while (isListening)
			{
				if (server.Pending())
				{
					client = server.AcceptTcpClient();
					clients.Add(client);
					stream = client.GetStream();
					clientsInWork.Add(client.GetStream());
					tasks.Add(Task.Run(() => SubReceive(stream, (str) => SendToAll(clientsInWork, str))));
				}
			}
			foreach (var t in tasks)
			{
				t.Wait();
			}
		}
		void SendToAll(List<NetworkStream> streams, string message)
		{
			message += '\n';
			Parallel.For(0, streams.Count, (X) =>
			{
				SubSend(streams[X], Encoding.UTF8.GetBytes(message));
			});
		}
		void SubSend(NetworkStream ns, byte[] info)
		{
			ns.Write(info);
		}
		void SubReceive(NetworkStream ns, Action<string> WriteTo)
		{
			StringBuilder sb = new StringBuilder();
			while (isListening)
			{
				int info = ns.ReadByte();
				if (info == -1)
				{
					continue;
				}
				if (info == '\n')
				{
					WriteTo?.Invoke(sb.ToString());
					sb.Clear();
					continue;
				}
				sb.Append((char)info);
			}
		}
		public void Send(string message)     //public event Action<string> SendMessage;
		{
			message = Environment.UserDomainName + message + '\n';
			SubSend(client.GetStream(), Encoding.UTF8.GetBytes(message));
		}
		void ReceiveAsClient()
		{
			SubReceive(client.GetStream(), MessageReceived);
		}
	}
}