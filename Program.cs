// See https://aka.ms/new-console-template for more information
using Alachisoft.NCache.Client;

Console.WriteLine("Hello, World!");

string cacheName = "demoCache";

// Connect to cache
ICache cache = CacheManager.GetCache(cacheName);

Console.WriteLine("cache connected");
