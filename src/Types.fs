module XsdLint.Types

type UnqualifiedName = string

type Occurrance =
    | Bounded of uint32
    | Unbounded

type QualifiedName =
    { Namespace: string
      Name: UnqualifiedName }

type Usage =
    | Prohibited
    | Optional
    | Required

type Location =
    { Line: uint32
      Column: uint32 }

type Attribute =
    | Base of QualifiedName
    | MaxOccurs of Occurrance
    | MinOccurs of Occurrance
    | Name of string
    | Nillable of bool
    | Ref of QualifiedName
    | Type of QualifiedName
    | Use of Usage
    | Value of string

type AttributeLocation =
    { Attribute: Attribute
      Location: Location }

type Element =
    | XsAll
    | XsAny
    | XsAnyAttribute
    | XsAppinfo
    | XsAssert
    | XsAssertion
    | XsAttribute
    | XsAttributeGroup
    | XsChoice
    | XsComplexContent
    | XsComplexType
    | XsDocumentation
    | XsElement
    | XsEnumeration
    | XsExplicitTimezone
    | XsExtension
    | XsField
    | XsFractionDigits
    | XsGroup
    | XsImport
    | XsInclude
    | XsKey
    | XsKeyref
    | XsLength
    | XsList
    | XsMaxExclusive
    | XsMaxInclusive
    | XsMaxLength
    | XsMinExclusive
    | XsMinInclusive
    | XsMinLength
    | XsNotation
    | XsOverride
    | XsPattern
    | XsRedefine
    | XsRestriction
    | XsSchema
    | XsSelector
    | XsSequence
    | XsSimpleContent
    | XsSimpleType
    | XsSubstitution
    | XsTotalDigits
    | XsUnion
    | XsUnique

type ElementLocation =
    { Element: Element
      Location: Location }

type Attributes = AttributeLocation list

type Schema =
    | StarElement of (ElementLocation * Attributes * Schema list)
    | Text of string

type Severity =
    | Suggestion
    | Information
    | Warning
    | Error

type RuleIdentifier =
    | IdShouldBeRequired = 1

type ValidationMessage =
    { RuleIdentifier: RuleIdentifier
      Location: Location
      Severity: Severity
      Message: string }
