This is a .NET Console application that finds train routes between towns given an input file containing direct links from one town to another and their distances.

Design Decisions

1. Graph Representation
The system represents the train network as a directed graph where:
	- Nodes represent towns.
	- Edges represent direct routes between towns, each with a distance.

2. Algorithms Used
	- Route Distance Calculation: A simple traversal over the input route is done to calculate the total distance.
	- Shortest Path Calculation (Dijkstra's Algorithm): I use Dijkstra's algorithm to find the shortest route between two towns, considering edge weights as the distances.
	- Route Counting: Recursion is used to count the number of trips based on conditions like the number of stops or distance constraints.

3. Data Structures
	- Dictionary<string, List<TrainRoute>>: This is used to represent the graph. The key is the town name (a string), and the value is a list of `TrainRoute` objects representing the outgoing routes from that town.
	- TrainRoute Class: This class contains `From`, `To`, and `Distance` properties to represent each route.

4. Error Handling
	- If a route cannot be found (e.g., no direct link between towns), we return `-1`.
	- The application is designed to handle both valid and invalid inputs gracefully.

5. Unit Testing
Unit tests are written using the xUnit framework. The tests verify the correctness of the key methods, including:
	- Route Distance Calculation: Ensures that the distance for a given route is calculated correctly.
	- Shortest Path Calculation: Verifies that the shortest path between two towns is found.
	- Route Counting: Validates that the correct number of trips is counted under different conditions.

6. Running the Application
To run the application, place the input data in a file called `Input.txt` in the directory "C:\Users\Chen\source\repos\TechnicalTest_VixenRufino\TechnicalTest_VixenRufino\bin\Debug\net8.0" same as the executable.

