
//Array
const array = [1, 2, 3, 4, 5];
// Loop (for loop with break/continue)
for (let i = 0; i < array.length; i++) {
    if (array[i] === 3) {
        break; // Exit the loop
    }
    if (array[i] === 2) {
        continue; // Skip the rest of the loop and continue to the next iteration
    }
    console.log(array[i]);
}

// Add/Remove/Get/Set/Check Existence
array.push(6); // Add an element to the end
array.pop();  // Remove an element from the end
array[0];     // Get an element by index
array[1] = 10; // Set the value of an element
array.includes(3); // Check if an element exists in the array (returns true/false)

//#####################################################################
// Dictionary
const dictionary = {
    key1: "value1",
    key2: "value2"
};

// Loop through keys/values
for (const key in dictionary) {
    if (dictionary.hasOwnProperty(key)) {
        const value = dictionary[key];
        console.log(key, value);
    }
}

// Add/Remove/Get/Set/Check Existence
dictionary.key3 = "value3"; // Add a key-value pair
delete dictionary.key2;     // Remove a key-value pair
dictionary.key1;             // Get the value by key
dictionary.key1 = "new value"; // Set the value of a key
dictionary.hasOwnProperty("key1"); // Check if a key exists (returns true/false)
//#####################################################################

//SORTED LIST 
const sortedList = [1, 2, 4, 4, 3];
sortedList.sort();

// LOOP
sortedList.forEach(i => console.log(i));

// ADD
sortedList.push(6);
sortedList.sort();

// REMOVE
index = sortedList.indexOf(2);
index > -1 && sortedList.splice(index, 1);

// GET
array[1];

// SET
index = sortedList.indexOf(3);
index > -1 && sortedList.splice(index, 1, 5);
sortedList.sort();

// EXISTS KEY/VALUE
sortedList.includes(2);


//##########################################################
const hashSet = new Set([1, 2, 3]);

// Loop (for...of loop)
for (const item of hashSet) {
    console.log(item);
}

// Add/Remove/Check Existence
hashSet.add(4);    // Add an element
hashSet.delete(2); // Remove an element
hashSet.has(3);    // Check if an element exists (returns true/false)

//##########################################################
// Queue
const queue = [];
//Loop
while (queue.length > 0) {
    const element = queue.shift();
    // Do something with element
}

// Enqueue/Dequeue
queue.push(1);        // Enqueue an element
const dequeued = queue.shift(); // Dequeue an element from the front
const element = queue[0];//Get (Peek, without removing):



//##########################################################
//stack
const stack = [];

//Loop
while (stack.length > 0) {
    const element = stack.pop();
    
}

// Push/Pop
stack.push(1);      // Push an element onto the stack
const popped = stack.pop(); // Pop an element from the top of the stack
const element = stack[stack.length - 1]; // Get (Peek, without removing):

stack.includes(3);// EXISTS KEY/VALUE




//##########################################################
//Linkedlist
class Node {
    constructor(value) {
        this.value = value;
        this.next = null;
    }
}

class LinkedList {
    constructor() {
        this.head = null;
    }

    // Add a node to the end of the linked list
    add(value) {
        const newNode = new Node(value);
        if (!this.head) {
            this.head = newNode;
        } else {
            let current = this.head;
            while (current.next) {
                current = current.next;
            }
            current.next = newNode;
        }
    }

    // Remove a node by its value
    remove(value) {
        if (!this.head) {
            return;
        }
        if (this.head.value === value) {
            this.head = this.head.next;
            return;
        }
        let current = this.head;
        while (current.next) {
            if (current.next.value === value) {
                current.next = current.next.next;
                return;
            }
            current = current.next;
        }
    }

    // Get a node by its index
    get(index) {
        if (index < 0 || !this.head) {
            return null;
        }
        let current = this.head;
        for (let i = 0; i < index; i++) {
            if (!current.next) {
                return null;
            }
            current = current.next;
        }
        return current.value;
    }

    // Set the value of a node at a specific index
    set(index, value) {
        if (index < 0 || !this.head) {
            return false;
        }
        let current = this.head;
        for (let i = 0; i < index; i++) {
            if (!current.next) {
                return false;
            }
            current = current.next;
        }
        current.value = value;
        return true;
    }

    // Loop through the linked list
    loop() {
        let current = this.head;
        while (current) {
            console.log(current.value);
            current = current.next;
        }
    }
}

// Example usage:
const linkedList = new LinkedList();
linkedList.add(1);
linkedList.add(2);
linkedList.add(3);

linkedList.loop(); // Output: 1, 2, 3
linkedList.remove(2);
linkedList.loop(); // Output: 1, 3
console.log(linkedList.get(1)); // Output: 3
linkedList.set(1, 4);
linkedList.loop(); // Output: 1, 4


