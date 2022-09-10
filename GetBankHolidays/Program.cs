using System.Net;
using System;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Globalization;

string endpoint = "https://www.gov.uk/bank-holidays.json";

using (HttpClient client = new HttpClient())
{
    HttpResponseMessage responce = await client.GetAsync(endpoint);
    string responceBody = await responce.Content.ReadAsStringAsync();

    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(responceBody)!;
    var cultureInfo = new CultureInfo("en-GB");

    List<DateTime> bankHolidays = new List<DateTime>();
    foreach (var bankHoliday in rootObject.EnglandAndWales.events)
    {
        DateTime bh = DateTime.ParseExact(bankHoliday.date, "yyyy-MM-dd", cultureInfo);
        if (bh >= DateTime.Now)
            bankHolidays.Add(bh);
    }

    bankHolidays.ForEach(x => Console.WriteLine(x.ToString()));
}

Console.WriteLine("Hello, World!");

public class Event
{
    public string title { get; set; }
    public string date { get; set; }
    public string notes { get; set; }
    public bool bunting { get; set; }
}

public class EnglandAndWales
{
    public string division { get; set; }
    public List<Event> events { get; set; }
}

public class Scotland
{
    public string division { get; set; }
    public List<Event> events { get; set; }
}

public class NorthernIreland
{
    public string division { get; set; }
    public List<Event> events { get; set; }
}

public class RootObject
{
    [JsonProperty(PropertyName = "england-and-wales")]
    public EnglandAndWales EnglandAndWales { get; set; }
    public Scotland scotland { get; set; }
    [JsonProperty(PropertyName = "northern-ireland")]
    public NorthernIreland NorthernIreland { get; set; }
}