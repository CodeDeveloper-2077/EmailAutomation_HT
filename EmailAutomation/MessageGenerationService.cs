using System.Text;

namespace EmailAutomation
{
    public class MessageGenerationService
    {
        private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public string GenerateRandomMessage(int minMessageLength, int maxMessageLength)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            int messageLength = random.Next(minMessageLength, maxMessageLength);

            for (int i = 0; i <= messageLength; i++)
            {
                int index = random.Next(0, chars.Length);

                if (i % 3 == 0 && char.IsLetter(chars[index]))
                {
                    sb.Append(chars[index].ToString().ToLowerInvariant());
                }
                else
                {
                    sb.Append(chars[index]);
                }
            }

            return sb.ToString();
        }
    }
}
