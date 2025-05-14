/*
 * ============================================================================
 * Projekt:     CLI-Multithreaded Baumarkt-Server (Server-/Handler-Muster)
 * Datei:       ServerHandler.cs
 * Autor:       Doniman F. Peña Parra
 * Datum:       14. Mai 2025
 * Kurs:        betriebliche informationssysteme
 * ============================================================================
 * Beschreibung:
 * Dieses C#-Programm implementiert prototypisch das Server-/Handler-Muster aus 
 * der Kurseinheit. Der Server simuliert einen lustigen Baumarkt-Bestellservice 
 * auf Kommandozeilenbasis. Bestellungen wie "schraube:10" oder "farbeimer:2" 
 * werden entgegengenommen und jeweils an eine separate Handler-Klasse 
 * (Lagerarbeiter) delegiert, die in einem eigenen Thread die Bestellung "bearbeitet".
 *
 * Ziel:
 * Demonstration eines parallelen Verarbeitungskonzepts, bei dem der Server 
 * jederzeit neue Aufträge annehmen kann, während die Bearbeitung durch 
 * die Handler im Hintergrund erfolgt.
 *
 * Besondere Merkmale:
 * - CLI-Interface zur Auftragseingabe
 * - Mehrere Handler-Threads arbeiten unabhängig
 * - Humorvolle Ausgaben pro Auftrag
 * - Einfache Datenvalidierung
 * - Nutzung von Klassen gemäß ER-Diagramm-ähnlicher Struktur
 *
 * Hinweis zur Abgabe:
 * Die Datei ServerHandlerBaumarkt.cs" ist gemäß den Anforderungen benannt und kann 
 * direkt in Moodle eingereicht werden.
 * ============================================================================
 */

using System;
using System.Threading;

//  Handler-Klasse: Repräsentiert einen Pizzabäcker, der Bestellungen bearbeitet
public class PizzaChef
{
    private string pizzaType;
    private static Random random = new Random();

    // Konstruktor: Erhält die bestellte Pizzasorte
    public PizzaChef(string pizzaType)
    {
        this.pizzaType = pizzaType;
    }

    // Methode, die den "Backvorgang" simuliert – läuft in einem separaten Thread
    public void BakePizza()
    {
        Console.WriteLine($" [Küche] Bestellung '{pizzaType}' erhalten. Heize Ofen vor...");
        int bakeTime = random.Next(2, 5); // Simuliere Backzeit von 2–4 Sekunden
        Thread.Sleep(bakeTime * 1000);    // Blockiere den Thread für die "Zubereitung"
        Console.WriteLine($" [Küche] Deine '{pizzaType}' ist fertig gebacken nach {bakeTime} Sekunden!");
    }
}

//  Server-Klasse: Nimmt Bestellungen entgegen und startet für jede einen Handler-Thread
public class PizzaServer
{
    // Methode zum Starten des Servers – wird aus Main() aufgerufen
    public void Start()
    {
        Console.WriteLine(" Willkommen beim Multi-Threaded Pizza-Service!");
        Console.WriteLine("Gib eine Pizzasorte ein (z. B. Margherita, Salami) oder 'exit' zum Beenden:");

        while (true) // Endlosschleife für Auftragsannahme
        {
            Console.Write("\n🍽️ >> "); // Eingabeaufforderung
            string? input = Console.ReadLine(); // Lese Benutzereingabe

            // Leere oder ungültige Eingabe behandeln
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(" Bitte eine gültige Pizzasorte eingeben!");
                continue;
            }

            input = input.Trim(); // Leerzeichen entfernen

            // Falls Benutzer 'exit' eingibt → Server beenden
            if (input.ToLower() == "exit")
            {
                Console.WriteLine(" Der Ofen ist aus. Bis bald!");
                break;
            }

            // Erstelle neuen Handler (PizzaChef) mit dem eingegebenen Pizzatyp
            PizzaChef chef = new PizzaChef(input);

            // Starte neuen Thread mit der BakePizza()-Methode
            Thread thread = new Thread(new ThreadStart(chef.BakePizza));
            thread.Start();

            Console.WriteLine($" [Server] Bestellung für '{input}' aufgenommen. Weiter geht’s!");
        }
    }

    // Einstiegspunkt: Initialisiert und startet den Server
    public static void Main()
    {
        PizzaServer server = new PizzaServer();
        server.Start();
    }
}
