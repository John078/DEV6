using EntryPoint;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
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



        //----------------------------------   Assignment 2 ---------------------------------//


        //j is current value and k is the new to inserting value
        static KDTree<Vector2> Insert(KDTree<Vector2> j, Vector2 k)
        {
            if (j.IsVertical)
            {
                //if the new value k is smaller then j he moves left. Vertical becomes false.
                if (j.Value.X > k.X)
                {
                    if (j.Left.IsEmpty)
                    {
                        //For the left branch of the tree
                        //Make a new node when its empty. Left branch and right branch Empty with the new value k and Vertical becomes false.
                        j.Left = new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), k, false);
                    }
                    else
                    {
                        //Not empty so grab the node that is left and Insert it with the new value
                        j.Left = Insert(j.Left, k);
                    }
                }
                else
                {
                    if (j.Right.IsEmpty)
                    {
                        //For the Right branch of the tree
                        //Make a new node when its empty. Left branch and right branch Empty with the new value k and Vertical becomes false.
                        j.Right = new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), k, false);
                    }
                    else
                    {
                        //Not empty so grab the node that is right and Insert it with the new value.
                        j.Right = Insert(j.Right, k);
                    }
                }
            }
            else
            {
                //als de nieuwe value kleiner is dan j gaat hij naar links. Vertical wordt true.
                if (j.Value.Y > k.Y)
                { 
                    if (j.Left.IsEmpty)
                    {
                        //For the left branch of the tree
                        //Make a new node when its empty. Left branch and right branch Empty with the new value k and Vertical becomes false.
                        j.Left = new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), k, true);
                    }
                    else
                    {
                        //Not empty so grab the node that is left and Insert it with the new value
                        j.Left = Insert(j.Left, k);
                    }
                }
                else
                {
                    if (j.Right.IsEmpty)
                    {
                        //For the Right branch of the tree
                        //Make a new node when its empty. Left branch and right branch Empty with the new value k and Vertical becomes false.
                        j.Right = new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), k, true);
                    }
                    else
                    {
                        //Not empty so grab the node that is right and Insert it with the new value
                        j.Right = Insert(j.Right, k);
                    }
                }
            }
            return j;            
        }

        static void Search(KDTree<Vector2> j, Vector2 house, float distance, List<Vector2> result)
        {
            if (j.IsEmpty)
            {
                Console.WriteLine("Empty");
            }
            else
            {
                if (j.Value == house)
                {
                    Console.WriteLine("OK");
                }
                else
                {
                    if (j.IsVertical) //X-as
                    {
                        if (Math.Abs(j.Value.X - house.X) <= distance)
                        {
                            if (Vector2.Distance(j.Value, house) <= distance)
                            {
                                result.Add(j.Value);
                            }
                            Search(j.Left, house, distance, result);
                            Search(j.Right, house, distance, result);
                        }
                        else if (j.Value.X > (house.X + distance))
                        {
                            Search(j.Left, house, distance, result);
                        }
                        else if (j.Value.X < (house.X - distance))
                        {
                            Search(j.Right, house, distance, result);
                        }

                    }
                    else //Y-as
                    {
                        if (Math.Abs(j.Value.Y - house.Y) <= distance)
                        {
                            if (Vector2.Distance(j.Value, house) <= distance)
                            {
                                result.Add(j.Value);
                            }

                            Search(j.Left, house, distance, result);
                            Search(j.Right, house, distance, result);
                        }
                        else if (j.Value.Y > (house.Y + distance))
                        {
                            Search(j.Left, house, distance, result);
                        }
                        else if (j.Value.Y < (house.Y - distance))
                        {
                            Search(j.Right, house, distance, result);
                        }
                    }
                }
            }
        }


        // List with specialbuildings and the radius(housesAndDistance from the specialbuilding.
        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            List<Vector2> specialBuildingsList = specialBuildings.ToList();
            List<List<Vector2>> final_result = new List<List<Vector2>>();
            
            KDTree<Vector2> j = null;
            for (int i = 0; i < specialBuildings.Count(); i++)
            {
                if (j == null)
                {
                    j = new Node<Vector2>(new Empty<Vector2>(), new Empty<Vector2>(), specialBuildingsList[i], true);
                }
                else
                {
                    j = Insert(j, specialBuildingsList[i]);
                }
            }

            foreach (Tuple<Vector2, float> h in housesAndDistances)
            {
                for (int i = 0; i < specialBuildings.Count(); i++)
                {
                    Console.WriteLine("Special Building: " + specialBuildingsList[i]);                    
                }

                List<Vector2> result = new List<Vector2>();
                Search(j, h.Item1, h.Item2, result);
                final_result.Add(result);
            }
            return final_result;
        }




        //----------------------------------   Assignment 3   ---------------------------------//
       // NOT WORKING CODE ...
       



        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding,
          Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {


            var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
            List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
            List<Tuple<Vector2, Vector2>> allroads = roads.ToList();
            List<Tuple<Vector2, Vector2>> neeew = allroads;

            var prevRoad = startingRoad;

            var StartX = startingBuilding.X;
            var StartY = startingBuilding.Y;

            //int beginX = int(BeginStartRoad.X);
            //int beginY = BeginStartRoad.Y;


            for (int i = 0; i < 30; i++)
            {
                prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
                fakeBestPath.Add(prevRoad);
                Console.WriteLine(allroads[i]);
            }
            
            Console.WriteLine(startingBuilding);
            Console.WriteLine(StartX);
            Console.WriteLine(destinationBuilding);
            Console.WriteLine(allroads[2].Item1);
            Console.WriteLine(prevRoad);

            return fakeBestPath;
        }

        private static int Minimumdistance(int[] distance, bool[] shortest, int count)
        {
            int inf = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < count; v++)
            {
                if (shortest[v] == false && distance[v] <= inf)
                {
                    inf = distance[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }

        private static void Print(int[] distance, int count)
        {
            Console.WriteLine("Vertex      distance from start");

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("{0}\t  {1}", i, distance[i]);
            }
        }


        //public static void Dijkstra(int[,] graph, int source, int count)
        //{
        //    int[] distance = new int[count];
        //    bool[] shortest = new bool[count];

        //    for (int i = 0; i < count; i++)
        //    {
        //        distance[i] = int.MaxValue;
        //        shortest[i] = false;
        //    }

        //    distance[source] = 0;

        //    for (int sum = 0; sum < count - 1; sum++)
        //    {
        //        int u = Minimumdistance(distance, shortest, count);
        //        shortest[u] = true;

        //        for (int v = 0; v < count; v++)
        //            if (!shortest[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
        //                distance[v] = distance[u] + graph[u, v];
        //    }
        //    Print(distance, count);
        //}





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

    internal class T
    {
    }
    
}