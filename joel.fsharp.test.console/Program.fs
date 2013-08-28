#light

// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

let sqr x = x * x

let sumOfSquares list =
    list
    |> Seq.map sqr
    |> Seq.sum    

[<EntryPoint>]
let main2 argv = 
    printfn "%A hello" argv
    0 // return an integer exit code
    