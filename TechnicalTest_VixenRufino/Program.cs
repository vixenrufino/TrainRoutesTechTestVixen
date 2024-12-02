using System;
using System.Collections.Generic;
using System.IO;

public class TrainRoute
{
    public string From { get; set; }
    public string To { get; set; }
    public int Distance { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        string inputFile = "Input.txt";
        var graph = LoadGraph(inputFile);

        // Test #1: The distance of the route A=>B=>C
        Console.WriteLine("Test #1: " + CalculateRouteDistance(graph, new[] { "A", "B", "C" }));
        // Test #2: The distance of the route A=>D
        Console.WriteLine("Test #2: " + CalculateRouteDistance(graph, new[] { "A", "D" }));
        // Test #3: The distance of the route A=>D=>C
        Console.WriteLine("Test #3: " + CalculateRouteDistance(graph, new[] { "A", "D", "C" }));
        // Test #4: The distance of the route A=>E=>B=>C=>D
        Console.WriteLine("Test #4: " + CalculateRouteDistance(graph, new[] { "A", "E", "B", "C", "D" }));
        // Test #5: Route A=>E=>D doesn't exist
        Console.WriteLine("Test #5: " + (CalculateRouteDistance(graph, new[] { "A", "E", "D" }) == -1 ? "No route" : "Route exists"));
        // Test #6: The number of trips from C to C with maximum 3 stops
        Console.WriteLine("Test #6: " + CountTripsWithMaxStops(graph, "C", "C", 3));
        // Test #7: The number of trips from A to C with exactly 4 stops
        Console.WriteLine("Test #7: " + CountTripsWithExactStops(graph, "A", "C", 4));
        // Test #8: The length of the shortest route from A to C
        Console.WriteLine("Test #8: " + FindShortestRoute(graph, "A", "C"));
        // Test #9: The length of the shortest route from B to B
        Console.WriteLine("Test #9: " + FindShortestRoute(graph, "B", "B"));
        // Test #10: The number of trips from C to C with distance less than 30
        Console.WriteLine("Test #10: " + CountTripsWithMaxDistance(graph, "C", "C", 30));
    }

    // Load the graph from the input file
    public static Dictionary<string, List<TrainRoute>> LoadGraph(string inputFile)
    {
        var graph = new Dictionary<string, List<TrainRoute>>();

        foreach (var line in File.ReadLines(inputFile))
        {
            var parts = line.Split(',');
            var from = parts[0].Trim();
            var to = parts[1].Trim();
            var distance = int.Parse(parts[2].Trim());

            if (!graph.ContainsKey(from))
                graph[from] = new List<TrainRoute>();
            if (!graph.ContainsKey(to))
                graph[to] = new List<TrainRoute>();

            graph[from].Add(new TrainRoute { From = from, To = to, Distance = distance });
        }
        return graph;
    }

    // Calculate the distance of a route (e.g., A=>B=>C)
    public static int CalculateRouteDistance(Dictionary<string, List<TrainRoute>> graph, string[] route)
    {
        int totalDistance = 0;
        for (int i = 0; i < route.Length - 1; i++)
        {
            var from = route[i];
            var to = route[i + 1];
            var found = false;

            foreach (var routeInfo in graph[from])
            {
                if (routeInfo.To == to)
                {
                    totalDistance += routeInfo.Distance;
                    found = true;
                    break;
                }
            }

            if (!found) return -1;  // If any part of the route is not found, return -1
        }
        return totalDistance;
    }

    // Dijkstra's Algorithm to find the shortest route between two towns
    public static int FindShortestRoute(Dictionary<string, List<TrainRoute>> graph, string start, string end)
    {
        var distances = new Dictionary<string, int>();
        var priorityQueue = new Dictionary<int, List<string>>();  // Dictionary to store distances and their corresponding towns
        var visited = new HashSet<string>();

        foreach (var town in graph.Keys)
        {
            distances[town] = int.MaxValue;
        }
        distances[start] = 0;
        AddToPriorityQueue(priorityQueue, 0, start);

        while (priorityQueue.Count > 0)
        {
            var currentTown = GetNextTown(priorityQueue);
            if (currentTown == null) break;

            var current = currentTown;
            visited.Add(current);

            if (current == end)
                return distances[end];

            foreach (var route in graph[current])
            {
                if (visited.Contains(route.To)) continue;

                var newDist = distances[current] + route.Distance;
                if (newDist < distances[route.To])
                {
                    distances[route.To] = newDist;
                    AddToPriorityQueue(priorityQueue, newDist, route.To);
                }
            }
        }

        return -1; // No route found
    }

    // Helper method to add to the priority queue (dictionary)
    public static void AddToPriorityQueue(Dictionary<int, List<string>> priorityQueue, int distance, string town)
    {
        if (!priorityQueue.ContainsKey(distance))
        {
            priorityQueue[distance] = new List<string>();
        }
        priorityQueue[distance].Add(town);
    }

    // Helper method to retrieve the next town with the smallest distance from the priority queue
    public static string GetNextTown(Dictionary<int, List<string>> priorityQueue)
    {
        foreach (var key in priorityQueue.Keys)
        {
            var towns = priorityQueue[key];
            if (towns.Count > 0)
            {
                var town = towns[0];
                towns.RemoveAt(0);
                if (towns.Count == 0)
                {
                    priorityQueue.Remove(key);
                }
                return town;
            }
        }
        return null;
    }

    // Count the number of trips from start to end with a maximum number of stops
    public static int CountTripsWithMaxStops(Dictionary<string, List<TrainRoute>> graph, string start, string end, int maxStops)
    {
        int tripCount = 0;
        FindTripsWithMaxStops(graph, start, end, maxStops, 0, ref tripCount);
        return tripCount;
    }

    public static void FindTripsWithMaxStops(Dictionary<string, List<TrainRoute>> graph, string current, string end, int maxStops, int stops, ref int tripCount)
    {
        if (stops > maxStops) return;

        if (current == end && stops > 0)
        {
            tripCount++;
            return;
        }

        foreach (var route in graph[current])
        {
            FindTripsWithMaxStops(graph, route.To, end, maxStops, stops + 1, ref tripCount);
        }
    }

    // Count the number of trips with exactly a given number of stops
    public static int CountTripsWithExactStops(Dictionary<string, List<TrainRoute>> graph, string start, string end, int exactStops)
    {
        int tripCount = 0;
        FindTripsWithExactStops(graph, start, end, exactStops, 0, ref tripCount);
        return tripCount;
    }

    public static void FindTripsWithExactStops(Dictionary<string, List<TrainRoute>> graph, string current, string end, int exactStops, int stops, ref int tripCount)
    {
        if (stops == exactStops)
        {
            if (current == end)
                tripCount++;
            return;
        }

        foreach (var route in graph[current])
        {
            FindTripsWithExactStops(graph, route.To, end, exactStops, stops + 1, ref tripCount);
        }
    }

    // Count the number of trips with distance less than a given threshold
    public static int CountTripsWithMaxDistance(Dictionary<string, List<TrainRoute>> graph, string start, string end, int maxDistance)
    {
        int tripCount = 0;
        FindTripsWithMaxDistance(graph, start, end, maxDistance, 0, ref tripCount);
        return tripCount;
    }

    public static void FindTripsWithMaxDistance(Dictionary<string, List<TrainRoute>> graph, string current, string end, int maxDistance, int currentDistance, ref int tripCount)
    {
        if (currentDistance >= maxDistance) return;

        if (current == end && currentDistance > 0)
        {
            tripCount++;
            return;
        }

        foreach (var route in graph[current])
        {
            FindTripsWithMaxDistance(graph, route.To, end, maxDistance, currentDistance + route.Distance, ref tripCount);
        }
    }
}
