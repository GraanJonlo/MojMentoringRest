namespace Domain

module DomainTypes =
    type Uuid = Uuid of string

    type CreateCategory = {
        id: string
        name: string
        slug: string
        description: string
        status:string
    }
