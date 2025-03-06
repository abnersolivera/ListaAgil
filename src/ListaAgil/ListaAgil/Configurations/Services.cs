namespace ListaAgil.Configurations;

public static class Services
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ISeleniumService, SeleniumService>();
        services.AddSingleton<IWhatsAppApplication, WhatsAppApplication>();
    }
}