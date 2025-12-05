using GeekShopping.CartAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;


namespace GeekShopping.CartAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _conection;

        public RabbitMQMessageSender()
        {
            //my-rabbit
            //_hostname = "my-rabbit";
            _hostname = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName=_hostname,
                UserName =_username,
                Password=_password, 


            };
            _conection = factory.CreateConnection();//cria conexão
            using var channel = _conection.CreateModel();//cria canal de acesso ao rabbitMQ
            channel.QueueDeclare(queue:queueName,false,false,false,arguments: null);//declaração de fila
;           byte[] body = GetMessageAsByteArray(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null,body:body);//faz publicação da mensagem para rabbitMQ

        }
        /// <summary>
        /// converte mensagem em array de bytes
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        byte[]  GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<CheckoutHeaderDto>((CheckoutHeaderDto)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
