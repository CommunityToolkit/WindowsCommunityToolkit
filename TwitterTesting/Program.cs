using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

namespace TwitterTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var passwordManager = new MockPasswordManager();
            var storageManager = new MockStorageManager();
            var signatureManager = new MockSignatureManager();
            var broker = new Broker();

            var service = new Microsoft.Toolkit.Services.Twitter.TwitterService();

            var initialized = service.Initialize("8IrvyRNPtjOrq1s522mtmW8F1", "GcWaNKlbH5JEEXRvkyT07bHqC1tLT5DKVmjynplGHgz1aLXvzO", "http://www.google.com", broker, passwordManager, storageManager, signatureManager);
            if (!initialized)
            {
                Console.WriteLine("ERROR cannot initialize service");
            }

            var picture = File.OpenRead("C:\\Users\\GabrielBarzola\\Pictures\\armani.jpg");

            var posted =  service.TweetStatusAsync("Hello from NetStandard. Go Argentina Go", picture).Result;
            if (posted)
            {
                Console.WriteLine("Message posted OK");
            }else{
                Console.WriteLine("Message posted FAIL");
            }

            Console.WriteLine("Wainting");
            Console.ReadLine();

        }



    }

    public class Broker : Microsoft.Toolkit.Services.Core.IAuthenticationBroker
    {
        public Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri)
        {
            throw new NotImplementedException();
        }
    }

    public class MockSignatureManager : Microsoft.Toolkit.Services.Core.ISignatureManager
    {
        public string GetSignature(string baseString, string secret, bool append = false)
        {
            var key = append ? secret + "&" : secret;

            var mac = System.Security.Cryptography.HMACSHA1.Create("System.Security.Cryptography.HMACSHA1");            
            byte[] keyMaterial = System.Text.UTF8Encoding.UTF8.GetBytes(key);
            mac.Key = keyMaterial; //CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);

            byte[] dataToBeSigned = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
            var hash = mac.ComputeHash(dataToBeSigned); //IBuffer hash = CryptographicEngine.Sign(cryptoKey, dataToBeSigned);
            return Convert.ToBase64String(hash);
        }
    }

    public class MockStorageManager : Microsoft.Toolkit.Services.Core.IStorageManager
    {
        private Dictionary<string, string> _internal = new Dictionary<string, string>();

        public MockStorageManager()
        {
            _internal.Add("TwitterScreenName", "BarzolaGabriel");
        }

        public string Get(string key)
        {
            return _internal[key] ?? string.Empty;
        }

        public void Set(string key, string value)
        {
            _internal[key] = value;
        }
    }

    public class MockPasswordManager : Microsoft.Toolkit.Services.Core.IPasswordManager
    {
        private Dictionary<string, PasswordCredential> _internal = new Dictionary<string, PasswordCredential>();

        public MockPasswordManager()
        {
            _internal.Add("TwitterAccessToken", new PasswordCredential { Password = "q4wEazgXToQTFH9huYYeSF0vQ0RWgcC1dxixhIjGGGqSO", UserName = "1691361648-Cc8jorXshCazzp5dHzAELXbHtb2n6piM1DE6CMz" });
        }

        public PasswordCredential Get(string key)
        {
            return _internal[key];
        }

        public void Remove(string key)
        {

        }

        public void Store(string resource, PasswordCredential credential)
        {
            _internal[resource] = credential;
        }
    }
}
