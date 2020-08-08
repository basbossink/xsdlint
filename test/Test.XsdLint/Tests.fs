module ``Test validation rules``

open Xunit
open FsUnit.Xunit
open XsdLint.Types
open XsdLint.Validate

[<Fact>]
let `` Elements with a name ending with Id should be required test``() =
    let input =
        (StarElement
            ({ Element = XsElement
               Location =
                   { Line = 37u
                     Column = 45u } },
             [ { Attribute = (Name "testId")
                 Location =
                     { Line = 37u
                       Column = 48u } }
               { Attribute = (MinOccurs(Bounded 0u))
                 Location =
                     { Line = 37u
                       Column = 58u } } ], []))
    let expected =
        [ { Location =
                { Line = 37u
                  Column = 58u }
            RuleIdentifier = RuleIdentifier.IdShouldBeRequired
            Severity = Warning
            Message = "Entity 'testId' has a name ending with 'Id', hence it should be required." } ]

    let actual = validate input
    actual |> should equal expected
