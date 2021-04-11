Necromancy - Server
===
Server Emulator for the Online Game Wizardry Online.

## Table of contents
- [Disclaimer](#disclaimer)
- [Wiki](#wiki)
- [Setup](#setup)
  - [Visual Studio](#visual-studio)
  - [VS Code](#vs-code)
  - [IntelliJ Rider](#intellij-rider)
- [Project](#project)
  - [Id Types](#id-types)
  - [Instance](#instance)  
  - [Item](#item)
- [Guidelines](#guidelines)
- [Attribution](#attribution)
  - [Contributers](#contributers)
  - [3rd Parties and Libraries](#3rd-parties-and-libraries)

# Disclaimer
The project is intended for educational purpose only.

# Wiki
- Index: https://github.com/necromancyonline/necromancy-server/wiki
- ウィキペディア: https://github.com/necromancyonline/necromancy-server/wiki/%E5%85%A5%E9%96%80

Please check the wiki for additional information.

# Setup
## 1) Clone the repository  
`git clone https://github.com/necromancyonline/necromancy-server.git`

## 2) Install .Net Core 3.0 SDK or later  
https://dotnet.microsoft.com/download

## 3) Use your IDE of choice:

## 3.1) Visual Studio
### Notice:
Minimum version of "Visual Studio 2019 v16.3" or later.

### Open Project:
Open the `necromancy.sln`-file

## 3.2) VS Code
Download IDE: https://code.visualstudio.com/download  
C# Plugin: https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp  

### Open Project:
Open the Project Folder:  
`\necromancy-server`

## 3.3) IntelliJ Rider
https://www.jetbrains.com/rider/

### Open Project:  
Open the `necromancy.sln`-file

## 4) Debug the Project
Run the `Necromancy.Cli`-Project

# Project
The following section will explain some concepts and systems that have been designed and explain the intentions.
This should help developers to gain high level overview of existing components and how to make best use of them.

## Id Types
| Name       | Type       | Uniqueness | Constant | Description                                                                                             |
|:-----------|:-----------|:-----------|:---------|:--------------------------------------------------------------------------------------------------------|
| InstanceId | ObjectId   | Globally   | No       | Identifies a object uniquely globally (No duplicate Ids are possible, can distinguish between anything) |
| Id         | DatabaseId | per Domain | Yes      | Identifies a object uniquely among its domain. (Can distinguish between two npcs with the same NpcId)   |
| NpcId      | SerialId   | None       | Yes      | Identifies a type of npc (Multiple npc can exist with same NpcId)                                       |
| MonsterId  | SerialId   | None       | Yes      | Identifies a type of monster (Multiple monster can exist with same MonsterId)                           |
| ModelId    | SerialId   | None       | Yes      | Identifies a type of model (Multiple model can exist with same ModelId)                                 |

## Instance
Inside the game world various kinds of objects exist.
The game uses ids to identify a specific object inside the game world.
This brings the requirement that every id needs to be globally unique, to identify a specific object.
For example there could be two monsters, who look identically but to differentiate them we need to give them different ids.
These ids are called `InstanceId` because they point to a specific instance of an object inside the game world.
Every object that exists inside the game world needs to be assigned an unique `InstanceId` so we can identify the object that is being interacted with.
  
In general and during development you should not concern yourself about these `InstanceId`s they are supposed to be randomly and on demand assigned.
That means after a server restart, every object inside the game world could have a different `InstanceId`.

We are using concrete objects to represent an object inside the game world, for example a `Item.cs` or `Monster.cs` class.
These classes when instantiated will be assigned an `InstanceId`. 
While the `InstanceId` is assigned, the server will keep a journal of object instance reference to `InstanceId`. 
When a monster spawn packet is send, the `InstanceId` of the instantiated `Monster.cs` class will be added to the packet.
The moment a player will attack this monster, we will receive the `InstanceId` in our handling method,
then the journal can be used to find the original `Monster.cs` class instance by the provided `InstanceId`.

To summarize the Instance-System is used to create journal entries / lookups for `InstanceId` to object instance.
Additionally it provides the ability to retrieve the object instance by `InstanceId`.

This is why the `InstanceId` should never be used in any comparison (`InstanceId != 0`) or stored anywhere.
It should exclusively be used when sending packet data (by appending it) and when handling packet data (by looking the object up).

```
uint instanceId = packet.ReadUInt32();
IInstance instance = Server.Instances.GetInstance(instanceId);
if(instance == null)
{
  // the instanceId is invalid
} else if (instance is Character character)
{
  // the instanceId is a valid Character class object
  // access character properties
  Logger.Info($"CharacterName: {character.Name}")
} else if (instance is Monster monster)
{
  // the instanceId is a valid Monster class object
  // access monster properties
  Logger.Info($"MonsterName: {monster.Name}")
}
```
Looking up a `InstanceId` will return a `IInstance` object.
If the object is null, it means the instance is invalid or does not exist anymore.
Usually `IInstance` is expected to be of a certain type, for example in a monster attack handler check
if it is a valid monster instance (`if (instance is Monster monster)`) and continue handling by utilizing the `monster` instance.
If the check failed it just means the instance is not valid anymore and it can be logged or dealt with in an appropriate way.

In a case where it is expected that an `InstanceId` can be multiple types of an object, the above if-else construct can be used to
test for multiple types of object instances and handle them accordingly.

If dealing with an `InstanceId` only use it to retrieve the expected object, and then use that object. 
Do not use `InstanceId` for anything else other than (writing to packet, reading from packet, looking object up).

## Items

### Item
It holds basic static default values of an item.


### InventoryItem
Represents an actual `Item` that belongs to a `Character`
The `InventoryItem` will hold values that are unique to the item.

# Guidelines
## Git 
### Workflow
The work on this project should happen via `feature-branches`
   
Feature branches (or sometimes called topic branches) are used to develop new features for the upcoming or a distant future release. 
When starting development of a feature, the target release in which this feature will be incorporated may well be unknown at that point. 
The essence of a feature branch is that it exists as long as the feature is in development, 
but will eventually be merged back into develop (to definitely add the new feature to the upcoming release) or discarded (in case of a disappointing experiment).
   
1) Create a new `feature/feature-name` or `fix/bug-fix-name` branch from master
2) Push all your changes to that branch
3) Create a Pull Request to merge that branch into `master`

## Best Practise
- Do not use Console.WriteLine etc, use the specially designed logger.
- Own the Code: extract solutions, discard libraries.
- Annotate functions with documentation comments (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments).

## C# Coding Standards and Naming Conventions
| Object Name               | Notation    | Char Mask          | Underscores |
|:--------------------------|:------------|:-------------------|:------------|
| Class name                | PascalCase  | [A-z][0-9]         | No          |
| Constructor name          | PascalCase  | [A-z][0-9]         | No          |
| Method name               | PascalCase  | [A-z][0-9]         | No          |
| Method arguments          | camelCase   | [A-z][0-9]         | No          |
| Local variables           | camelCase   | [A-z][0-9]         | No          |
| Constants name            | PascalCase  | [A-z][0-9]         | No          |
| Field name                | _camelCase  | [A-z][0-9]         | Yes         |
| Properties name           | PascalCase  | [A-z][0-9]         | No          |
| Delegate name             | PascalCase  | [A-z]              | No          |
| Enum type name            | PascalCase  | [A-z]              | No          |

## SQL Style
- Table, Column, and Indices names are all lower case with underscore ( _ ) separating the names

# Attribution
## Contributors
- Unknownone69 [@Unknownone69](https://github.com/Unknownone69) 
- Nothilvien [@sebastian-heinz](https://github.com/sebastian-heinz)
- WizOnHiraeth [@WizOnHiraeth](https://github.com/WizOnHiraeth) 
- blackice36 [@blackice36](https://github.com/blackice36) 
- Aidoxilia [@Aidoxilia](https://github.com/Aidoxilia) 
- taewyth [@taewyth](https://github.com/taewyth) 
- koffeeMist [@koffeeMist](https://github.com/koffeeMist) 
- WIZONMAGLOVE [@WIZONMAGLOVE](https://github.com/WIZONMAGLOVE) 
- Wolfzennn [@Wolfzennn](https://github.com/Wolfzennn)
- Raezaiel [@Raezaiel](https://github.com/Raezaiel) 

## 3rd Parties and Libraries
- System.Data.SQLite (https://system.data.sqlite.org/)
- MySqlConnector (https://www.nuget.org/packages/MySqlConnector)
- bcrypt.net (https://github.com/BcryptNet/bcrypt.net)
- .NET Standard (https://github.com/dotnet/standard)
- Arrowgene.Services (https://github.com/Arrowgene/Arrowgene.Services)

