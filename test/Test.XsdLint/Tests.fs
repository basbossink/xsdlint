module ``Test validation rules``

open Xunit
open FsUnit.Xunit
open XsdLint.Types
open XsdLint.Validate

let optionalElemntEndingInId() =
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

let expectedIdShouldBeRequired() =
    [ { Location =
            { Line = 37u
              Column = 58u }
        RuleIdentifier = RuleIdentifier.IdShouldBeRequired
        Severity = Warning
        Message = "Entity 'testId' has a name ending with 'Id', hence it should be required." } ]


[<Fact>]
let `` Elements with a name ending with Id should be required test``() =
    let actual = optionalElemntEndingInId() |> validate
    actual
    |> should equal
    <| expectedIdShouldBeRequired()

[<Fact>]
let `` Elements with a name ending with Id nested somewhere in the document should be required test``() =
    let input =
        (StarElement
            ({ Element = XsComplexType
               Location =
                   { Line = 3u
                     Column = 45u } }, [], [ optionalElemntEndingInId() ]))

    let actual = validate input
    actual
    |> should equal
    <| expectedIdShouldBeRequired()
