using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace UnitTestProject1
{
    public class UnitTest1
    {

        // Sample data from the problem description
        private readonly Dictionary<string, List<TrainRoute>> _graph;

        public UnitTest1()
        {
            // Sample graph setup
            _graph = new Dictionary<string, List<TrainRoute>>
            {
                { "A", new List<TrainRoute> { new TrainRoute { From = "A", To = "B", Distance = 5 }, new TrainRoute { From = "A", To = "D", Distance = 5 } } },
                { "B", new List<TrainRoute> { new TrainRoute { From = "B", To = "C", Distance = 4 } } },
                { "C", new List<TrainRoute> { new TrainRoute { From = "C", To = "E", Distance = 3 } } },
                { "D", new List<TrainRoute> { new TrainRoute { From = "D", To = "C", Distance = 8 }, new TrainRoute { From = "D", To = "E", Distance = 7 } } },
                { "E", new List<TrainRoute> { new TrainRoute { From = "E", To = "B", Distance = 3 } } }
            };
        }

        // Test #1: The distance of the route A=>B=>C
        [Fact]
        public void CalculateRouteDistance_AtoBtoC_ShouldReturn9()
        {
            var route = new[] { "A", "B", "C" };
            var distance = Program.CalculateRouteDistance(_graph, route);
            Assert.Equal(9, distance);
        }

        // Test #2: The distance of the route A=>D
        [Fact]
        public void CalculateRouteDistance_AtoD_ShouldReturn5()
        {
            var route = new[] { "A", "D" };
            var distance = Program.CalculateRouteDistance(_graph, route);
            Assert.Equal(5, distance);
        }

        // Test #3: The distance of the route A=>D=>C
        [Fact]
        public void CalculateRouteDistance_AtoDtoC_ShouldReturn13()
        {
            var route = new[] { "A", "D", "C" };
            var distance = Program.CalculateRouteDistance(_graph, route);
            Assert.Equal(13, distance);
        }

        // Test #5: Route A=>E=>D doesn't exist
        [Fact]
        public void CalculateRouteDistance_AtoEtoD_ShouldReturnMinus1()
        {
            var route = new[] { "A", "E", "D" };
            var distance = Program.CalculateRouteDistance(_graph, route);
            Assert.Equal(-1, distance);
        }

        // Test #8: The length of the shortest route from A to C
        [Fact]
        public void FindShortestRoute_AtoC_ShouldReturn9()
        {
            var distance = Program.FindShortestRoute(_graph, "A", "C");
            Assert.Equal(9, distance);
        }
    }
}