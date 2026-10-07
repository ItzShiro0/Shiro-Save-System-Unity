# Shiro Save System

A lightweight and autonomous save system for Unity based on ScriptableObjects. No external dependencies.

## Features

* **RAM protection (XOR obfuscation):** Stores variables in memory in an obfuscated form, so scanners like Cheat Engine cannot easily find and change them.
* **Disk validation (SHA-256):** Generates a hash for each `.json` save file. If the file is edited manually, the save is reset to default values.
* **ScriptableObject schemas:** Default values are defined in a schema, so loading an empty or new save does not cause a `NullReferenceException`.
* **Custom editor inspector:** Simple UI with protection against accidental asset renaming, which keeps the save database stable.

## Requirements

* Tested on Unity 6.6
* Older versions: not tested

## Installation

1. Download or clone this repository.
2. Copy the `ShiroSaveSystem` folder into your project's `Assets` folder.

## Quick Start

### 1. Create a save schema

1. In Unity, open `Tools -> Save System -> Create New Schema`.

   ![Create a schema](Media/Media/converted.gif)

2. Enter a unique Save ID (for example, `gameplay_save`) and click **Save Name**.

   ![Set the Save ID](Media/Media/converted(1).gif)

3. Add your default variables in the custom inspector.

   ![Add variables](Media/Media/converted(2).gif)

### 2. Use it in code

Use the static `SaveSystem` class. You do not need to look for components on the scene.

```csharp
// Read a value (returns the default from the schema if it is a new game)
int currentCoins = SaveSystem.GetInt("gameplay_save", "Coins");

// Change a value in RAM
SaveSystem.SetInt("gameplay_save", "Coins", currentCoins + 100);

// Write progress to disk
SaveSystem.Save("gameplay_save");

// Reset progress to the schema's default values
SaveSystem.Reset("gameplay_save");
```

## Limitations

This is protection against casual cheating, not a full anti-cheat.

* XOR obfuscation makes values harder to find in memory, but a determined person can still change them.
* The SHA-256 hash stops simple edits of the save file. The salt is stored in the code, so someone who reads your build can calculate a valid hash.
* The API is static for ease of use. This makes it harder to replace in unit tests than an interface-based service.

## License

MIT
