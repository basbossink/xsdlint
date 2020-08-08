module XsdLint.Validate

open XsdLint.Types

let isMinOccursZero (attribute: AttributeLocation): Location option =
    match attribute with
    | { Attribute = (MinOccurs(Bounded 0u)); Location = l } -> (Some l)
    | _ -> None

let isOptional (attributes: Attributes): Location list = attributes |> List.choose isMinOccursZero

let isNameEndingWithId (attribute: AttributeLocation): string option =
    match attribute with
    | { Attribute = (Name name); Location = l } when name.EndsWith
                                                         ("Id", System.StringComparison.InvariantCultureIgnoreCase) ->
        (Some name)
    | _ -> None

let nameEndsWithId (attributes: Attributes): string list = attributes |> List.choose isNameEndingWithId

let validateFragments (fragemnts: Schema list) (messages: ValidationMessage list) = messages

let rec validateStartElement
        (startElement: ElementLocation * Attributes * Schema list)
        (messages: ValidationMessage list)
        : ValidationMessage list
    =
    match startElement with
    | ({ Element = XsElement; Location = l }, attrs, rest) ->
        match (nameEndsWithId attrs, isOptional attrs) with
        | ([ name ], [ lopt ]) ->
            validateFragments rest
                (({ RuleIdentifier = RuleIdentifier.IdShouldBeRequired
                    Location = lopt
                    Severity = Severity.Warning
                    Message = sprintf "Entity '%s' has a name ending with 'Id', hence it should be required." name })
                 :: messages)
        | _ -> validateFragments rest messages
    | (_, _, rest) -> validateFragments rest messages

let validate (schema: Schema): ValidationMessage list =
    match schema with
    | StarElement e -> validateStartElement e []
    | _ -> []
