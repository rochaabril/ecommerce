using MimeKit;
using MailKit.Net.Smtp;


namespace EcommerceApi.Services
{
	public class EmailService
	{
		private readonly string _smtpServer;
		private readonly int _port;
		private readonly string _email;
		private readonly string _password;

		public EmailService(IConfiguration configuration)
		{
			_smtpServer = configuration["Email:SmtpServer"];
			_port = int.Parse(configuration["Email:Port"]);
			_email = configuration["Email:Address"];
			_password = configuration["Email:Password"];
		}

		public async Task SendResetPasswordEmailAsync(string toEmail, string resetLink)
		{
			//var message = new MimeMessage();
			//message.From.Add(new MailboxAddress("Tu Nombre", _email));
			//message.To.Add(new MailboxAddress("", toEmail));
			//message.Subject = "Restablecimiento de Contraseña";

			//message.Body = new TextPart("plain")
			//{
			//	Text = $"Hola,\n\nPara restablecer tu contraseña, haz clic en el siguiente enlace:\n{resetLink}\n\nGracias."
			//};

			//using (var client = new SmtpClient())
			//{
			//	await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
			//	await client.AuthenticateAsync(_email, _password);
			//	await client.SendAsync(message);
			//	await client.DisconnectAsync(true);
			//}
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("Accesorios Magrini", _email));
				message.To.Add(new MailboxAddress("", toEmail));
				message.Subject = "Restablecimiento de Contraseña";

				message.Body = new TextPart("plain")
				{
					Text = $"Hola,\n\nPara restablecer tu contraseña, haz clic en el siguiente enlace:\n{resetLink}\n\nGracias."
				};

				using (var client = new SmtpClient())
				{
					await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
					await client.AuthenticateAsync(_email, _password);
					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				// Registrar el error o lanzar una excepción personalizada
				Console.WriteLine($"Error al enviar correo: {ex.Message}");
				throw; // O maneja el error según necesites
			}
		}
	}
}
