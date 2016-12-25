using System;

namespace ConsoleApplication
{
    public struct Message
    {
        public int Value;
    }

    public class Consumer
    {
        public event EventHandler<Message> OnMessage;

        public Message? Consume(TimeSpan? timeout)
        {
            return new Message { Value = 42 };
        }

        public void Poll(TimeSpan? timeout)
        {
            var msgMaybe = Consume(timeout);
            if (msgMaybe.HasValue)
            {
                OnMessage?.Invoke(this, new Message { Value = 42 } );
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            int N = 50000000;
            var consumer = new Consumer();

            int cntr = 0;

            DateTime start = DateTime.Now;
            for (int i=0; i<N; ++i)
            {
                var msgMaybe = consumer.Consume(TimeSpan.FromMilliseconds(100));
                if (msgMaybe.HasValue)
                {
                    // do something
                    cntr += 1;
                }
            }
            Console.WriteLine("Duration: " + (DateTime.Now - start).TotalMilliseconds);
            // 1734.223

            consumer.OnMessage += (_, msg) =>
            {
                // do something.
                cntr += 1;
            };
            start = DateTime.Now;
            for (int i=0; i<N; ++i)
            {
                consumer.Poll(TimeSpan.FromMilliseconds(100));
            }
            Console.WriteLine("Duration: " + (DateTime.Now - start).TotalMilliseconds);
            // 2735.788
        }
    }
}
