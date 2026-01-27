using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1.Data
{
	/// <summary>
	/// Cliente WebSocket para conectar con el servidor Django
	/// Maneja la sincronización en tiempo real de datos
	/// </summary>
	public class WebSocketClient
	{
		private static WebSocketClient _instance;
		private ClientWebSocket _webSocket;
		private CancellationTokenSource _cancellationTokenSource;
		private Task _receiveTask;
		private string _serverUrl;
		private bool _isConnected;
		private int _reconnectAttempts = 0;
		private const int MaxReconnectAttempts = 5;

		public event EventHandler<bool> ConnectionStatusChanged;
		public event EventHandler<string> MessageReceived;
		public event EventHandler<string> ErrorOccurred;

		private WebSocketClient()
		{
			_isConnected = false;
		}

		private string ExtractString(JToken token, params string[] keys)
		{
			if (token == null || token.Type == JTokenType.Null)
				return string.Empty;
			if (token.Type == JTokenType.Object)
			{
				foreach (var k in keys)
				{
					var v = token[k];
					if (v != null && v.Type != JTokenType.Null)
					{
						var s = v.Value<string>();
						if (!string.IsNullOrEmpty(s)) return s;
					}
				}
				// fallback to raw object string
				return token.ToString();
			}
			// If it's a value type, just return its string representation
			return token.Value<string>() ?? token.ToString();
		}

		private bool ExtractBool(JToken token, string key)
		{
			if (token == null || token.Type == JTokenType.Null) return false;
			var t = token[key];
			if (t == null || t.Type == JTokenType.Null) return false;
			try { return t.Value<bool>(); } catch { return false; }
		}

		public static WebSocketClient Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WebSocketClient();
				}
				return _instance;
			}
		}

		public bool IsConnected => _isConnected;

		public async Task<bool> ConnectAsync(string serverUrl)
		{
			try
			{
				_serverUrl = serverUrl;
				_webSocket = new ClientWebSocket();
				_cancellationTokenSource = new CancellationTokenSource();

				await _webSocket.ConnectAsync(new Uri(_serverUrl), _cancellationTokenSource.Token);

				_isConnected = true;
				_reconnectAttempts = 0;
				ConnectionStatusChanged?.Invoke(this, true);

				_receiveTask = Task.Run(() => ReceiveLoop());

				return true;
			}
			catch (Exception ex)
			{
				_isConnected = false;
				ConnectionStatusChanged?.Invoke(this, false);
				ErrorOccurred?.Invoke(this, $"Error al conectar: {ex.Message}");
				return false;
			}
		}

		private async Task ReceiveLoop()
		{
			byte[] buffer = new byte[1024 * 4];

			try
			{
				while (_webSocket.State == WebSocketState.Open && !_cancellationTokenSource.Token.IsCancellationRequested)
				{
					StringBuilder messageBuilder = new StringBuilder();
					WebSocketReceiveResult result;

					do
					{
						result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

						if (result.MessageType == WebSocketMessageType.Close)
						{
							await HandleDisconnection();
							return;
						}

						string chunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
						messageBuilder.Append(chunk);

					} while (!result.EndOfMessage);

					string message = messageBuilder.ToString();
					await ProcessMessage(message);
				}
			}
			catch (OperationCanceledException)
			{
				// Normal durante cierre
			}
			catch (Exception ex)
			{
				ErrorOccurred?.Invoke(this, $"Error en recepción: {ex.Message}");
				await HandleDisconnection();
			}
		}

		private async Task ProcessMessage(string message)
		{
			try
			{
				JObject json = JObject.Parse(message);
				string tipo = json["tipo"]?.ToString();

				switch (tipo)
				{
					case "initial_data":
						// Validar que el campo "data" exista y no sea nulo
						if (json["data"] != null && json["data"].Type != JTokenType.Null)
						{
							// await SyncInitialData(json["data"]);
						}
						else
						{
							ErrorOccurred?.Invoke(this, "Error: No se recibió información de datos iniciales del servidor.");
						}
						break;

					case "catalogo_actualizado":
						MessageReceived?.Invoke(this, "Catálogo actualizado");
						break;

					case "data_update":
						// await HandleDataUpdate(json);
						break;

					default:
						ErrorOccurred?.Invoke(this, $"Tipo de mensaje desconocido: {tipo}");
						break;
				}
			}
			catch (Exception ex)
			{
				ErrorOccurred?.Invoke(this, $"Error procesando mensaje: {ex.Message}");
			}
		}

		private async Task HandleDisconnection()
		{
			_isConnected = false;
			ConnectionStatusChanged?.Invoke(this, false);

			if (_reconnectAttempts < MaxReconnectAttempts)
			{
				_reconnectAttempts++;
				await Task.Delay(5000 * _reconnectAttempts);
				await ConnectAsync(_serverUrl);
			}
			else
			{
				ErrorOccurred?.Invoke(this, "Máximo de intentos de reconexión alcanzado");
			}
		}

		public async Task DisconnectAsync()
		{
			try
			{
				if (_webSocket != null && _webSocket.State == WebSocketState.Open)
				{
					_cancellationTokenSource?.Cancel();
					await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Desconexión manual", CancellationToken.None);
				}

				_isConnected = false;
				ConnectionStatusChanged?.Invoke(this, false);
			}
			catch (Exception ex)
			{
				ErrorOccurred?.Invoke(this, $"Error al desconectar: {ex.Message}");
			}
		}

		public async Task SendMessageAsync(string message)
		{
			if (_webSocket != null && _webSocket.State == WebSocketState.Open)
			{
				byte[] buffer = Encoding.UTF8.GetBytes(message);
				await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
			}
		}
	}
}
