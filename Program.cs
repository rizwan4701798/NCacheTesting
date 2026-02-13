// See https://aka.ms/new-console-template for more information
using Alachisoft.NCache.Client;

Console.WriteLine("Hello, World!");

string cacheName = "demoCache";

// Connect to cache
var options = new CacheConnectionOptions();

options.UserCredentials = new Credentials("test", "test12");

ICache cache = CacheManager.GetCache(cacheName, options);

Console.WriteLine("cache connected");

Console.ReadLine();
