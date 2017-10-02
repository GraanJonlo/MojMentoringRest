namespace Domain

module Infrastructure =
    open System

    let trim (x:string) =
        x.Trim()
    
    let toLower (x:string) =
        x.ToLowerInvariant()

    let stringCannotBeEmpty x =
        if String.IsNullOrWhiteSpace(x) then
            Error "Empty string"
        else
            Ok x
    
    type ResultBuilder() =
        member this.Bind(x, f) =
            match x with
            | Error x -> Error x
            | Ok x -> f x

        member this.Return(x) =
            Ok x
