using System.Net.Http;
using System;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json;

var _key = Environment.GetEnvironmentVariable("deepl_api_key");
var _client = new HttpClient();

var translateMore = true;
do
{
    await translate(_client, _key);
    Console.WriteLine("Would you like to translate more? y");
    var answer = Console.ReadLine();
    if (!(answer.ToUpper() == "Y"))
    {
        translateMore = false;
    }
} while (translateMore);

async Task translate(HttpClient client, string key)
{
    var textToBeTranslated = "";

    Console.WriteLine("What would you like to translate?");
    while (textToBeTranslated == "")
    {
        textToBeTranslated = Console.ReadLine();
    }

    Console.WriteLine("To what language do you want it to be translated?\n1: Deutsch\n2: English\n3: Français");
    var targetLanguage = "";
    while (targetLanguage == "")
    {
        targetLanguage = Console.ReadLine();
    }
    switch (targetLanguage.ToUpper())
    {
        case "1": case "DE": case "Deutsch":
            targetLanguage = "DE";
            break;
        case "2": case "EN": case "English":
            targetLanguage = "EN";
            break;
        case "3": case "FR":case "Français":
            targetLanguage = "FR";
            break;
    }

    var values = new Dictionary<string, string>
  {
    { "text", textToBeTranslated },
    { "target_lang", targetLanguage},
    { "auth_key", key }
  };

    var content = new FormUrlEncodedContent(values);

    var response = await client.PostAsync("https://api-free.deepl.com/v2/translate", content);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();

    DeeplResponse responseObj = JsonConvert.DeserializeObject<DeeplResponse>(responseString);

    Console.WriteLine("Source language: " + responseObj.translations[0].detected_source_language + "\nTranslation: " + responseObj.translations[0].text);
}

public class DeeplResponse
{
    public List<DeeplTranslation> translations { get; set; }
}

public class DeeplTranslation
{
    public string detected_source_language { get; set; }
    public string text { get; set; }
}

