var serviceCollection = new ServiceCollection();

Services.ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

var app = serviceProvider.GetService<IWhatsAppApplication>();
app.ShoppingBot("@mercado");