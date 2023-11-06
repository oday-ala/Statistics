using System;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Array
        int[] array = new int[] { 1, 2, 3, 4, 5 };
        // Loop
        foreach (int num in array)
        {
            Console.WriteLine(num);
            // Break/Continue can be used here
        }
        // Add
        int[] newArray = new int[array.Length + 1];
        Array.Copy(array, newArray, array.Length);
        newArray[newArray.Length - 1] = 6;
        // Remove
        newArray = Array.FindAll(newArray, val => val != 3);
        // Get
        int element = newArray[2];
        // Set
        newArray[1] = 10;
        // Check existence of key/value
        bool exists = Array.Exists(newArray, val => val == 5);
//###########################################################
        // List
        List<int> list = new List<int> { 1, 2, 3, 4, 5 };
        // Loop
        foreach (int num in list)
        {
            Console.WriteLine(num);
            // Break/Continue can be used here
        }
        // Add
        list.Add(6);
        // Remove
        list.Remove(3);
        // Get
        int listElement = list[2];
        // Set
        list[1] = 10;
        // Check existence of key/value
        bool listExists = list.Contains(5);
//###########################################################

        // Dictionary
        Dictionary<string, int> dictionary = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };
        // Loop
        foreach (var item in dictionary)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
            // Break/Continue can be used here
        }
        // Add
        dictionary.Add("four", 4);
        // Remove
        dictionary.Remove("two");
        // Get
        int dictValue;
        bool keyExists = dictionary.TryGetValue("three", out dictValue);
        // Set
        dictionary["one"] = 10;
        // Check existence of key/value
        bool dictExists = dictionary.ContainsKey("four");
//###########################################################

        // SortedList
        SortedList<int, string> sortedList = new SortedList<int, string>
        {
            { 3, "three" },
            { 1, "one" },
            { 2, "two" }
        };
        // Loop
        foreach (var item in sortedList)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
            // Break/Continue can be used here
        }
        // Add
        sortedList.Add(4, "four");
        // Remove
        sortedList.Remove(2);
        // Get
        string sortedValue = sortedList[3];
        // Set
        sortedList[1] = "updated one";
        // Check existence of key/value
        bool sortedExists = sortedList.ContainsValue("four");
//###########################################################

        // HashSet
        HashSet<int> hashSet = new HashSet<int> { 1, 2, 3, 4, 5 };
        // Loop
        foreach (int num in hashSet)
        {
            Console.WriteLine(num);
            // Break/Continue can be used here
        }
        // Add
        hashSet.Add(6);
        // Remove
        hashSet.Remove(3);
        // Check existence of key/value
        bool hashSetExists = hashSet.Contains(5);

        // SortedSet
        SortedSet<int> sortedSet = new SortedSet<int> { 3, 1, 2, 5, 4 };
        // Loop
        foreach (int num in sortedSet)
        {
            Console.WriteLine(num);
            // Break/Continue can be used here
        }
        // Add
        sortedSet.Add(6);
        // Remove
        sortedSet.Remove(3);
        // Check existence of key/value
        bool sortedSetExists = sortedSet.Contains(5);
//###########################################################

        // Queue
        Queue<string> queue = new Queue<string>();
        queue.Enqueue("first");
        queue.Enqueue("second");
        queue.Enqueue("third");
        // Loop
        foreach (string item in queue)
        {
            Console.WriteLine(item);
            // Break/Continue can be used here
        }
        // Add
        queue.Enqueue("fourth");
        // Remove
        string dequeuedItem = queue.Dequeue();
        // Check existence of key/value
        queue.Contains(5);

//###########################################################

        // Stack
        Stack<string> stack = new Stack<string>();
        stack.Push("first");
        stack.Push("second");
        stack.Push("third");
        // Loop
        foreach (string item in stack)
        {
            Console.WriteLine(item);
            // Break/Continue can be used here
        }
        // Add
        stack.Push("fourth");
        // Remove
        string poppedItem = stack.Pop();
        // Check existence of key/value
        stack.Contains(1);
//###########################################################

        // LinkedList
        LinkedList<int> linkedList = new LinkedList<int>();
        linkedList.AddLast(1);
        linkedList.AddLast(2);
        linkedList.AddLast(3);
        // Loop
        foreach (int num in linkedList)
        {
            Console.WriteLine(num);
            // Break/Continue can be used here
        }
        // Add
        linkedList.AddLast(4);
        // Remove
        linkedList.Remove(2);
        // Check existence of key/value 
        linkedList.Contains(9);
        
    }
}
