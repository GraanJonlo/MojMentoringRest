namespace Domain

open System

module DomainTypes =
    type Uuid = Uuid of string

    type Status =
    | Draft
    | Live

    type CreateCategoryCommand = {
        id: string
        name: string
        slug: string
        description: string
        status: string
    }

    type Category = {
        id: Uuid
        name: string
        slug: string
        description: string
        status: Status
    }

module UseCases =
    open DomainTypes

    let trim (x:string) =
        x.Trim()
    
    let toLower (x:string) =
        x.ToLowerInvariant()

    let stringCannotBeEmpty x =
        if String.IsNullOrWhiteSpace(x) then
            Error "Empty string"
        else
            Ok x

    let validateCategoryName (x:CreateCategoryCommand) =
        match stringCannotBeEmpty x.name with
        | Ok _ -> Ok x
        | Error _ -> Error "Name cannot be empty"
    
    let validateCategorySlug (x:CreateCategoryCommand) =
        match stringCannotBeEmpty x.slug with
        | Ok _ -> Ok x
        | Error _ -> Error "Slug cannot be empty"
    
    let validateCategoryStatus (x:CreateCategoryCommand) =
        match x.status with
        | "draft" -> Ok x
        | "live" -> Ok x
        | "" -> Ok x
        | null -> Ok x
        | _ -> Error "Unrecognised status"
    
    let validateCreateCategoryRequest (x:CreateCategoryCommand) =
        validateCategoryName x
        |> Result.bind validateCategorySlug
        |> Result.bind validateCategoryStatus

    let canonicalizeName =
        trim
    
    let canonicalizeSlug =
        trim >> toLower
    
    let canonicalizeDescription x =
        if isNull x then
            ""
        else
            trim x
    
    let canonicalizeStatus x =
        if isNull x then
            "draft"
        else
            x
    
    let canonicalizeCreateCategoryRequest (x:CreateCategoryCommand) =
        Ok {
            x with
                id = Guid.NewGuid().ToString()
                name = canonicalizeName x.name
                slug = canonicalizeSlug x.slug
                description = canonicalizeDescription x.description
                status = canonicalizeStatus x.status}

    let createCategory (x:CreateCategoryCommand) =
        validateCreateCategoryRequest x
        |> Result.bind canonicalizeCreateCategoryRequest
