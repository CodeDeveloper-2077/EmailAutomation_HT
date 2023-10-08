using System.Text;

namespace EmailAutomation
{
    public class MessageGenerationService
    {
        private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public string GenerateRandomMessage(int minMessageLength, int maxMessageLength)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            int messageLength = random.Next(minMessageLength, maxMessageLength);

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
