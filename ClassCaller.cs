/*
 * ============================================================================
 * Projekt:     Asynchroner Aufruf mit Callback (Server-/Handler-Muster)
 * Datei:       AsynchronerAufrufMitCallback.cs
 * Autor:       [DEIN NAME HIER]
 * Datum:       [DATUM DER ABGABE]
 * Kurs:        [MODUL / KURSNAME]
 * ============================================================================
 * Beschreibung:
 * In dieser Variante des Server-/Handler-Musters wird der Abschluss der
 * asynchronen Arbeit nicht aktiv vom Client überprüft (Polling), sondern der
 * Client erhält eine Rückmeldung per Callback.
 * ============================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

public class Caller
{
    private Action<string> callback; // Callback-Methode (an Client übergeben)

    // Startet die asynchrone Verarbeitung
    public void StartWork(Action<string> onComplete)
    {
        callback = onComplete;

        Task.Run(() =>
        {
            Console.WriteLine("[Caller] Starte aufwendige Arbeit...");
            Thread.Sleep(3000); // Simuliere lange laufende Aufgabe
            string result = "Arbeit erfolgreich abgeschlossen.";

            Console.WriteLine("[Caller] Arbeit beendet. Rufe Callback auf...");
            callback(result); // Rückmeldung an Client
        });
    }
}

public class Client
{
    public static void Main()
    {
        Console.WriteLine("[Client] Startet und delegiert Aufgabe...");

        Caller caller = new Caller();

        // Übergibt dem Caller die Methode, die bei Fertigstellung aufgerufen werden soll
        caller.StartWork(OnWorkFinished);

        Console.WriteLine("[Client] Tut währenddessen andere Dinge... ");

        // Hauptthread künstlich am Leben halten
        Thread.Sleep(5000); // In echter App: Event-Schleife, UI, Serverwartung etc.
    }

    // Callback-Methode: Wird vom Caller aufgerufen
    public static void OnWorkFinished(string result)
    {
        Console.WriteLine($"[Client] Ergebnis empfangen: {result}");
    }
}
