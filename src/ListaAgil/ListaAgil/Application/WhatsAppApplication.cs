namespace ListaAgil.Application;

public class WhatsAppApplication(ISeleniumService seleniumService) : IWhatsAppApplication
{
    public void ShoppingBot(string to)
    {
        seleniumService.StartBrowser();
        seleniumService.NavigateToWhatsApp();
        seleniumService.WaitForLoad();

        var elementSearch =
            seleniumService.FindElement(By.XPath("//*[@id=\"side\"]/div[1]/div/div[2]/div/div/div[1]/p"));
        elementSearch.SendKeys(to);
        elementSearch.SendKeys(Keys.Enter);

        Dictionary<string, (int? quantity, decimal? price, decimal? total)> shoppingList = new();
        bool isShopping = false;

        while (true)
        {
            string text = GetLastMessageText();

            if (text.Equals("iniciar compra", StringComparison.InvariantCultureIgnoreCase) && !isShopping)
            {
                isShopping = true;
                shoppingList.Clear();
                Console.WriteLine("Iniciando compra...");
            }
            else if (text.Equals("finalizar compra", StringComparison.InvariantCultureIgnoreCase))
            {
                isShopping = false;

                var message = new StringBuilder();
                message.AppendLine($"Compra finalizada - {DateTime.Now}");

                foreach (var item in shoppingList)
                {
                    string itemMessage =
                        $"{item.Key}: {item.Value.quantity} unidades - R$ {item.Value.price:F2} - Total R$ {item.Value.total:F2}";
                    message.AppendLine(itemMessage);
                }

                decimal? subtotal = shoppingList.Sum(i => i.Value.total);

                message.AppendLine($"Subtotal: R$ {subtotal:F2}");

                GetMessageElement().SendKeys(message.ToString());
            }
            else if (isShopping)
            {
                string pattern = @"^(?<product>.+?)\s+(?<quantity>\d+)\s+(?<price>\d+,\d{2})$";
                Match match = Regex.Match(text, pattern);

                if (match.Success)
                {
                    string product = match.Groups["product"].Value;
                    int quantity = int.Parse(match.Groups["quantity"].Value);
                    decimal price = decimal.Parse(match.Groups["price"].Value);

                    if (shoppingList.ContainsKey(product))
                        shoppingList[product] = (quantity, price, quantity * price);
                    else
                    {
                        shoppingList[product] = (quantity, price, quantity * price);
                        Console.WriteLine($"Adicionado: {product} - {quantity} unidades - R$ {price:F2}");
                    }
                }
            }
        }
    }

    private IWebElement GetMessageElement() => seleniumService.FindElement(
        By.XPath("//*[@id=\"main\"]/footer/div[1]/div/span/div/div[2]/div[1]/div[2]/div[1]/p"));

    private string GetLastMessageText()
    {
        var elementMensagens = seleniumService.FindElements(By.XPath("//div[contains(@data-id, '@g.us')]"));
        var mensagem = elementMensagens.LastOrDefault();
        var textoElemento =
            mensagem.FindElement(By.XPath(".//span[contains(@class, 'selectable-text copyable-text')]"));

        string text = string.Empty;

        try
        {
            text = textoElemento.Text;
        }
        catch (StaleElementReferenceException)
        {
            Console.WriteLine("⚠ Elemento foi removido do DOM. Ignorando...");
        }

        return text;
    }
}