namespace Angel.Webservice.Models
{
    using System.Linq;
    using System.Reflection;

    public class BearerTokenLoginModel : IEquatable<BearerTokenLoginModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public BearerTokenLoginModel() { }

        public BearerTokenLoginModel(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return other is BearerTokenLoginModel t && Equals(t);
        }

        public bool Equals(BearerTokenLoginModel other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            PropertyInfo[] publicProperties = GetType().GetProperties();

            if (publicProperties.Any())
                return publicProperties
                    .All(item => item.GetValue(this, null)
                        .Equals(item.GetValue(other, null)));

            return true;
        }

        public static bool operator ==(BearerTokenLoginModel left, BearerTokenLoginModel right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (ReferenceEquals(null, left) || ReferenceEquals(null, right))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(BearerTokenLoginModel left, BearerTokenLoginModel right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            const int index = 1;

            PropertyInfo[] publicProperties = GetType().GetProperties();

            if (publicProperties.Any())
            {
                foreach (var value in publicProperties.Select(item => item.GetValue(this, null)))
                {
                    if (value != null)
                    {
                        hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();
                        changeMultiplier = !changeMultiplier;
                    }
                    else
                        hashCode = hashCode ^ (index * 13);
                }
            }

            return hashCode;
        }
    }
}
