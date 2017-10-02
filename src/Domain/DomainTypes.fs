namespace Domain

module BoundedString =
    type String50 = private String50 of string
    type String250 = private String250 of string

    let create50 (x:string) =
        if x.Length <= 50 then
            Ok <| String50 x
        else
            Error "String too long"

    let create250 (x:string) =
        if x.Length <= 250 then
            Ok <| String250 x
        else
            Error "String too long"

    let value50 (String50 x) = x

    let value250 (String250 x) = x

module Uuid =
    open System.Text.RegularExpressions

    type Uuid = private Uuid of string

    let validate x =
        if Regex.IsMatch(x, "^[a-z\d]{8}-[a-z\d]{4}-[a-z\d]{4}-[a-z\d]{4}-[a-z\d]{12}$") then
            Ok x
        else
            Error "Not a valid UUID"

    let create x =
        let create' x =
            Ok <| Uuid x

        validate x
        |> Result.bind create'
    
    let value (Uuid x) = x

type Status =
    | Draft
    | Live
