module ``Test validation rules``

open Xunit
open FsUnit.Xunit
open XsdLint.Types
open XsdLint.Validate

[<Fact>]
let ``Elements with a name ending with Id should be required test``() =
    let input =
        (StarElement
            ({ Element = XsElement
               Location =
                   { Line = 37u
                     Column = 45u } },
             [ { Attribute = (MinOccurs(Bounded 0u))
                 Location =
                     { Line = 37u
                       Column = 48u } } ], []))
    let expected =
        [ { Location =
                { Line = 37u
                  Column = 42u }
            RuleIdentifier = RuleIdentifier.IdShouldBeRequired
            Severity = Error
            Message = "Entities that have a name ending with 'Id' should be requiered" } ]

    let actual = validate input
    actual |> should equal expected
