// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

let x = 4
let y = 5

let sum = x + y


// record

type Person = { Name: string; Age: int }

let person = { Name = "Alice"; Age = 30 }

// function

let add a b = a + b

let number = add x y

let add25 = add 25

let add100 = add25 100 // curried function


// ## functions as first-class citizens
let opNum x1 y1 = x1 y1

let result = opNum (+) 25

let res = result

