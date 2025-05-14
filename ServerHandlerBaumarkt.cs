using System;
using System.Threading;

public class Lagerarbeiter
{
    private string produkt;
    private int menge;
    private static string[] sprueche = new[]
    {
        "✅ Hab ich aus Regal 3 geholt.",
        "📦 Musste kurz suchen, aber gefunden!",
        "🔧 Das lag ganz hinten im Lager!",
        "🛠️ Schon erledigt, Chef!",
        "😂 Das war schwer, aber ich hab's!"
    };
    private static Random rand = new Random();

    public Lagerarbeiter(string produkt, int menge)
    {
        this.produkt = produkt;
        this.menge = menge;
    }

    public void BearbeitePosition()
    {
        Console.WriteLine($"👷 Lagerarbeiter bekommt Auftrag: {menge}x {produkt}");
        Thread.Sleep(rand.Next(2000, 4000)); // simuliere Aufwand
        Console.WriteLine($"👷 {menge}x {produkt} bereitgestellt. {sprueche[rand.Next(sprueche.Length)]}");
    }
}

public class AuftragsServer
{
    public void Start()
    {
        Console.WriteLine("🛒 Willkommen beim lustigen Baumarkt-Bestellserver!");
        Console.WriteLine("Format: <Produktname>:<Menge>  | z. B. schraube:10");
        Console.WriteLine("Gib 'exit' ein zum Beenden.");

        while (true)
        {
            Console.Write("\n📥 >> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("😅 Bitte was Vernünftiges eingeben.");
                continue;
            }

            if (input.Trim().ToLower() == "exit")
            {
                Console.WriteLine("👋 Bis zum nächsten Einkauf!");
                break;
            }

            var teile = input.Split(":");

            if (teile.Length != 2 || !int.TryParse(teile[1], out int menge))
            {
                Console.WriteLine("⚠️ Ungültiges Format! Bitte wie 'farbeimer:2' eingeben.");
                continue;
            }

            string produkt = teile[0].Trim();
            if (menge <= 0)
            {
                Console.WriteLine("🙄 Ernsthaft? Mindestens 1 Stück, bitte.");
                continue;
            }

            Lagerarbeiter handler = new Lagerarbeiter(produkt, menge);
            Thread t = new Thread(handler.BearbeitePosition);
            t.Start();

            Console.WriteLine($"📑 Auftrag aufgenommen: {menge}x {produkt} – wird bearbeitet...");
        }
    }

    public static void Main()
    {
        AuftragsServer server = new AuftragsServer();
        server.Start();
    }
}
