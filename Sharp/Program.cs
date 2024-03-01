// See https://aka.ms/new-console-template for more information
using Sharp;

Console.WriteLine("Hello, World!");
Console.WriteLine("What's your name?");

var hello = Console.ReadLine();
Console.WriteLine(hello);

// data type
string name = "White";
Console.WriteLine(name);

char sex = 'M';
Console.WriteLine(sex);

int age = 30;
Console.WriteLine(age);

double height = 179.5;
Console.WriteLine(height);

bool is_male = false;
Console.WriteLine(is_male);

Console.WriteLine("Someone called " + name);
Console.WriteLine("age: {0}", age);

int[] scores = { 20, 30, 40, 59, 78 };
Console.WriteLine("[{0}]", string.Join(", ", scores));
Array.ForEach(scores, Console.WriteLine);

// branch
if (is_male)
{
    Console.WriteLine("The person is male");
}
else
{
    Console.WriteLine("The person is female");
}

// loop
int num = 0;
while (num < 5)
{
    Console.WriteLine(num++);
}

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

// array
int[,] arr = { { 1, 2, 3}, { 4, 5, 6}, { 7, 8, 9} };
// Console.WriteLine("[{0}]", string.Join(", ", arr));
Console.WriteLine(arr[1, 1]);

// object
Person mark = new Person(1, "Mark", 23);

Console.WriteLine(mark);
mark.Show_name();

Student lee = new Student(1, "Lee", 35);
lee.Show_name();

// linq
lee.Linq();
