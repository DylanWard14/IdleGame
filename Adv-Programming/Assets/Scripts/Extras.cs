using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script was added to show i could use a dictionary

public class Extras : MonoBehaviour
{
    public Dictionary<int, User> Users = new Dictionary<int, User>(); // creating a new dictionary
    Queue<User> userQueue = new Queue<User>();
    Stack<User> userStack = new Stack<User>();
    // Use this for initialization
    void Start ()
    {
        AddUsers(); // add the users to the dictionary
        foreach (KeyValuePair<int, User> userKey in Users) // loop through all useres in the dictionary
        {
            Debug.Log("ID: " + userKey.Value.UserID + " Username: " + userKey.Value.UserName); // print their name and their id
            Debug.Log("Test");
        }

        PrintUsersFromQueueAndAddToStack();

        RemoveAndPrintUsersFromStack();

        MaleUser male = new MaleUser("Steve");
        FemaleUser female = new FemaleUser("Veronica");
        male.Speak();
        female.Speak();
    }

    void AddUsers()
    {
        User user1 = new User(1, "Tron"); // create new users
        User user2 = new User(2, "Dylan");
        User user3 = new User(3, "Taylah");
        User user4 = new User(4, "Justin Bieber");

        Users.Add(user1.UserID, user1); // add them to the dictionary
        Users.Add(user2.UserID, user2);
        Users.Add(user3.UserID, user3);
        Users.Add(user4.UserID, user4);

        AddUsersToQueue();
    }

    void AddUsersToQueue()
    {
        userQueue.Enqueue(Users[1]); // add the users to the queue from the dictionary
        userQueue.Enqueue(Users[2]);
        userQueue.Enqueue(Users[3]);
        userQueue.Enqueue(Users[4]);


    }

    void PrintUsersFromQueueAndAddToStack()
    {
        for (int i = 0; i <= 3; i++) // loop through the dictionary
        {
            User dequeuedUser = userQueue.Dequeue(); // store the dequeued user
            Debug.Log(dequeuedUser.UserName + " Removed from queue and added to stack"); // print the users name to the console
            userStack.Push(dequeuedUser); // added the dequeued user to the stack
        }  
    }

    void RemoveAndPrintUsersFromStack()
    {
        for (int i = 0; i <= 3; i++) // loop through how many items there are in the dictionary
        {
            User poppedUser = userStack.Pop(); // pop a user from the stack
            Debug.Log(poppedUser.UserName + " Removed from stack"); // print the users name to the console
        }
    }

}

// the user class, holds the users id and name
public class User
{
    public User(int id, string name)
    {
        UserID = id;
        UserName = name;
    }

    private int id;
    public int UserID
    {
        get { return id; }
        set { id = value; }
    }

    private string username;
    public string UserName
    {
        get { return username; }
        set { username = value; }
    }

    
}

public abstract class person
{
    public person(string name)
    {
        Name = name;
    }

    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public abstract void Speak(); // all people that inherit from this class must have an implementation of the speak method
}

public class MaleUser : person
{
    public MaleUser(string name) : base(name) // this persons name is set with the base constructor
    {

    }

    public override void Speak()
    {
        Debug.Log("Hello My name is " + Name + " I am a male"); // print name and that they are male
    }
}

public class FemaleUser : person
{
    public FemaleUser(string name) : base(name)
    {

    }
    public override void Speak()
    {
        Debug.Log("Hello My name is " + Name + " I am a female");
    }
}
