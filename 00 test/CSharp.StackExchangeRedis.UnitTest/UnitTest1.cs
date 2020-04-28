using System;
using System.Threading.Tasks;
using NUnit.Framework;
using StackExchange.Redis;

namespace CSharp.StackExchangeRedis.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var conn = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions
            {
                //ServiceName = "127.0.0.1",
                EndPoints = {"127.0.0.1:6379"},
                //Password = 
            });

            var db = conn.GetDatabase(0);
            await db.StringSetAsync("First", "1");
            var val1 = db.StringGetAsync("First");
            db.Wait(val1);

            var channelName = "first-chanel";
            var channel = conn.GetSubscriber().Subscribe(channelName);
            channel.OnMessage(message =>
            {
                Console.WriteLine(message.Message);
            });


            conn.GetSubscriber().Publish(channelName, "hellow");
        }
    }
}