/*
 * ============================================================================
 * Projekt:     CLI-Multithreaded Server-/Handler-Muster (Sequenzdiagramm)
 * Datei:       ServerHandler.cs
 * Autor:       [DEIN NAME HIER]
 * Datum:       [DATUM DER ABGABE]
 * Kurs:        [KURSBEZEICHNUNG / MODUL]
 * ============================================================================
 * Beschreibung:
 * Dieses C#-Programm demonstriert die Implementierung des Server-/Handler-Musters 
 * basierend auf einem UML-Sequenzdiagramm. Das Diagramm zeigt die Kommunikation 
 * zwischen einem Client, einem Server und einem Handler.
 *
 * Ablauf:
 * - Der Client sendet eine Anfrage (request) an den Server.
 * - Der Server erstellt einen Handler und delegiert die Aufgabe.
 * - Der Handler verarbeitet die Aufgabe in einem separaten Thread (Multithreading).
 * - Nach erfolgreicher Bearbeitung antwortet der Handler an den Client.
 * - Der Client erhält eine Abschlussnachricht (finished).
 *
 * Zielsetzung:
 * Die Implementierung soll das serverseitige Delegieren von Aufgaben an Threads 
 * zeigen, ohne dabei die Fähigkeit des Servers zu blockieren, weitere Anfragen 
 * anzunehmen. Dieses Muster ist typisch für Multitasking-Architekturen und wird 
 * in modernen verteilten Systemen und Serveranwendungen verwendet.
 *
 * Besonderheiten:
 * - Verwendung von Multithreading über System.Threading.Thread
 * - Klassenstruktur entspricht dem Diagramm: Client, Server, Handler
 * - Rückmeldung an den Client erfolgt per Callback-Methode
 * - Ausgaben im Konsolenstil zur einfachen Simulation
 *
 * Hinweis:
 * Dieser Code kann auch erweitert werden zu einer humorvollen Variante
 * (z. B. Baumarkt-/Pizzabestell-Server), ohne die fachliche Struktur zu verlieren.
 * ============================================================================
 */

using System;
using System.Threading;

public class Client
{
    private Server server;

    public Client(Server server)
    {
        this.server = server;
    }

    // Simuliert einen Request an den Server
    public void SendRequest(string aufgabe)
    {
        Console.WriteLine($" [Client] Sende Anfrage: '{aufgabe}'");
        server.HandleRequest(this, aufgabe); // Request() im Diagramm
    }

    // Wird vom Server aufgerufen, wenn die Aufgabe abgeschlossen ist
    public void Finished(string result)
    {
        Console.WriteLine($" [Client] Auftrag abgeschlossen: {result}"); // Finished()
    }
}

public class Server
{
    // Server delegiert Auftrag an Handler
    public void HandleRequest(Client client, string aufgabe)
    {
        Console.WriteLine("🖥️ [Server] Anfrage empfangen.");
        Handler handler = new Handler();
        // Delegation an Handler in neuem Thread
        Thread t = new Thread(() => handler.Delegate(client, aufgabe)); // delegate()
        t.Start();
    }
}

public class Handler
{
    public void Delegate(Client client, string aufgabe)
    {
        Console.WriteLine(" [Handler] Aufgabe übernommen. Bearbeite...");
        Thread.Sleep(2000); // Prozess simulieren (process task)
        string ergebnis = $"'{aufgabe}' wurde erfolgreich erledigt!";

        Console.WriteLine(" [Handler] Sende Ergebnis an Server/Client..."); // response()
        client.Finished(ergebnis); // Callback
    }
}

public class Program
{
    public static void Main()
    {
        Server server = new Server();
        Client client1 = new Client(server);

        // Client schickt eine Aufgabe
        client1.SendRequest("Regal aufbauen");

        // Optional: Weitere Clients gleichzeitig starten
        Client client2 = new Client(server);
        client2.SendRequest("Dübel einsortieren");

        Console.WriteLine(" [Main] Clients arbeiten parallel. Warten auf Threads...");
    }
}
