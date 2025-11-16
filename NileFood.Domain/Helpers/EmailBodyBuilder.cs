namespace NileFood.Domain.Helpers;
public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {
        var templatePath = $"{Directory.GetCurrentDirectory()}/wwwroot/Templates/{template}";
        var streamReader = new StreamReader(templatePath);
        var body = streamReader.ReadToEnd();
        streamReader.Close();

        return templateModel.Aggregate(body, (current, item) => current.Replace(item.Key, item.Value));
    }
}
