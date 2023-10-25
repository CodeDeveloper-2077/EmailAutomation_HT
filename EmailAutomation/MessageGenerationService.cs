using System.Text;

namespace EmailAutomation
{
    public class MessageGenerationService
    {
        private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private readonly int _minLength;
        private readonly int _maxLength;

        public MessageGenerationService(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public string GenerateRandomMessage()
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            int messageLength = random.Next(_minLength, _maxLength);

            for (int i = 0; i <= messageLength; i++)
            {
                int index = random.Next(0, _chars.Length);

                if (i % 3 == 0 && char.IsLetter(_chars[index]))
                {
                    sb.Append(_chars[index].ToString().ToLowerInvariant());
                }
                else
                {
                    sb.Append(_chars[index]);
                }
            }

            return sb.ToString();
        }
    }
}
