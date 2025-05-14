/*
 * ============================================================================
 * Projekt:     Asynchroner Aufruf mit Callback (Server-/Handler-Muster)
 * Datei:       AsynchronerAufruf.cs
 * Autor:       Doniman F. Peña Parra
 * Datum:       14. Mai 2025
 * Kurs:        betriebliche informationssysteme
 * ============================================================================
 * Beschreibung:
 * Diese C#-Implementierung des Server-/Handler-Musters verwendet Callbacks
 * zur asynchronen Ergebnisverarbeitung. Der Client startet eine Aufgabe,
 * ohne aktiv auf deren Abschluss zu warten. Sobald die Aufgabe abgeschlossen
 * ist, informiert der Handler den Client über eine Callback-Funktion.
 *
 * Ablauf:
 * - Der Client ruft `StartWork()` beim Handler (Caller) auf.
 * - Der Handler führt die Aufgabe im Hintergrund (Task.Run) aus.
 * - Nach Abschluss ruft der Handler die Callback-Methode auf.
 * - Der Client erhält das Ergebnis über `ErgebnisVerarbeiten`.
 *
 * Vorteile:
 * - Kein aktives Warten (Polling) notwendig
 * - Nebenläufige Verarbeitung durch Task & Callback
 * - Klar strukturierte Trennung von Client- und Handler-Verantwortung
 * ============================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

// 🔧 Handler-Klasse: Führt Aufgabe asynchron aus und ruft Client-Callback auf
public class Caller
{
    private Action<string> _callback; // Methode, die nach Abschluss aufgerufen wird

    // Konstruktor: Empfängt Callback vom Client
    public Caller(Action<string> callback)
    {
        _callback = callback;
    }

    // Startet die asynchrone Arbeit im Hintergrund
    public void StartWork()
    {
        Task.Run(() =>
        {
            Console.WriteLine("[Caller] Beginne Verarbeitung...");
            Thread.Sleep(3000); // Simuliert lange Berechnung

            string result = "Callback-Ergebnis: Verarbeitung abgeschlossen.";
            _callback(result); // Callback ausführen, Rückgabe an Client
        });
    }
}

// 🧠 Client-Klasse: Startet den Prozess und definiert die Callback-Methode
public class Client
{
    public static void Main()
    {
        Console.WriteLine("🧠 Client delegiert Arbeit und macht sofort weiter...");

        // Übergibt Methode als Callback an den Caller
        Caller caller = new Caller(ErgebnisVerarbeiten);
        caller.StartWork();

        Console.WriteLine("[Client] Arbeitet weiter und wartet nicht...");

        // Warten, um Callback zu empfangen (sonst würde Programm sofort enden)
        Thread.Sleep(5000);
    }

    // Callback-Methode: Wird vom Caller aufgerufen, sobald Ergebnis vorliegt
    public static void ErgebnisVerarbeiten(string result)
    {
        Console.WriteLine($"✅ Client erhält Callback: {result}");
    }
}
