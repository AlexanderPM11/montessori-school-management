using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace RealtyApp.Core.Application.Helpers
{
    public static class SessionHelper
    {
        public static void SetString<T>(this ISession session, string key, T value)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            session.Set(key, Encoding.UTF8.GetBytes(serializedData));
        }

        public static T GetString<T>(this ISession session, string key)
        {
            if (session.TryGetValue(key, out byte[] value))
            {
                string stringValue = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject<T>(stringValue)!;
            }

            return default!;
        }
    }
}
