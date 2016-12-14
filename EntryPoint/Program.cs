using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
    public static class Program
    {

        [STAThread]
        static void Main()
        {

            var fullscreen = false;
            read_input:
            switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
            {
                case "1":
                    using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
                        game.Run();
                    break;
                case "2":
                    using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
                        game.Run();
                    break;
                case "3":
                    using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
                        game.Run();
                    break;
                case "4":
                    using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
                        game.Run();
                    break;
                case "q":
                    return;
            }
            goto read_input;
        }


        // The Euclidean distance formula written here
        public static double GetEuclideanDistance(Vector2 house, Vector2 specialBuildings)
        {
            double distance = Math.Sqrt(Math.Pow((house.X - specialBuildings.X), 2) + Math.Pow((house.Y - specialBuildings.Y), 2));
            return distance;
        }

        // List with building positions and house positions.
        private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
        {
            // Add special buildings to the list.
            List<Vector2> specialBuildingsList = specialBuildings.ToList();
            // Make a list with all the distances but not sorted
            List<double> unsortedDistancesList = new List<double>();
            Console.WriteLine("Here is a list with all the buildings and distances unsorted");
            //For the whole list do: distance; write: writeline; Add the distance to the unsortedList.
            for (int i = 0; i < specialBuildings.Count(); i++)
            {
                double distance = GetEuclideanDistance(house, specialBuildingsList[i]);
                Console.WriteLine("Building coordinate: " + specialBuildingsList[i] + " Distance: " + distance);
                unsortedDistancesList.Add(distance);
            }
            // Make a list with all the distances but now sorted
            List<double> sortedDistancesList = new List<double>();

            // Using the recursive MergeSort starting with 0 and ending with The whole list of unsortedDistances - 1.
            MergeSort_Recursive(unsortedDistancesList, 0, unsortedDistancesList.Count() - 1);
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Here is a list with all the buildings in order of most nearby");

            //For loop: for every Unsorted distance it will be added to the sortedDistance List.
            for (int i = 0; i < unsortedDistancesList.Count; i++)
            {
                sortedDistancesList.Add(unsortedDistancesList[i]);
            }
            //For loop: looping through all the elemenst in sortedDistances 
            for (int i = 0; i < sortedDistancesList.Count; i++)
            {
                //For loop: for the whole list calculate the distance for house -> specialbuilding.
                for (int j = 0; j < specialBuildings.Count(); j++)
                {
                    double distance = GetEuclideanDistance(house, specialBuildingsList[j]);
                    // If calculated distance is correct to distance of house --> building it will be printed.
                    if (unsortedDistancesList[i] == distance)
                    {
                        Console.WriteLine("Distance: " + unsortedDistancesList[i] + "    |     Coordinates: " + specialBuildingsList[j]);
                    }
                }
            }
            return specialBuildings;
        }

        // List with specialbuildings and the radius(housesAndDistance from the specialbuilding.
        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            Console.WriteLine("Building coordinate: " + specialBuildings + " Distance: " + housesAndDistances);

            List<Vector2> specialBuildingsList = specialBuildings.ToList();
            Console.WriteLine("Aantal specialbuildings: " + specialBuildings.Count());
            List<Vector2> coordinates = housesAndDistances.Select(t => t.Item1).ToList();
            List<float> distances = housesAndDistances.Select(t => t.Item2).ToList();

            for (int i = 0; i < housesAndDistances.Count(); i++)
            {
                Console.WriteLine(" House no.: " + i + "   with the coordinates " + coordinates[i] + "   has a distance of: " + distances[i]);
            }

            for (int i = 0; i < specialBuildings.Count(); i++)
            {
                for (int j = 0; j < coordinates.Count(); j++)
                {
                    if ((Vector2.Distance(specialBuildingsList[i], coordinates[j])) < distances[j])
                    {
                        Console.WriteLine("Special Building: " + specialBuildingsList[i] + "   with distance of: " + Vector2.Distance(specialBuildingsList[i], coordinates[j]) + "   from house " + coordinates[j] + " geeft    "+ distances[j]);
                    }
                }    
            }

            return
                from h in housesAndDistances
                select
                  from s in specialBuildings
                  where Vector2.Distance(h.Item1, s) <= h.Item2
                  select s;

        }

        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding,
          Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
            List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
            var prevRoad = startingRoad;
            for (int i = 0; i < 30; i++)
            {
                prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
                fakeBestPath.Add(prevRoad);
            }
            return fakeBestPath;
        }

        private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding,
          IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
            foreach (var d in destinationBuildings)
            {
                var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
                List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
                var prevRoad = startingRoad;
                for (int i = 0; i < 30; i++)
                {
                    prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
                    fakeBestPath.Add(prevRoad);
                }
                result.Add(fakeBestPath);
            }
            return result;
        }



        //MERGE
        static public void MergeSort_Recursive(List<double> unsortedDistancesList, int left, int right)
        {
            {
                int mid;
                if (right > left)
                {
                    mid = (right + left) / 2;
                    MergeSort_Recursive(unsortedDistancesList, left, mid);
                    MergeSort_Recursive(unsortedDistancesList, (mid + 1), right);

                    DoMerge(unsortedDistancesList, left, (mid + 1), right);
                }
            }
        }


        static public void DoMerge(List<double> unsortedDistancesList, int left, int mid, int right)

        {
            double[] temp = new double[unsortedDistancesList.Count];
            int i, left_end, num_elements, tmp_pos;

            left_end = (mid - 1);
            tmp_pos = left;
            num_elements = (right - left + 1);

            while ((left <= left_end) && (mid <= right))
            {
                if (unsortedDistancesList[left] <= unsortedDistancesList[mid])
                    temp[tmp_pos++] = unsortedDistancesList[left++];
                else
                    temp[tmp_pos++] = unsortedDistancesList[mid++];
            }

            while (left <= left_end)
                temp[tmp_pos++] = unsortedDistancesList[left++];

            while (mid <= right)
                temp[tmp_pos++] = unsortedDistancesList[mid++];

            for (i = 0; i < num_elements; i++)
            {
                unsortedDistancesList[right] = temp[right];
                right--;
            }
        }


#endif
    }
}
