using NCacheTesting.Models;
using NCacheTesting.Services;
using Alachisoft.NCache.Client;

string cacheName = "demoCache";
var options = new CacheConnectionOptions();
options.UserCredentials = new Credentials("test", "test12");

try
{
    ICache cache = CacheManager.GetCache(cacheName, options);
    Console.WriteLine("Cache connected successfully.");

    var cacheService = new CacheService(cache);
    var product = new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m };
    string key = $"Product:{product.Id}";

    // Add
    Console.WriteLine("Adding item to cache...");
    cacheService.Add(key, product);

    // Get
    Console.WriteLine("Fetching item from cache...");
    var cachedProduct = cacheService.Get<Product>(key);
    Console.WriteLine($"Retrieved: {cachedProduct}");

    // Update
    Console.WriteLine("Updating item in cache...");
    product.Price = 1150.00m;
    cacheService.Update(key, product);

    // Get Updated
    Console.WriteLine("Fetching updated item from cache...");
    cachedProduct = cacheService.Get<Product>(key);
    Console.WriteLine($"Retrieved Updated: {cachedProduct}");

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
