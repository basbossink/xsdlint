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

let buildIdShouldBeRequired (name: string) (lopt: Location): ValidationMessage =
    { RuleIdentifier = RuleIdentifier.IdShouldBeRequired
      Location = lopt
      Severity = Severity.Warning
      Message = sprintf "Entity '%s' has a name ending with 'Id', hence it should be required." name }

let consIdShouldBeRequired (attrs: Attributes) (messages: ValidationMessage list): ValidationMessage list =
    match (nameEndsWithId attrs, isOptional attrs) with
    | ([ name ], [ lopt ]) -> (buildIdShouldBeRequired name lopt) :: messages
    | _ -> messages

let rec validateStartElement
        (startElement: ElementLocation * Attributes * Schema list)
        (messages: ValidationMessage list)
        : ValidationMessage list
    =
    match startElement with
    | ({ Element = XsElement; Location = _ }, attrs, rest) ->
        rest |> List.fold (fun m s -> validateFragment s m) (consIdShouldBeRequired attrs messages)
    | (_, _, rest) -> rest |> List.fold (fun m s -> validateFragment s m) messages

and validateFragment (schema: Schema) (messages: ValidationMessage list): ValidationMessage list =
    match schema with
    | StarElement e -> validateStartElement e []
    | _ -> []

let validate (schema: Schema): ValidationMessage list = validateFragment schema []
