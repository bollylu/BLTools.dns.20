 BLTOOLS
Library of classes for generic support in .NET

## Compiled for 
- .NET standard 2.0
- .NET standard 2.1
- .NET 5.0

## Content
  ### Extension methods
  > Extends various existing classes with methods and functions
  - #### ByteArray
  - #### Dictionnary
  - #### IEnumerable by blocks
  - #### IEnumerable tests and extensions
  - #### Date and time
  - #### Binary reader
  - #### Memory streams
  - #### Numbers
  - #### Tasks
  - #### Processes
  - #### Network
  - #### .NET Reflection
  - #### Strings and chars
  - #### XML
   
- ### Logging
- ### Command line arguments
  - #### SplitArgs
    > Allows the handling of command line arguments by their name. Arguments could start with '-' or '/' or even nothing.
    > Arguments are separated by a space. Strings are surrounded by double-quote '"'.
    > The value of the argument is retrieved already converted to the right type. A default value can be specified in case
    > the argument is missing or there is a error in the conversion.
- ### Text
  - #### TextBox
    > Build lines or boxes around a message to make it more readable
- ### Security
  - #### Encryption
    - ##### Symmetric
      > Encrypt and decrypt data with a symmetric key
    - ##### Asymmetric
      > Encrypt and decrypt data with a public/private key pair.
  - #### Hashing
    > Calculate the hash values for data
- ### Actions and functions
- ### Console extensions
- ### Data structures
- ### Debugging
- ### MVVM
- ### Interfaces
  - #### IToXml
    > The class must be able to transform itself to Xml (XElement) and to fill its content from a source Xml (XElement)
  - #### IToCsv
    > The class must be able to transform itself to Csv (string) and to fill its content from a source Csv (string)
  - #### IToJson
    > The class must be able to transform itself to Json (string) and to fill its content from a source Json (string)

