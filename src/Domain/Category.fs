namespace Domain

module Category =
    open System
    open Infrastructure

    type Dto = {
        id: string
        name: string
        slug: string
        description: string
        status: string
    }

    type Category = private {
        id: Uuid.Uuid
        name: BoundedString.String50
        slug: BoundedString.String50
        description: BoundedString.String250
        status: Status
    }

    let create x =
        let validate x =
            let validateName (x:Dto) =
                let notEmpty (x:Dto) =
                    match stringCannotBeEmpty x.name with
                    | Ok _ -> Ok x
                    | Error _ -> Error "Name cannot be empty"
                
                let notTooLong (x:Dto) =
                    if x.name.Length <= 50 then
                        Ok x
                    else
                        Error "Name too long"
                
                notEmpty x
                |> Result.bind notTooLong
            
            let validateSlug (x:Dto) =
                let notEmpty (x:Dto) =
                    match stringCannotBeEmpty x.slug with
                    | Ok _ -> Ok x
                    | Error _ -> Error "Slug cannot be empty"
                
                let notTooLong (x:Dto) =
                    if x.slug.Length <= 50 then
                        Ok x
                    else
                        Error "Slug too long"
                
                notEmpty x
                |> Result.bind notTooLong
            
            let validateDescription (x:Dto) =
                if x.slug.Length <= 250 then
                    Ok x
                else
                    Error "Description too long"
            
            let validateStatus (x:Dto) =
                match x.status with
                | "draft" -> Ok x
                | "live" -> Ok x
                | "" -> Ok x
                | null -> Ok x
                | _ -> Error "Unrecognised status"
                
            validateName x
            |> Result.bind validateSlug
            |> Result.bind validateDescription
            |> Result.bind validateStatus

        let canonicalize (x:Dto) =
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
            
            Ok {
                x with
                    id = Guid.NewGuid().ToString()
                    name = canonicalizeName x.name
                    slug = canonicalizeSlug x.slug
                    description = canonicalizeDescription x.description
                    status = canonicalizeStatus x.status}
        
        let create' (x:Dto) =
            let result = ResultBuilder()
            result {
                let! id = Uuid.create x.id
                let! name = BoundedString.create50 x.name
                let! slug = BoundedString.create50 x.slug
                let! description = BoundedString.create250 x.description
                return { id=id; name=name; slug=slug; description=description; status = Draft }
            }
        
        validate x
        |> Result.bind canonicalize
        |> Result.bind create'

    let dto x =
        {
            Dto.id = Uuid.value x.id
            name = BoundedString.value50 x.name
            slug = BoundedString.value50 x.slug
            description = BoundedString.value250 x.description
            status =
                match x.status with
                | Draft -> "draft"
                | Live -> "live"}
