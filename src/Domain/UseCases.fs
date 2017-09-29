namespace Domain

open System
open DomainTypes

module UseCases =
    let stringCannotBeEmpty x =
        if String.IsNullOrWhiteSpace(x) then
            Error "Empty string"
        else
            Ok x

    let validateCategoryName x =
        match stringCannotBeEmpty x.name with
        | Ok _ -> Ok x
        | Error _ -> Error "Name cannot be empty"
    
    let validateCategorySlug x =
        match stringCannotBeEmpty x.slug with
        | Ok _ -> Ok x
        | Error _ -> Error "Slug cannot be empty"
    
    let validateCategoryDescription x =
        match stringCannotBeEmpty x.description with
        | Ok _ -> Ok x
        | Error _ -> Error "Description cannot be empty"
    
    let validateCreateCategoryRequest x =
        validateCategoryName x
        |> Result.bind validateCategorySlug
        |> Result.bind validateCategoryDescription

    let createCategory x =
        validateCreateCategoryRequest x
