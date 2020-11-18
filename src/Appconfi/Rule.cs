using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Appconfi
{
    public abstract class Rule
    {
        public abstract bool IsTarget(User user);
    }
    public class RulePercentage : Rule
    {
        public int Percent { get; set; }

        public override bool IsTarget(User user)
        {
            if (Percent == 0)
                return false;
            if (Percent == 100)
                return true;

            var hashId = CreateMD5Hash(user["id"]);
            int value = int.Parse(hashId[0].ToString(), System.Globalization.NumberStyles.HexNumber);
            var number = (Percent / 100.0) * 16;
            return value <= number;
        }

        public string CreateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class ByPropertyRule : Rule
    {
        public string Values { get; set; }
        public string Option { get; set; }
        public string Property { get; set; }
        public override bool IsTarget(User user)
        {
            if (user == null || string.IsNullOrEmpty(Property) || string.IsNullOrEmpty(Values))
                return false;

            if (!user.ContainsKey(Property))
                return false;

            var value = user[Property];
            return ApplyRule(value);
        }

        private bool ApplyRule(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            switch (Option)
            {
                case "isin":
                    return Values.Split(",").Any(x => x.Contains(value));
                case "isnotin":
                    return Values.Split(",").Any(x => !x.Contains(value));
                case "contains":
                    return value.Contains(Values);
                case "notcontains":
                    return !value.Contains(Values);
                default:
                    return false;
            }
        }
    }
}