using NCacheTesting.Models;
using NCacheTesting.Services;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Config.Dom;

string cacheName = "ctPartitionedReplicaCache";

try
{
    ICache cache = CacheManager.GetCache(cacheName);
    Console.WriteLine("Cache connected successfully.");

    var cacheService = new CacheService(cache);
    var product = new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m };
    string key = $"Product:{product.Id}";
    cache.Clear();

    CacheItem cacheItem = new CacheItem(product);
    cacheItem.Priority = Alachisoft.NCache.Runtime.CacheItemPriority.AboveNormal;

    // Add
    Console.WriteLine("Adding item to cache...");
  //  cacheService.Add(key, product);

    // Get
    Console.WriteLine("Fetching item from cache...");

    cacheItem.Expiration = new Alachisoft.NCache.Runtime.Caching.Expiration(Alachisoft.NCache.Runtime.Caching.ExpirationType.Absolute, TimeSpan.FromSeconds(5));

    try
    {
        int itemSizeInKB = 200; // each item = 50KB
        int counter = 0;
        long cachecount = 0;

        while (true)
        {
            byte[] data = new byte[itemSizeInKB * 1024];
            new Random().NextBytes(data);
            var keyName = "key_" + counter;

            CacheItem cacheItem123 = new CacheItem(data);

            if (keyName == "key_0")
            {
                cacheItem123.Priority = Alachisoft.NCache.Runtime.CacheItemPriority.High;
            }


            cache.Add(keyName, cacheItem123);

            if(true)
            {
                var cacheItem1 = cache.Get<byte[]>(keyName);
            }

            Console.WriteLine($"Added item {counter}");
            counter++;
            cachecount = cache.Count;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Cache full or error: {ex.Message}");
    }
    var keyn = "key_0";

    for (int i = 0; i < 20; i++)
    {
         keyn = "key_" + i;
         var cacheItem1 = cache.Get<byte[]>(keyn);
        if (cacheItem1 is null)
        {
            Console.WriteLine($"evicted item {keyn}");
        }
        else
        {
            Console.WriteLine($"Fetched item {keyn}");
        }
    }



    var size =  cache.Count;

    var cacheitem =  cache.GetCacheItem("test1234");

    var cacheitem1 = cache.GetCacheItem("test1234");

    var prodcut =  cacheItem .GetValue<Product>();

    cache.Remove("test1234");





    // Update
    Console.WriteLine("Updating item in cache...");
    product.Price = 1150.00m;
    cacheService.Update(key, product);

    // Get Updated
    Console.WriteLine("Fetching updated item from cache...");
  //  cachedProduct = cacheService.Get<Product>(key);
   // Console.WriteLine($"Retrieved Updated: {cachedProduct}");

    // Remove
    Console.WriteLine("Removing item from cache...");
    cacheService.Remove(key);

    // Verify Removal
    if (!cacheService.Exists(key))
    {
        Console.WriteLine("Item successfully removed from cache.");
    }
    else
    {
        Console.WriteLine("Error: Item still exists in cache.");
    }

    // --- Bulk Operations ---
    Console.WriteLine("\n--- Bulk Operations ---");
    var products = new Dictionary<string, Product>
    {
        { "Product:101", new Product { Id = 101, Name = "Mouse", Category = "Peripherals", Price = 25.00m } },
        { "Product:102", new Product { Id = 102, Name = "Keyboard", Category = "Peripherals", Price = 45.00m } },
        { "Product:103", new Product { Id = 103, Name = "Monitor", Category = "Peripherals", Price = 150.00m } }
    };

    // Add Bulk
    Console.WriteLine("Adding bulk items to cache...");
    cacheService.AddBulk(products);

    // Get Bulk
    Console.WriteLine("Fetching bulk items from cache...");
    var bulkKeys = products.Keys.ToList();
    var cachedProducts = cacheService.GetBulk<Product>(bulkKeys);
    foreach (var kvp in cachedProducts)
    {
        Console.WriteLine($"Retrieved: {kvp.Key} -> {kvp.Value}");
    }

    // Update Bulk
    Console.WriteLine("Updating bulk items in cache...");
    foreach (var p in products.Values)
    {
        p.Price += 10.00m; // Increase price
    }
    cacheService.UpdateBulk(products);

    // Get Bulk Updated
    Console.WriteLine("Fetching updated bulk items from cache...");
    cachedProducts = cacheService.GetBulk<Product>(bulkKeys);
    foreach (var kvp in cachedProducts)
    {
        Console.WriteLine($"Retrieved Updated: {kvp.Key} -> {kvp.Value}");
    }

    // Remove Bulk
    Console.WriteLine("Removing bulk items from cache...");
    cacheService.RemoveBulk(bulkKeys);

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("Press Enter to exit...");
Console.ReadLine();
