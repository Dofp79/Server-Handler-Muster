/*
* ============================================================================
 * Projekt:     CLI-Multithreaded Baumarkt-Server (Server-/Handler-Muster)
 * Datei:       ServerHandler.cs
 * Autor:       Doniman F. Peña Parra
 * Datum:       14. Mai 2025
 * Kurs:        betriebliche informationssysteme
 * ============================================================================
 * Beschreibung:
 * Dieses Programm simuliert prototypisch das Server-/Handler-Muster anhand 
 * eines lustigen Baumarkt-Szenarios. Der "Server" nimmt per Konsole Aufträge 
 * entgegen (z. B. „farbeimer:2“) und delegiert jede Auftragsposition an einen 
 * separaten Thread der "Lagerarbeiter"-Handler-Klasse. 
 *
 * So wird demonstriert, wie Server und Handler unabhängig arbeiten können:
 * Während der Lagerarbeiter im Hintergrund arbeitet, kann der Server weiter 
 * Eingaben annehmen.
 * ============================================================================
 */

using System;
using System.Threading;

// Handler-Klasse: Simuliert einen Lagerarbeiter, der Aufträge parallel bearbeitet
public class Lagerarbeiter
{
    private string produkt;
    private int menge;

    // Sprüche zur Ausgabe nach Erledigung – humorvoll für mehr Spaß
    private static string[] sprueche = new[]
    {
        " Hab ich aus Regal 3 geholt.",
        " Musste kurz suchen, aber gefunden!",
        " Das lag ganz hinten im Lager!",
        " Schon erledigt, Chef!",
        " Das war schwer, aber ich hab's!"
    };

    private static Random rand = new Random(); // Für zufällige Sprüche und Bearbeitungszeit

    // Konstruktor: Speichert Produktname und Menge
    public Lagerarbeiter(string produkt, int menge)
    {
        this.produkt = produkt;
        this.menge = menge;
    }

    // Simuliert die Bearbeitung des Auftrags
    public void BearbeitePosition()
    {
        Console.WriteLine($" Lagerarbeiter bekommt Auftrag: {menge}x {produkt}");
        Thread.Sleep(rand.Next(2000, 4000)); // Wartezeit 2–4 Sekunden zur Simulation
        Console.WriteLine($" {menge}x {produkt} bereitgestellt. {sprueche[rand.Next(sprueche.Length)]}");
    }
}

// Server-Klasse: Nimmt Bestellungen entgegen und startet jeweils einen Lagerarbeiter-Thread
public class AuftragsServer
{
    // Hauptmethode zum Starten des Servers
    public void Start()
    {
        Console.WriteLine("Willkommen beim lustigen Baumarkt-Bestellserver!");
        Console.WriteLine("Format: <Produktname>:<Menge>  | z. B. schraube:10");
        Console.WriteLine("Gib 'exit' ein zum Beenden.");

        while (true)
        {
            Console.Write("\n📥 >> ");
            string? input = Console.ReadLine(); // Lese Nutzereingabe

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(" Bitte was Vernünftiges eingeben.");
                continue;
            }

            if (input.Trim().ToLower() == "exit")
            {
                Console.WriteLine(" Bis zum nächsten Einkauf!");
                break; // Beende die Schleife (Programm)
            }

            var teile = input.Split(":"); // Erwartetes Format: "produkt:menge"

            // Überprüfen, ob die Eingabe korrekt formatiert ist
            if (teile.Length != 2 || !int.TryParse(teile[1], out int menge))
            {
                Console.WriteLine(" Ungültiges Format! Bitte wie 'farbeimer:2' eingeben.");
                continue;
            }

            string produkt = teile[0].Trim();

            if (menge <= 0)
            {
                Console.WriteLine(" Ernsthaft? Mindestens 1 Stück, bitte.");
                continue;
            }

            // Erstelle neuen Handler (Lagerarbeiter) und starte Thread
            Lagerarbeiter handler = new Lagerarbeiter(produkt, menge);
            Thread t = new Thread(handler.BearbeitePosition);
            t.Start();

            Console.WriteLine($" Auftrag aufgenommen: {menge}x {produkt} – wird bearbeitet...");
        }
    }

    // Einstiegspunkt des Programms
    public static void Main()
    {
        AuftragsServer server = new AuftragsServer();
        server.Start();
    }
}
