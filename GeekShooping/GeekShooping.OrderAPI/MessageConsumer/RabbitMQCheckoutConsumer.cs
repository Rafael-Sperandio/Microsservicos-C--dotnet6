using GeekShooping.OrderAPI.Model.Base;
using GeekShopping.CartAPI.Repository;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    /// <summary>
    /// Serviço em segundo plano responsável por consumir mensagens da fila RabbitMQ
    /// e criar registros de pedidos (<see cref="OrderHeader"/> e <see cref="OrderDetail"/>).
    /// </summary>
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;


        public RabbitMQCheckoutConsumer(OrderRepository repository)
        {
            _repository = repository;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);  
        }


        /// <summary>
        /// Inicia o processo de escuta da fila RabbitMQ e define o comportamento ao receber mensagens.
        /// </summary>
        /// <param name="stoppingToken">Token que sinaliza a interrupção do serviço.</param>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                // Desserializa o DTO recebido da fila.
                var headerDto = JsonSerializer.Deserialize<CheckoutHeaderDto>(content);

                // Processa o pedido de forma síncrona dentro do evento.
                ProcessOrder(headerDto).GetAwaiter().GetResult();

                // Confirma o processamento da mensagem. 
                // Removendo a mensagem da fila do RabitMQ
                _channel.BasicAck(evt.DeliveryTag, multiple: false);
            };

            // Inicia o consumo da fila.
            _channel.BasicConsume(
                queue: "checkoutqueue",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        /// <summary>
        /// Converte os dados recebidos do RabbitMQ em entidades de domínio
        /// (<see cref="OrderHeader"/> e <see cref="OrderDetail"/>)
        /// e os envia para persistência.
        /// </summary>
        /// <param name="dto">Dados provenientes do checkout do carrinho.</param>
        private async Task ProcessOrder(CheckoutHeaderDto dto)
        {
            OrderHeader order = new()
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = dto.CardNumber,
                CouponCode = dto.CouponCode,
                CVV = dto.CVV,
                DiscountAmount = dto.DiscountAmount,
                Email = dto.Email,
                ExpiryMonthYear = dto.ExpiryMothYear,
                OrderTime = DateTime.Now, // momento de criação da 
                PurchaseAmount = dto.PurchaseAmount,
                PaymentStatus = false, //padrão falso pois deve ser true apenas quando confirmado o pagamento
                Phone = dto.Phone,
                DateTime = dto.DateTime
            };


            
            // Monta os itens do pedido.
            //TODO ver se funciona
            order.AddOrderItems(dto.CartDetails);
            /*
                        foreach (var details in dto.CartDetails)
                        {
                            var detail = new OrderDetail
                            {
                                ProductId = details.ProductId,
                                ProductName = details.Product.Name,
                                Price = details.Product.Price,
                                Count = details.Count
                            };

                            order.CartTotalItens += details.Count;
                            order.OrderDetails.Add(detail);
                        }
            */
            // Salva o pedido no banco.
            await _repository.AddOrder(order);
        }
    }
}
